using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniVerlet2D {

	[RequireComponent(typeof(IFormLayer), typeof(CircleCollider2D))]
	public class SimCollider : MonoBehaviour {

		[System.Serializable]
		public class CollisionEvent : UnityEvent<int> { }

		public float particleRadius = 0.1f;
		public float particleForceFeedback = 0.5f;
		public float targetForceFeedback = 0.5f;

		[Header("Event")]
		public CollisionEvent onCollisionEnter;
		public CollisionEvent onCollisionExit;

		CircleCollider2D _trigger;
		Simulator _sim;
		Dictionary<CircleCollider2D, Rigidbody2D> _neighborDic;
		HashSet<int> _touchStateSet;

		/*
	 * Unity events
	 */

		void Awake() {
			_trigger = GetComponent<CircleCollider2D>();
			_trigger.isTrigger = true;
			_sim = GetComponent<IFormLayer>().sim;

			_neighborDic = new Dictionary<CircleCollider2D, Rigidbody2D>();
			_touchStateSet = new HashSet<int>();
		}

		void FixedUpdate() {
			BroadPhaseUpdate();
			NarrowPhaseUpdate();
		}

		void OnTriggerEnter2D(Collider2D col) {
			var target = col.GetComponent<CircleCollider2D>();
			var targetRb2D = col.GetComponent<Rigidbody2D>();
			if(!target || !targetRb2D || _neighborDic.ContainsKey(target)) {
				return;
			}
			_neighborDic.Add(target, targetRb2D);
		}

		void OnTriggerExit2D(Collider2D col) {
			var target = col.GetComponent<CircleCollider2D>();
			if(!target || !_neighborDic.ContainsKey(target)) {
				return;
			}
			_neighborDic.Remove(target);
		}

		/*
		 * Methods
		 */

		void BroadPhaseUpdate() {
			var bc = _sim.GetBoundingCircle(particleRadius);
			_trigger.radius = bc.radius;
			_trigger.offset = bc.center;
		}

		void NarrowPhaseUpdate() {
			if(_neighborDic.Count == 0) {
				return;
			}
			foreach(var col in _neighborDic.Keys) {
				SimulateCollision(_sim, col, _neighborDic[col]);
			}
		}

		void SimulateCollision(Simulator sim, CircleCollider2D col, Rigidbody2D rbody2d) {
			var trans = col.transform;
			var mat = transform.worldToLocalMatrix;
			Vector4 pos = trans.position;
			pos.w = 1f;
			pos.x += col.offset.x; pos.y += col.offset.y;
			var bc = new BoundingCircle((Vector2)(mat * pos), col.radius * Mathf.Max(trans.localScale.x, trans.localScale.y));
			Vector2 dir;
			for(var i = 0; i < sim.numOfParticles; ++i) {
				var p = sim.GetParticleAt(i);
				if(bc.OverlapsResult(p.pos, particleRadius, out dir)) {
					p.pos += dir * particleForceFeedback;
					rbody2d.AddForce(-dir * targetForceFeedback, ForceMode2D.Force);
					if(!_touchStateSet.Contains(p.uid)) {
						_touchStateSet.Add(p.uid);
						onCollisionEnter.Invoke(p.uid);
					}
				} else {
					if(_touchStateSet.Contains(p.uid)) {
						_touchStateSet.Remove(p.uid);
						onCollisionExit.Invoke(p.uid);
					}
				}
			}
		}
	}
}