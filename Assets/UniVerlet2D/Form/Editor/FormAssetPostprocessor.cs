using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniVerlet2D.Data {

	public class FormAssetPostprocessor : AssetPostprocessor {

		public override uint GetVersion() {
			return Form.VERSION;
		}

		static void OnPostprocessAllAssets(
			string[] importedAssets, string[] deletedAssets,
			string[] movedAssets, string[] movedFromPaths
		) {
			for(var i = 0; i < importedAssets.Length; ++i) {
				var ext = Path.GetExtension(importedAssets[i]);
				if(ext.Equals(Form.EXTENSION)) {
					var data = File.ReadAllText(importedAssets[i]);
					var asset = FormAsset.MakeFromFormattedText(data);
					AssetDatabase.CreateAsset(asset, importedAssets[i] + ".asset");
					AssetDatabase.Refresh();
				}
			}
		}
	}
}