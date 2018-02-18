using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D {

	public class MonoSimulator : MonoBehaviour, IFormLayer {

		/*
		 * Fields
		 */

		[Header("Simulator")]
		[SerializeField]
		bool _startWithClear = true;
		[SerializeField]
		Simulator _sim;
		[SerializeField]
		bool _updateSim = true;

		/*
		 * Properties
		 */

		public Simulator sim { get { return _sim; } }
		public bool startWithClear { set { _startWithClear = value; } }
		public bool updateSim { get { return _updateSim; } set { _updateSim = value; } }

		/*
		 * Unity events
		 */

		void Start() {
			sim.Init(_startWithClear);
		}

		void FixedUpdate() {
			if(_updateSim) {
				sim.Update(0.016f);
			}
		}
	}
}