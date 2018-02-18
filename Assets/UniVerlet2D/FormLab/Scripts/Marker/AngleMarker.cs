using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
	public class AngleMarker : SimElemMarker {

		public Color activeColor = Color.white;
		public Color disactiveColor = Color.gray;

		public float minRadius = 0.5f;

		MeshFilter _meshFilter;
		PolygonCollider2D _collider;
		MeshBuilder _meshBuilder;

		float _minAngle, _maxAngle;
		float _radius;

		protected override void Awake() {
			base.Awake();
			_meshFilter = GetComponent<MeshFilter>();
			_collider = GetComponent<PolygonCollider2D>();

			_meshBuilder = new MeshBuilder("Angle mesh", useColor: true);
		}

		public void SetAngle(AngleConstraint angle) {
			var aAngle = angle.aAngle;
			var bAngle = angle.bAngle;
			var aLen = (angle.a.pos - angle.m.pos).magnitude;
			var bLen = (angle.b.pos - angle.m.pos).magnitude;

			_minAngle = Mathf.Min(aAngle, bAngle);
			_maxAngle = Mathf.Max(aAngle, bAngle);
			_radius = Mathf.Max(minRadius, Mathf.Min(aLen, bLen) * 0.2f);

			SetMesh(_minAngle, _maxAngle, _radius, 10, activeColor);
		}

		public void SetMesh(float minAngle, float maxAngle, float radius, int resolution, Color color) {
			var deltaAngle = maxAngle - minAngle;
			if(deltaAngle > Mathf.PI) {
				deltaAngle -= Mathf.PI * 2f;
			} else if(deltaAngle < -Mathf.PI) {
				deltaAngle += Mathf.PI * 2f;
			}

			var angleStep = deltaAngle * (1f / resolution);
			var stepCount = Mathf.FloorToInt(deltaAngle / angleStep);
			var angle = minAngle;

			_meshBuilder.Clear();
			_meshBuilder.AddVertex(Vector3.zero);
			_meshBuilder.AddColor(color);
			_meshBuilder.AddVertex(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
			_meshBuilder.AddColor(color);

			for(var i = 1; i <= stepCount; ++i) {
				angle = minAngle + i * angleStep;
				_meshBuilder.AddVertex(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
				_meshBuilder.AddTriangleIndex(0, i, i + 1);
				_meshBuilder.AddColor(color);
			}
			_meshBuilder.Apply();
			_meshFilter.mesh = _meshBuilder.mesh;

			if(_collider) {
				var vertices = _meshBuilder.mesh.vertices;
				var points = new Vector2[vertices.Length];
				for(var i = 0; i < vertices.Length; ++i) {
					points[i] = vertices[i];
				}
				_collider.SetPath(0, points);
			}
		}

		public override void Activate() {
			SetMesh(_minAngle, _maxAngle, _radius, 10, activeColor);
			_collider.enabled = true;
		}

		public override void Disactivate() {
			SetMesh(_minAngle, _maxAngle, _radius, 10, disactiveColor);
			_collider.enabled = false;
		}
	}
}