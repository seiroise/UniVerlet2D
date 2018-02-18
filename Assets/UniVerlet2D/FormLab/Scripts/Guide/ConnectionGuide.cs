using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class ConnectionGuide : MonoBehaviour {

		public BezierSpline[] splines;

		List<Vector3> _selectedPositions;

		Quaternion _rotationQuat;

		void Awake() {
			_selectedPositions = new List<Vector3>();
			_rotationQuat = Quaternion.AngleAxis(20f, Vector3.forward);
		}

		public void AddPosition(Vector3 position) {
			_selectedPositions.Add(position);
			if(_selectedPositions.Count > 1) {
				UpdateSpline(_selectedPositions.Count - 2);
			}
		}

		public void SetPositionAt(int idx, Vector3 position) {
			if(idx < 0 || _selectedPositions.Count <= idx) {
				return;
			}
			_selectedPositions[idx] = position;
			UpdateSpline(idx - 1);
		}

		void UpdateSpline(int idx) {
			if((_selectedPositions.Count <= idx + 1) || (splines.Length <= idx)) {
				return;
			}

			var from = _selectedPositions[idx];
			var to = _selectedPositions[idx + 1];

			var dir = to - from;
			var rad = Mathf.RoundToInt(Mathf.Atan2(dir.y, dir.x) / 0.314f);
			dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * 4f;
			var c0 = from + _rotationQuat * dir;
			var c1 = to - _rotationQuat * dir;

			var spline = splines[idx];
			spline.Set(from, c0, c1, to);
		}
	}
}