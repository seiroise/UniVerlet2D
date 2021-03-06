﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public abstract class SimElement {

		/*
		 * Fields
		 */

		[SerializeField, HideInInspector()]
		protected int _uid;

		/*
		 * Properties
		 */

		public int uid { get { return _uid; } }

		/*
		 * Functions
		 */

		public void OverrideUID(int uid) {
			_uid = uid;
		}

		/*
		 * Abstract methods
		 */

		public abstract void Step(float dt);

		public abstract Matrix4x4 GetMatrix();
	}
}