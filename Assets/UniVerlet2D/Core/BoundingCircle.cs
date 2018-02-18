using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class BoundingCircle {

		/*
		 * Fields
		 */

		Vector2 _center;
		float _radius;

		/*
		 * Properties
		 */

		public static BoundingCircle zero { get { return new BoundingCircle(Vector2.zero, 0f); } }

		public Vector2 center { get { return _center; } set { _center = value; } }
		public float radius { get { return _radius; } set { _radius = value; } }

		/*
		 * Constructor
		 */

		public BoundingCircle(Vector2 center, float radius) {
			_center = center;
			_radius = radius;
		}

		/*
		 * Functions
		 */

		public bool OverlapsResult(Vector2 center, float radius, out Vector2 dir) {
			dir = center - _center;
			float m2 = dir.sqrMagnitude;
			float rr = radius + _radius;
			if(m2 < rr * rr) {
				m2 = Mathf.Sqrt(m2);
				float o = radius + _radius - m2;
				dir *= o / m2;
				return true;
			} else {
				return false;
			}
		}
	}
}