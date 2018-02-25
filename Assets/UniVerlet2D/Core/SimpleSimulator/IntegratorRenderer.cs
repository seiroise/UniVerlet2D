using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class IntegratorRenderer : MonoBehaviour {

		public List<int> renderedSimElemIdx;

		public Vector4[] particlePositions = {
			new Vector4(-0.5f, 0f, 0f, 1f),
			new Vector4(0f, 0.5f, 0f, 1f),
			new Vector4(0f, -0.5f, 0f, 1f),
			new Vector4(0.5f, 0f, 0f, 1f)
		};

		public Vector2[] particleUVs = {
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};

		MeshBuilder _meshBuilder;
		MeshFilter _filter;

		void Awake() {
			_meshBuilder = new MeshBuilder("SimMesh", true, useColor:true, dynamic:true);
			_filter = GetComponent<MeshFilter>();
		}

		public void SetMesh(SimpleSim integrator) {
			_meshBuilder.Clear();

			for(var i = 0; i < renderedSimElemIdx.Count; ++i) {
				var elem = integrator.GetSimElementAt(renderedSimElemIdx[i]);
				var mat = elem.GetMatrix();

				_meshBuilder.AddQuad(
					mat * particlePositions[0],
					mat * particlePositions[1],
					mat * particlePositions[2],
					mat * particlePositions[3]
				);
				_meshBuilder.AddQuadUV(
					particleUVs[0],
					particleUVs[1],
					particleUVs[2],
					particleUVs[3]
				);

				_meshBuilder.AddQuadColor(Color.white);
			}

			_meshBuilder.Apply();

			_filter.mesh = _meshBuilder.mesh;
		}
	}
}