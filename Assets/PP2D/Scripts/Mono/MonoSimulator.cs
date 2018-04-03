using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class MonoSimulator : MonoBehaviour, ISimHolder {

		[SerializeField]
		Simulator _sim;

		public Simulator simulator { get { return _sim; } }

		void Awake() {
			_sim = new Simulator();
			_sim.Init();
		}

		void Update() {
			_sim.Update(0.016f);
		}
	}
}