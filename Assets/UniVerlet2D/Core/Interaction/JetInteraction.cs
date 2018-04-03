using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class JetInteraction : Interaction {

		Particle _a, _b;

		[SerializeField]
		float _power;

		/*
		 * Properties
		 */

		public Particle a { get { return _a; } }
		public Particle b { get { return _b; } }

		public float power { get { return _power; } set { _power = value; } }

		/*
		 * Constructor
		 */

		public JetInteraction(Particle a, Particle b, float power = 1f) {
			_a = a;
			_b = b;
			_power = power;
		}

		/*
		 * Methods
		 */

		public override bool ContainParticle(Particle p) {
			return _a.uid == p.uid || _b.uid == p.uid;
		}

		public override Matrix4x4 GetMatrix() {
			return _a.worldMatrix;
		}

		protected override void ActiveStep(float dt) {
			Vector2 dir = _a.pos - _b.pos;
			_a.pos += dir * dt * _power;
		}
	}
}