using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D;

namespace PP2D {

	[RequireComponent(typeof(ISimHolder))]
	public class ParticleDragArm : MonoBehaviour {

		[SerializeField, Range(0.01f, 10f)]
		float _particleRadius = 0.5f;

		ISimHolder _simHolder;
		Simulator _sim;

		bool _isDragging = false;
		Particle _draggedParticle;

		List<Particle> _particles;

		void Awake() {
			_simHolder = GetComponent<ISimHolder>();
		}

		void Start() {
			_sim = _simHolder.simulator;
			_sim.onChangeComposition.AddListener(OnChangeSimComposition);
			_particles = _sim.FindSimElems<Particle>();
		}

		void LateUpdate() {
			Vector3 pos = Input.mousePosition;
			pos.z = -Camera.main.transform.position.z;
			pos = Camera.main.ScreenToWorldPoint(pos);

			if(!_isDragging) {
				if(Input.GetMouseButtonDown(0)) {
					int idx;
					if(SimElementHelper.FindOverlapParticleIdx(_particles, pos, _particleRadius, out idx)) {
						_isDragging = true;
						_draggedParticle = _particles[idx];
					}
				}
			} else {
				if(Input.GetMouseButtonUp(0)) {
					_isDragging = false;
				} else if(Input.GetMouseButton(0)) {
					_draggedParticle.pos = pos;
				}
			}
		}

		void OnChangeSimComposition() {
			_particles = _sim.FindSimElems<Particle>();
			Debug.Log("パーティクルの数は" + _particles.Count);
		}
	}
}