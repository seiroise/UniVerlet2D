using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class ConstantForceConstraint : SimElement {

		Particle _a;

		[SerializeField]
		Vector2 _force;
		[SerializeField]
		float _forceScale;

		public Particle a { get { return _a; } }
		public Vector2 force { get { return _force; } }

		public ConstantForceConstraint(Particle a, Vector2 force, float forceScale = 1f) {
			_a = a;
			_force = force;
			_forceScale = forceScale;
		}

		public override void Step(float dt) {
			_a.pos += _force * (_forceScale * dt);
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid;
		}

		public override Matrix4x4 GetMatrix() {
			return _a.GetMatrix();
		}
	}
}