using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(ISimHolder), typeof(SimRenderer))]
	public class Spinal : MonoBehaviour {

		public class Composite {
			public List<SimElement> simElements;
			public List<SimRenderer.SimRenderingGroup> renderingGroups;

			public Composite() {
				simElements = new List<SimElement>();
				renderingGroups = new List<SimRenderer.SimRenderingGroup>();
			}
		}

		[Range(2f, 10f)]
		public float length = 5;
		[Range(2, 10)]
		public int boneNum = 5;

		MonoSimulator _monoSim;
		Simulator _sim;

		SimRenderer _simRenderer;

		void Awake() {
			_monoSim = GetComponent<MonoSimulator>();
			_simRenderer = GetComponent<SimRenderer>();
		}

		void Start() {
			_sim = _monoSim.simulator;
			var composite = MakeSpinalComposite(ref _sim, length, boneNum, Vector2.zero);

			for(var i = 0; i < composite.simElements.Count; ++i) {
				_sim.AddSimElement(composite.simElements[i]);
			}

			for(var i = 0; i < composite.renderingGroups.Count; ++i) {
				var group = composite.renderingGroups[i];
				_simRenderer.AddRenderingGroup(group.groupID, group.indices);
			}
		}

		Composite MakeSpinalComposite(ref Simulator sim, float length, int boneNum, Vector2 rootPosition) {
			if(boneNum <= 1) {
				throw new System.ArgumentException("boneNUmは\"2\"以上に設定してください。");
			}
			float boneLength = length / (boneNum + 1);

			var composite = new Composite();
			List<int> particleIndices = new List<int>();
			List<int> springIndices = new List<int>();

			var tp = new Particle(rootPosition);
			sim.AddSimElement(tp);
			particleIndices.Add(composite.simElements.Count);
			composite.simElements.Add(tp);

			for(int i = 1; i < boneNum; ++i) {
				var p = new Particle(rootPosition + new Vector2(0f, -boneLength * i));
				Debug.Log(p.pos);
				sim.AddSimElement(p);
				particleIndices.Add(composite.simElements.Count);
				composite.simElements.Add(p);

				var s = new SpringConstraint(tp, p);
				sim.AddSimElement(s);
				springIndices.Add(composite.simElements.Count);
				composite.simElements.Add(s);

				tp = p;
			}

			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(1, springIndices));
			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(0, particleIndices));

			return composite;
		}
	}
}