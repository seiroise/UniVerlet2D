using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(ISimHolder), typeof(SimRenderer), typeof(SimCollider))]
	public abstract class MonoComposite : MonoBehaviour {

		[Header("SimElement settings")]
		[Range(0.001f, 0.999f)]
		public float particleDamping = 0.985f;
		[Range(0.1f, 50f)]
		public float springStiffness = 1f;
		[Range(0.1f, 50f)]
		public float angleStiffness = 1f;

		public ISimHolder simHolder { get; set; }
		public SimRenderer simRenderer { get; set; }
		public Simulator simulator { get; set; }

		void Awake() {
			simHolder = GetComponent<ISimHolder>();
			simRenderer = GetComponent<SimRenderer>();
		}

		void Start() {
			simulator = simHolder.simulator;
			Composite composite = MakeComposite();
			composite.SetSimElementsToSim(simulator);
			composite.SetRenderingGroupToSim(simRenderer);
		}

		public abstract Composite MakeComposite();
	}
}