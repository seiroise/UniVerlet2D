using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	[System.Serializable]
	public class RenderingGroupSettings {
		public int groupID;                         // 描画グループID
		public Vector3[] positions;                 // 使用する座標(4つまで)
		public Vector2[] uvs;                       // 使用するUV座標(4つまで)
		public Color color = Color.white;           // 頂点カラー

		public static RenderingGroupSettings MakeSquare() {
			var settings = new RenderingGroupSettings();
			settings.positions = new Vector3[]{
				new Vector3(-0.5f, -0.5f, 0f),
				new Vector3(-0.5f, 0.5f, 0f),
				new Vector3(0.5f, -0.5f, 0f),
				new Vector3(0.5f, 0.5f, 0f)
			};
			settings.uvs = new Vector2[] {
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f)
			};

			return settings;
		}
	}

	[CreateAssetMenu(menuName = "PP2D/SimRenderingSettings", fileName = "SimRenderingSettings")]
	public class SimRenderingSettings : ScriptableObject {

		[SerializeField]
		public List<RenderingGroupSettings> _renderingGroupSettings;

		public RenderingGroupSettings GetGroupSettingsAt(int idx) {
			return _renderingGroupSettings[idx];
		}
	}
}