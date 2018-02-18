using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public abstract class Interaction : SimElement {

		bool _on;

		public bool on { get { return _on; } }

		public Interaction(Simulator sim) : base(sim) {
			_on = true;
		}

		public void TurnOn() {
			_on = true;
		}

		public void TurnOff() {
			_on = false;
		}

		public abstract void Apply(float dt);

		public abstract bool ContainParticle(Particle p);
	}
}