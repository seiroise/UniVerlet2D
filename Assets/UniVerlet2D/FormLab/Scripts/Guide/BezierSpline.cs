using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class BezierSpline : MonoBehaviour {

		[Range(4, 32)]
		public int resolution = 10;
		public Color color = Color.white;
		public float width = 0.1f;

		MeshFilter _filter;
		MeshBuilder _builder;
		Quaternion _rotation;

		void Awake() {
			_filter = GetComponent<MeshFilter>();
			_builder = new MeshBuilder("Bezier", true, useColor:true, dynamic: true);
			_rotation = Quaternion.AngleAxis(90f, Vector3.forward);
		}

		/*
		 * Mesthods
		 */

		public void Set(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
			SetLocal(transform.InverseTransformPoint(p0),
			    transform.InverseTransformPoint(p1),
			    transform.InverseTransformPoint(p2),
			    transform.InverseTransformPoint(p3));
		}

		void SetLocal(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
			_builder.Clear();

			var step = 1f / resolution;

			var t = 0f;
			var pos = GetPoint(p0, p1, p2, p3, t);
			var normal = (_rotation * GetFirstDerivative(p0, p1, p2, p3, t)).normalized;
			_builder.AddVertex(pos + normal * width);
			_builder.AddVertex(pos - normal * width);

			_builder.AddUV(new Vector2(0f, 0f));
			_builder.AddUV(new Vector2(1f, 0f));

			_builder.AddColor(color);
			_builder.AddColor(color);

			for(var i = 1; i <= resolution; ++i) {
				t = step * i;
				pos = GetPoint(p0, p1, p2, p3, t);
				normal = (_rotation * GetFirstDerivative(p0, p1, p2, p3, t)).normalized;
				_builder.AddVertex(pos + normal * width);
				_builder.AddVertex(pos - normal * width);

				_builder.AddUV(new Vector2(0f, 0f));
				_builder.AddUV(new Vector2(1f, 0f));

				_builder.AddColor(color);
				_builder.AddColor(color);

				var idx = i * 2;
				_builder.AddQuadIndex(idx - 2, idx - 1, idx, idx + 1);
			}

			_builder.Apply();
			_filter.mesh = _builder.mesh;
		}

		public void Clear() {
			_builder.Clear();
			_builder.Apply();
			_filter.mesh = _builder.mesh;
		}

		/*
		 * Static functions
		 */

		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
			t = Mathf.Clamp01(t);
			float oneMinusT = 1f - t;
			return oneMinusT * oneMinusT * p0 +
				2f * oneMinusT * t * p1 +
				t * t * p2;
		}

		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			t = Mathf.Clamp01(t);
			float oneMinusT = 1f - t;
			return
				oneMinusT * oneMinusT * oneMinusT * p0 +
				3f * oneMinusT * oneMinusT * t * p1 +
				3f * oneMinusT * t * t * p2 +
				t * t * t * p3;
		}

		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
			t = Mathf.Clamp01(t);
			return 2f * (1f - t) * (p1 - p0) +
				2f * t * (p2 - p1);
		}

		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			t = Mathf.Clamp01(t);
			float oneMinusT = 1f - t;
			return
				3f * oneMinusT * oneMinusT * (p1 - p0) +
				6f * oneMinusT * t * (p2 - p1) +
				3f * t * t * (p3 - p2);
		}
	}
}