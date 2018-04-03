using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(ISimHolder), typeof(SimRenderer), typeof(SimCollider))]
	public class Spinal2 : MonoBehaviour {

		[Range(2f, 50f)]
		public float length = 5f;
		[Range(2, 50)]
		public int rimbNum = 5;
		public Vector2 gravity = new Vector2(0f, -0.1f);
		[Range(-3.14f, 3.14f)]
		public float angle = Mathf.PI * 0.5f;
		[Range(0f, 1f)]
		public float damping = 0.98f;

		[Header("Wave")]
		public float amplitude = 1f;
		public float timeScale = 1f;

		ISimHolder simHolder { get; set; }
		Simulator simulator { get; set; }

		SimRenderer simRenderer { get; set; }
		SimCollider simCollider { get; set; }

		void Awake() {
			simHolder = GetComponent<ISimHolder>();
			simRenderer = GetComponent<SimRenderer>();
			simCollider = GetComponent<SimCollider>();
		}

		void Start() {
			simulator = simHolder.simulator;
			var spinal = MakeSpinalComposite(length, rimbNum, Vector2.zero, angle);

			spinal.SetSimElementsToSim(simulator);
			spinal.SetRenderingGroupToSim(simRenderer);
		}

		Composite MakeSpinalComposite(float length, int rimbNum, Vector2 rootPosition, float angle) {
			if(rimbNum <= 1) {
				throw new System.ArgumentException("rimbNumは\"2\"以上に設定してください。");
			}
			float boneLength = length / (rimbNum + 1);
			var composite = new Composite();

			List<int> particleIndices = new List<int>();
			List<int> gravityIndices = new List<int>();
			List<int> springIndices = new List<int>();

			Vector2 direction = AngleToVec2(angle);

			var tp = new Particle(rootPosition);
			particleIndices.Add(composite.elemNum);
			composite.AddSimElement(tp, 0);

			var g = new ConstantForceConstraint(tp, gravity);
			gravityIndices.Add(composite.elemNum);
			composite.AddSimElement(g, 5);

			//var sinWave = new SinWaveForce(tp, Vector2.right, amplitude, timeScale, 0f);
			//composite.simElements.Add(sinWave);

			for(int i = 1; i < rimbNum; ++i) {
				var p = new Particle(rootPosition + direction * boneLength * i, damping);
				particleIndices.Add(composite.elemNum);
				composite.AddSimElement(p, 0);

				// var s = new SpringConstraint(tp, p);
				var s = new DistanceConstraint(tp, p);
				springIndices.Add(composite.elemNum);
				composite.AddSimElement(s, 1);

				g = new ConstantForceConstraint(p, gravity);
				gravityIndices.Add(composite.elemNum);
				composite.AddSimElement(g, 5);

				tp = p;
			}

			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(1, springIndices));
			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(2, gravityIndices));
			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(0, particleIndices));

			return composite;
		}

		Vector2 AngleToVec2(float angle) {
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}