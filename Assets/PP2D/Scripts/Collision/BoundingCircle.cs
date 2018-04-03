using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class BoundingCircle {

		/*
		 * Fields
		 */

		public Vector2 center { get; set; }
		public float radius { get; set; }

		/*
		 * Properties
		 */

		public static BoundingCircle zero { get { return new BoundingCircle(Vector2.zero, 0f); } }

		/*
		 * Constructor
		 */

		public BoundingCircle(Vector2 center, float radius) {
			this.center = center;
			this.radius = radius;
		}

		/*
		 * Functions
		 */

		public bool OverlapsResult(Vector2 center, float radius, out Vector2 dir) {
			dir = center - this.center;
			float m2 = dir.sqrMagnitude;
			float rr = radius + this.radius;
			if(m2 < rr * rr) {
				m2 = Mathf.Sqrt(m2);
				float o = radius + this.radius - m2;
				dir *= o / m2;
				return true;
			} else {
				return false;
			}
		}
	}
}