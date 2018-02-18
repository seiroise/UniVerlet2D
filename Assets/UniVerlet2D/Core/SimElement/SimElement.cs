using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public abstract class SimElement {

		/*
		 * Fields
		 */

		[SerializeField, HideInInspector()]
		protected int _uid;
		protected Simulator _sim;

		/*
		 * Properties
		 */

		public int uid { get { return _uid; } }

		/*
		 * Constructor
		 */

		public SimElement(Simulator sim) {
			_sim = sim;
			_uid = sim.uidDistributer.next;
		}

		/*
		 * Functions
		 */

		public void OverrideUID(int uid) {
			_uid = uid;
		}

		public string ExportJson() {
			return JsonUtility.ToJson(this);
		}

		/*
		 * Abstract methods
		 */

		public abstract void AfterDeserializeFromJson(Simulator sim);

		public abstract void Step(float dt);
	}
}