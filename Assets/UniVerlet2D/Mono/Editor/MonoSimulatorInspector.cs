using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UniVerlet2D {

	[CustomEditor(typeof(MonoSimulator))]
	public class MonoSimulatorInspector : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			var t = target as MonoSimulator;

			GUILayout.Label("---- Current ----");
			GUILayout.Label("Particles: " + t.sim.numOfParticles);
			GUILayout.Label("Springs: " + t.sim.numOfSprings);
			GUILayout.Label("Angles: " + t.sim.numOfAngles);
			GUILayout.Label("Pins: " + t.sim.numOfPins);

			var serialized = t.sim.serializedForm;
			GUILayout.Label("---- Serialized ----");

			if(serialized != null) {
				GUILayout.Label("Particles: " + t.sim.numOfParticles);
				GUILayout.Label("Springs: " + t.sim.numOfSprings);
				GUILayout.Label("Angles: " + t.sim.numOfAngles);
				GUILayout.Label("Pins: " + t.sim.numOfPins);
			} else {
				GUILayout.Label("null");
			}
		}
	}
}