using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PP2D {

	[RequireComponent(typeof(ISimHolder), typeof(CircleCollider2D))]
	public class SimCollider : MonoBehaviour {

		public class NeighborInfo {
			public CircleCollider2D collider;
			public Rigidbody2D rigidbody2d;

			public NeighborInfo(CircleCollider2D collider, Rigidbody2D rigidbody2d) {
				this.collider = collider;
				this.rigidbody2d = rigidbody2d;
			}
		}

		[System.Serializable]
		public class CollisionEvent : UnityEvent<int> { }

		[Header("Collision parameters")]
		public float particleRadius = 0.1f;
		public float particleForceFeedback = 0.5f;
		public float targetForceFeedback = 0.5f;

		[Header("Event")]
		public CollisionEvent onCollisionEnter;
		public CollisionEvent onCollisionExit;

		CircleCollider2D trigger { get; set; }

		Simulator sim { get; set; }
		Dictionary<Collider2D, NeighborInfo> neighbors { get; set; }
		List<Particle> particles { get; set; }

		void Awake() {
			trigger = GetComponent<CircleCollider2D>();
			trigger.isTrigger = true;
			neighbors = new Dictionary<Collider2D, NeighborInfo>();
		}

		void Start() {
			sim = GetComponent<ISimHolder>().simulator;
			sim.onChangeComposition.AddListener(OnChangeComposition);
			OnChangeComposition();
		}

		void FixedUpdate() {
			BroadPhaseUpdate();
			NarrowPhaseUpdate();
		}

		void OnTriggerEnter2D(Collider2D collision) {
			if(!neighbors.ContainsKey(collision)) {
				var collider = collision.GetComponent<CircleCollider2D>();
				var rigidbody2d = collision.GetComponent<Rigidbody2D>();

				if(collider && rigidbody2d) {
					neighbors.Add(collision, new NeighborInfo(collider, rigidbody2d));
				}
			}
		}

		void OnTriggerExit2D(Collider2D collision) {
			if(neighbors.ContainsKey(collision)) {
				neighbors.Remove(collision);
			}
		}

		void BroadPhaseUpdate() {
			var bc = SimElementHelper.ComputeParticlesBoundingCircle(particles, particleRadius);
			trigger.radius = bc.radius;
			trigger.offset = bc.center;
		}

		void NarrowPhaseUpdate() {
			if(neighbors.Count == 0) {
				return;
			}
			foreach(var col in neighbors.Values) {
				SimulateCollision(sim, col.collider, col.rigidbody2d);
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
			for(var i = 0; i < particles.Count; ++i) {
				var p = particles[i];
				if(bc.OverlapsResult(p.pos, particleRadius, out dir)) {
					p.pos += dir * particleForceFeedback;
					rbody2d.AddForce(-dir * targetForceFeedback, ForceMode2D.Force);
					/*
					if(!_touchStateSet.Contains(p.uid)) {
						_touchStateSet.Add(p.uid);
						onCollisionEnter.Invoke(p.uid);
					}
					*/
				} else {
					/*
					if(_touchStateSet.Contains(p.uid)) {
						_touchStateSet.Remove(p.uid);
						onCollisionExit.Invoke(p.uid);
					}
					*/
				}
			}
		}

		void OnChangeComposition() {
			particles = sim.GetSimElems<Particle>();
		}
	}
}