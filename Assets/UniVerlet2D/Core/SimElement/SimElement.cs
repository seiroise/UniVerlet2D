using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class SimElement {

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

		public SimElement() { }

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
			BeforeSerializeToJson();
			return JsonUtility.ToJson(this);
		}

		/*
		 * Abstract methods
		 */

		protected virtual void BeforeSerializeToJson() { }

		public virtual void AfterDeserializeFromJson(Simulator sim) { }

		public virtual void Step(float dt) { }
	}
}