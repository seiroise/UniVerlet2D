using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class MeshBuilder {

		Mesh _mesh;
		public Mesh mesh {
			get { return _mesh; }
		}

		bool _useUV, _useUV2, _useColor, _calcNormal;

		List<Vector3> _vertices = null;
		List<Vector2> _uvs = null;
		List<Vector2> _uv2s = null;
		List<Color32> _color32s = null;
		List<int> _indices = null;

		public MeshBuilder(
			string name,
			bool useUV = false,
			bool useUV2 = false,
			bool useColor = false,
			bool calcNormal = true,
			bool dynamic = false
		) {
			_mesh = new Mesh();
			_mesh.name = name;
			if (dynamic) {
				_mesh.MarkDynamic();
			}
			_useUV = useUV;
			_useUV2 = useUV2;
			_useColor = useColor;
			_calcNormal = calcNormal;
		}

		public void Clear() {
			_mesh.Clear();
			_vertices = ListPool<Vector3>.Get();
			if (_useColor) {
				_color32s = ListPool<Color32>.Get();
			}
			if (_useUV) {
				_uvs = ListPool<Vector2>.Get();
			}
			if (_useUV2) {
				_uv2s = ListPool<Vector2>.Get();
			}
			_indices = ListPool<int>.Get();
		}

		public void Apply() {
			_mesh.SetVertices(_vertices);
			ListPool<Vector3>.Add(_vertices);
			_vertices = null;

			if (_useColor) {
				_mesh.SetColors(_color32s);
				ListPool<Color32>.Add(_color32s);
				_color32s = null;
			}

			if (_useUV) {
				_mesh.SetUVs(0, _uvs);
				ListPool<Vector2>.Add(_uvs);
				_uvs = null;
			}
			if (_useUV2) {
				_mesh.SetUVs(1, _uv2s);
				ListPool<Vector2>.Add(_uv2s);
				_uv2s = null;
			}

			_mesh.SetTriangles(_indices, 0);
			ListPool<int>.Add(_indices);
			_indices = null;

			if (_calcNormal) {
				_mesh.RecalculateNormals();
			}
		}

		public int NumOfVertex() {
			return _vertices.Count;
		}

		public void AddVertex(Vector3 v) {
			_vertices.Add(v);
		}

		public void AddColor(Color c) {
			_color32s.Add(c);
		}

		public void AddColor(Color32 c) {
			_color32s.Add(c);
		}

		public void AddUV(Vector2 uv) {
			_uvs.Add(uv);
		}

		public void AddUV2(Vector2 uv2) {
			_uv2s.Add(uv2);
		}

		public void AddTriangleIndex(int i1, int i2, int i3) {
			_indices.Add(i1);
			_indices.Add(i2);
			_indices.Add(i3);
		}

		public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
			int vertexIndex = _vertices.Count;

			_vertices.Add(v1);
			_vertices.Add(v2);
			_vertices.Add(v3);
			_indices.Add(vertexIndex);
			_indices.Add(vertexIndex + 1);
			_indices.Add(vertexIndex + 2);
		}

		public void AddTriangleColor(Color color) {
			_color32s.Add(color);
			_color32s.Add(color);
			_color32s.Add(color);
		}

		public void AddTriangleColor(Color32 color) {
			_color32s.Add(color);
			_color32s.Add(color);
			_color32s.Add(color);
		}

		public void AddTriangleColor(Color c1, Color c2, Color c3) {
			_color32s.Add(c1);
			_color32s.Add(c2);
			_color32s.Add(c3);
		}

		public void AddTriangleColor(Color32 c1, Color32 c2, Color32 c3) {
			_color32s.Add(c1);
			_color32s.Add(c2);
			_color32s.Add(c3);
		}

		public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector2 uv3) {
			_uvs.Add(uv1);
			_uvs.Add(uv2);
			_uvs.Add(uv3);
		}

		public void AddTriangleUV2(Vector2 uv1, Vector2 uv2, Vector2 uv3) {
			_uv2s.Add(uv1);
			_uv2s.Add(uv2);
			_uv2s.Add(uv3);
		}


		public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
			int vertexIndex = _vertices.Count;

			_vertices.Add(v1);
			_vertices.Add(v2);
			_vertices.Add(v3);
			_vertices.Add(v4);
			_indices.Add(vertexIndex);
			_indices.Add(vertexIndex + 2);
			_indices.Add(vertexIndex + 1);
			_indices.Add(vertexIndex + 1);
			_indices.Add(vertexIndex + 2);
			_indices.Add(vertexIndex + 3);
		}

		public void AddQuadIndex(int i1, int i2, int i3, int i4) {
			_indices.Add(i1);
			_indices.Add(i3);
			_indices.Add(i2);
			_indices.Add(i2);
			_indices.Add(i3);
			_indices.Add(i4);
		}

		public void AddQuadColor(Color c1, Color c2, Color c3, Color c4) {
			_color32s.Add(c1);
			_color32s.Add(c2);
			_color32s.Add(c3);
			_color32s.Add(c4);
		}

		public void AddQuadColor(Color32 c1, Color32 c2, Color32 c3, Color32 c4) {
			_color32s.Add(c1);
			_color32s.Add(c2);
			_color32s.Add(c3);
			_color32s.Add(c4);
		}

		public void AddQuadColor(Color c1, Color c2) {
			_color32s.Add(c1);
			_color32s.Add(c1);
			_color32s.Add(c2);
			_color32s.Add(c2);
		}

		public void AddQuadColor(Color32 c12, Color32 c34) {
			_color32s.Add(c12);
			_color32s.Add(c12);
			_color32s.Add(c34);
			_color32s.Add(c34);
		}

		public void AddQuadColor(Color c) {
			_color32s.Add(c);
			_color32s.Add(c);
			_color32s.Add(c);
			_color32s.Add(c);
		}

		public void AddQuadColor(Color32 c) {
			_color32s.Add(c);
			_color32s.Add(c);
			_color32s.Add(c);
			_color32s.Add(c);
		}

		public void AddQuadUV(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
			_uvs.Add(uv1);
			_uvs.Add(uv2);
			_uvs.Add(uv3);
			_uvs.Add(uv4);
		}

		public void AddQuadUV(float uMin, float uMax, float vMin, float vMax) {
			_uvs.Add(new Vector2(uMin, vMin));
			_uvs.Add(new Vector2(uMax, vMin));
			_uvs.Add(new Vector2(uMin, vMax));
			_uvs.Add(new Vector2(uMax, vMax));
		}

		public void AddQuadUV2(Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
			_uv2s.Add(uv1);
			_uv2s.Add(uv2);
			_uv2s.Add(uv3);
			_uv2s.Add(uv4);
		}
	}
}