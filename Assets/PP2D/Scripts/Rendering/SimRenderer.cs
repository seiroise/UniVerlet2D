using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D;

namespace PP2D {

	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(ISimHolder))]
	public class SimRenderer : MonoBehaviour {

		[System.Serializable]
		public class SimRenderingGroup {
			public int groupID;
			public List<int> indices;

			public SimRenderingGroup(int groupID, List<int> indices) {
				this.groupID = groupID;
				this.indices = indices;
			}
		}

		[SerializeField]
		SimRenderingSettings _renderingSettings;
		[SerializeField]
		public List<SimRenderingGroup> _renderingGroups;

		MeshRenderer _renderer;
		MeshFilter _filter;

		ISimHolder _simHolder;
		Simulator _sim;

		MeshBuilder _builder;

		/*
		 * Unity events
		 */

		void Awake() {
			if(!_renderingSettings) {
				throw new System.NullReferenceException("SimRenderingSettingsを設定してください。");
			}
			_renderer = GetComponent<MeshRenderer>();
			_filter = GetComponent<MeshFilter>();

			_simHolder = GetComponent<ISimHolder>();

			_builder = new MeshBuilder("Sim mesh", useUV: true, useColor: true, dynamic: true);
		}

		void Start() {
			_sim = _simHolder.simulator;
		}

		void OnWillRenderObject() {
			TriangulateSimElements();
		}

		/*
		 * Methods
		 */

		void TriangulateSimElements() {
			_builder.Clear();
			for(var i = 0; i < _renderingGroups.Count; ++i) {
				var group = _renderingGroups[i];
				var groupSettings = _renderingSettings.GetGroupSettingsAt(group.groupID);
				for(var j = 0; j < group.indices.Count; ++j) {
					var elem = _sim.GetSimElementAt(group.indices[j]);
					var mat = elem.GetMatrix();

					_builder.AddQuad(
						mat.MultiplyPoint3x4(groupSettings.positions[0]),
						mat.MultiplyPoint3x4(groupSettings.positions[1]),
						mat.MultiplyPoint3x4(groupSettings.positions[2]),
						mat.MultiplyPoint3x4(groupSettings.positions[3])
					);

					_builder.AddQuadUV(
						groupSettings.uvs[0],
						groupSettings.uvs[1],
						groupSettings.uvs[2],
						groupSettings.uvs[3]
					);

					_builder.AddQuadColor(groupSettings.color);
				}
			}

			_builder.Apply();
			_filter.mesh = _builder.mesh;
		}

		public void AddRenderingGroup(int groupID, List<int> indices) {
			_renderingGroups.Add(new SimRenderingGroup(groupID, indices));
		}
	}
}