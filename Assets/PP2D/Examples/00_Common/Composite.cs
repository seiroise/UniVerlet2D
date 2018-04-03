using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	public class Composite {

		public class OrderedSimElement {
			public int order;
			public SimElement simElement;

			public OrderedSimElement(int order, SimElement simElement) {
				this.order = order;
				this.simElement = simElement;
			}
		}

		List<OrderedSimElement> orderedSimElements;
		List<SimRenderer.SimRenderingGroup> renderingGroups;

		public int elemNum { get { return orderedSimElements.Count; } }

		public Composite() {
			orderedSimElements = new List<OrderedSimElement>();
			renderingGroups = new List<SimRenderer.SimRenderingGroup>();
		}

		public void AddSimElement(SimElement simElement, int order = 0) {
			orderedSimElements.Add(new OrderedSimElement(order, simElement));
		}

		public void AddRenderingGroup(SimRenderer.SimRenderingGroup renderingGroup) {
			renderingGroups.Add(renderingGroup);
		}

		public void SetSimElementsToSim(Simulator sim) {
			for(var i = 0; i < orderedSimElements.Count; ++i) {
				var ordered = orderedSimElements[i];
				sim.AddSimElement(ordered.simElement, ordered.order);
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