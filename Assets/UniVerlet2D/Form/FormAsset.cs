using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Data {

	public class FormAsset : ScriptableObject {

		public Form form;

		public static FormAsset MakeFromFormattedText(string data) {
			var asset = ScriptableObject.CreateInstance<FormAsset>();
			asset.form = Form.MakeFromFormattedText(data);
			return asset;
		}
	}
}