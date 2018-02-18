using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UniVerlet2D.Examples {

	[CustomEditor(typeof(MonoSimTree))]
	public class MonoSimTreeInspector : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if(GUILayout.Button("Generate")) {
				var t = target as MonoSimTree;
				t.BuildForm();
			}
		}
	}
}