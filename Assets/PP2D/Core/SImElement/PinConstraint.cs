using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class PinConstraint : SimElement {

		Particle _a;

		[SerializeField]
		Vector2 _pos;

		public Particle a { get { return _a; } }
		public Vector2 pos { get { return _pos; } }

		public PinConstraint(Particle a, Vector2 pos) {
			_a = a;
			_pos = pos;
		}

		public void Relax(float dt) {
			_a.pos = _pos;
		}

		public override void Step(float dt) {
			_a.pos = _pos;
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid;
		}

		public override Matrix4x4 GetMatrix() {
			return _a.GetMatrix();
		}
	}
}