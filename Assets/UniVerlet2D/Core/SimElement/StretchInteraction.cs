using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class StretchInteraction : Interaction {

		Particle _a, _b;
		float _power;

		public Particle a { get { return _a; } }
		public Particle b { get { return _b; } }
		public float power { get { return _power; } set { _power = value; } }

		public float currentLength { get { return (_a.pos - _b.pos).magnitude; } }
		public Vector2 middlePos { get { return (_a.pos + _b.pos) * 0.5f; } }
		public float a2bRadian { get { var rad = Mathf.Atan2(_b.pos.y - _a.pos.y, _b.pos.x - _a.pos.x); return float.IsNaN(rad) ? 0f : rad; } }

		public StretchInteraction(Simulator sim, Particle a, Particle b) : base(sim) {
			_a = a;
			_b = b;
		}

		public override void Apply(float dt) {
			if(on) {
				var dir = (_b.pos - _a.pos) * dt;
				_a.pos += dir;
				_b.pos -= dir;
			}
		}

		public override bool ContainParticle(Particle p) {
			return _a.uid == p.uid || _b.uid == p.uid;
		}
	}
}