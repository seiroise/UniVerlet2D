using UnityEngine;
using System;
using System.Collections.Generic;

namespace UniVerlet2D {

	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class MinMaxRangeAttribute : PropertyAttribute {

		public readonly float minLimit;
		public readonly float maxLimit;

		public MinMaxRangeAttribute(float minLimit = 0f, float maxLimit = 1f) {
			this.minLimit = minLimit;
			this.maxLimit = maxLimit;
		}
	}
}