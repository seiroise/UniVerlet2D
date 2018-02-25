using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public abstract class SimInteraction : SimElement {

		bool _on;

		public bool on { get { return _on; } }

		System.Action<float> _stepMethod;

		public SimInteraction() {
			TurnOff();
		}

		public void TurnOn() {
			_on = true;
			_stepMethod = ActiveStep;
		}

		public void TurnOff() {
			_on = false;
			_stepMethod = DisactiveStep;
		}

		public override void Step(float dt) {
			_stepMethod(dt);
		}

		public abstract bool ContainParticle(Particle p);

		protected abstract void ActiveStep(float dt);

		protected void DisactiveStep(float dt) {
			return;
		}
	}
}