using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(ISimHolder), typeof(SimRenderer), typeof(SimCollider))]
	public class BTree : MonoBehaviour {

		[Header("Generating settings")]
		[Range(2, 8)]
		public int depth = 4;
		[Range(1, 10)]
		public float branchLength = 2;

		[Header("SimElement settings")]
		[Range(0.001f, 0.999f)]
		public float particleDamping = 0.985f;
		[Range(0.1f, 50f)]
		public float springStiffness = 1f;
		[Range(0.1f, 50f)]
		public float angleStiffness = 1f;
		[Range(0.1f, 2f)]
		public float angleSoftness = 0.9f;

		MonoSimulator monoSim { get; set; }
		SimRenderer simRenderer { get; set; }
		Simulator sim { get; set; }

		Composite composite { get; set; }
		List<int> branchParticleIndices { get; set; }
		List<int> springIndices { get; set; }
		List<int> leafSpringIndices { get; set; }

		void Awake() {
			monoSim = GetComponent<MonoSimulator>();
			simRenderer = GetComponent<SimRenderer>();
		}

		void Start() {
			sim = monoSim.simulator;
			var composite = MakeTree(depth, branchLength);

			composite.SetSimElementsToSim(sim);
			composite.SetRenderingGroupToSim(simRenderer);
		}

		Composite MakeTree(int depth, float branchLength) {
			composite = new Composite();
			branchParticleIndices = new List<int>();
			springIndices = new List<int>();
			leafSpringIndices = new List<int>();

			Particle root = new Particle(Vector2.zero, particleDamping);
			branchParticleIndices.Add(composite.simElements.Count);
			composite.simElements.Add(root);
			PinConstraint rootPin = new PinConstraint(root);

			Particle branch = MakeBranch(root, depth, branchLength, Mathf.PI * 0.5f, angleStiffness);
			PinConstraint branchPin = new PinConstraint(branch);

			composite.simElements.Add(rootPin);
			composite.simElements.Add(branchPin);

			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(2, springIndices));
			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(0, branchParticleIndices));
			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(1, leafSpringIndices));

			return composite;
		}

		Particle MakeBranch(Particle baseParticle, int depth, float branchLength, float angle, float angleStiffness) {
			Particle p = new Particle(baseParticle.pos + AngleToVector2(angle) * branchLength);
			if(depth > 0) {
				branchParticleIndices.Add(composite.simElements.Count);
			}
			composite.simElements.Add(p);

			SpringConstraint s = new SpringConstraint(baseParticle, p, springStiffness);
			if(depth > 0) {
				springIndices.Add(composite.simElements.Count);
			} else {
				leafSpringIndices.Add(composite.simElements.Count);
			}
			composite.simElements.Add(s);

			if(depth > 0) {
				Particle lp = MakeBranch(p, depth - 1, branchLength, angle + Mathf.PI * 0.1f, angleStiffness * angleSoftness);
				AngleConstraint lac = new AngleConstraint(baseParticle, lp, p, angleStiffness);
				composite.simElements.Add(lac);

				Particle rp = MakeBranch(p, depth - 1, branchLength, angle - Mathf.PI * 0.1f, angleStiffness * angleSoftness);
				AngleConstraint rac = new AngleConstraint(baseParticle, rp, p, angleStiffness);
				composite.simElements.Add(rac);

				AngleConstraint cac = new AngleConstraint(lp, rp, p, angleStiffness);
				composite.simElements.Add(cac);
			}
			return p;
		}

		Vector2 AngleToVector2(float angle) {
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}