using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class TranslationInteraction : Interaction {

		Particle _a;
		Vector2 _velocity;

		public Vector2 velocity { get { return _velocity; } set { _velocity = value; } }

		public TranslationInteraction(Simulator sim, Particle a, Vector2 velocity) : base(sim) {
			_a = a;
			_velocity = velocity;
		}

		public override void Apply(float dt) {
			if(on) {
				_a.pos += _velocity * dt;
			}
		}

		public override bool ContainParticle(Particle p) {
			return _a.uid == p.uid;
		}
	}
}