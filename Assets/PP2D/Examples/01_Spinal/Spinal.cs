using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(ISimHolder), typeof(SimRenderer))]
	public class Spinal : MonoBehaviour {

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
			var composite = MakeSpinalComposite(length, boneNum, Vector2.zero);

			composite.SetSimElementsToSim(_sim);
			composite.SetRenderingGroupToSim(_simRenderer);
		}

		Composite MakeSpinalComposite(float length, int boneNum, Vector2 rootPosition) {
			if(boneNum <= 1) {
				throw new System.ArgumentException("boneNUmは\"2\"以上に設定してください。");
			}
			float boneLength = length / (boneNum + 1);

			var composite = new Composite();
			List<int> particleIndices = new List<int>();
			List<int> springIndices = new List<int>();

			var tp = new Particle(rootPosition);
			particleIndices.Add(composite.elemNum);
			composite.AddSimElement(tp, 0);

			for(int i = 1; i < boneNum; ++i) {
				var p = new Particle(rootPosition + new Vector2(0f, -boneLength * i));
				particleIndices.Add(composite.elemNum);
				composite.AddSimElement(p, 0);

				var s = new SpringConstraint(tp, p);
				springIndices.Add(composite.elemNum);
				composite.AddSimElement(s, 1);

				tp = p;
			}

			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(1, springIndices));
			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(0, particleIndices));

			return composite;
		}
	}
}