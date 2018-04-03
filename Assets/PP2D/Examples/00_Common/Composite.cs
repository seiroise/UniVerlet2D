using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	public class Composite {
		public List<SimElement> simElements;
		public List<SimRenderer.SimRenderingGroup> renderingGroups;

		public Composite() {
			simElements = new List<SimElement>();
			renderingGroups = new List<SimRenderer.SimRenderingGroup>();
		}

		public void SetSimElementsToSim(Simulator sim) {
			for(var i = 0; i < simElements.Count; ++i) {
				sim.AddSimElement(simElements[i]);
			}
		}

		public void SetRenderingGroupToSim(SimRenderer renderer) {
			for(var i = 0; i < renderingGroups.Count; ++i) {
				var group = renderingGroups[i];
				renderer.AddRenderingGroup(group.groupID, group.indices);
			}
		}
	}
}