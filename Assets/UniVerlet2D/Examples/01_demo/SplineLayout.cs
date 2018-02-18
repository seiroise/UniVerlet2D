using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Examples {
	public class SplineLayout : MonoBehaviour {

		/*
		 * Fields
		 */

		public Transform[] ctrPoints;

		/*
		 * Unity events
		 */

		void Start() {
			Align();
		}

		void OnDrawGizmos() {
			DrawSpline();
		}

		/*
		 * Methods
		 */

		public void Align() {
			if(transform.childCount == 0) {
				return;
			}

			float step = 1f / (transform.childCount + 1);
			var i = 0;
			foreach(Transform c in transform) {
				c.position = GetPosition(i * step + step * 0.5f);
				++i;
			}
		}

		/*
		 * Spline related
		 */

		Vector3 GetPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
			Vector3 a = 2f * p1;
			Vector3 b = p2 - p0;
			Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
			Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

			return 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));
		}

		Vector3 GetPosition(float t, int pos) {
			Vector3 p0 = ctrPoints[ClampListPos(pos - 1)].position;
			Vector3 p1 = ctrPoints[ClampListPos(pos)].position;
			Vector3 p2 = ctrPoints[ClampListPos(pos + 1)].position;
			Vector3 p3 = ctrPoints[ClampListPos(pos + 2)].position;

			return GetPosition(t, p0, p1, p2, p3);
		}

		public Vector3 GetPosition(float t) {
			var range = 1f / (ctrPoints.Length - 1);
			int pos = Mathf.FloorToInt(t / range);

			return GetPosition((t % range) / range, pos);
		}

		int ClampListPos(int pos) {
			return Mathf.Clamp(pos, 0, ctrPoints.Length - 1);
		}

		void DrawSpline(int pos) {
			Vector3 p0 = ctrPoints[ClampListPos(pos - 1)].position;
			Vector3 p1 = ctrPoints[ClampListPos(pos)].position;
			Vector3 p2 = ctrPoints[ClampListPos(pos + 1)].position;
			Vector3 p3 = ctrPoints[ClampListPos(pos + 2)].position;

			Vector3 lastPos = p1;

			float resolution = 0.1f;
			int numStep = Mathf.FloorToInt(1f / resolution);

			for(int i = 1; i <= numStep; ++i) {
				float t = i * resolution;
				Vector3 newPos = GetPosition(t, p0, p1, p2, p3);

				Gizmos.DrawLine(lastPos, newPos);

				lastPos = newPos;
			}
		}

		public void DrawSpline() {
			if(ctrPoints.Length < 2) {
				return;
			}
			Gizmos.color = Color.white;
			for(int i = 0; i < ctrPoints.Length - 1; ++i) {
				DrawSpline(i);
			}
		}
	}
}