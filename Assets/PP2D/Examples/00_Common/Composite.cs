using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	public class Composite : MonoBehaviour {
		public List<SimElement> simElements;
		public List<SimRenderer.SimRenderingGroup> renderingGroups;

		public Composite() {
			simElements = new List<SimElement>();
			renderingGroups = new List<SimRenderer.SimRenderingGroup>();
		}
	}
}