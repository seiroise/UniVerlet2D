using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class PinConstraint : SimElement, IConstraint {

		Particle _a;
		Vector2 _pos;

		public Particle a { get { return _a; } }
		public Vector2 pos { get { return _pos; } }

		public PinConstraint(Simulator sim, Particle a, Vector2 pos) : base(sim) {
			_a = a;
			_pos = pos;
		}

		public void Relax(float dt) {
			_a.pos = _pos;
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid;
		}
	}
}