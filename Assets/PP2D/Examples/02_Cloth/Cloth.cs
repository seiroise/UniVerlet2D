using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(ISimHolder), typeof(SimRenderer))]
	public class Cloth : MonoBehaviour {

		[Header("Generate settings")]
		[Range(2, 32)]
		public int row = 10;
		[Range(2, 32)]
		public int col = 10;

		[Range(2f, 10f)]
		public float rowSize = 5f;
		[Range(2f, 10f)]
		public float colSize = 5f;

		public Vector2 offset;

		[Header("SimElement settings")]
		[Range(0.001f, 0.999f)]
		public float particleDamping = 0.985f;
		[Range(0.1f, 50f)]
		public float springStiffness = 1f;
		public Vector2 gravity = new Vector2(0f, -0.98f);

		MonoSimulator _monoSim;
		Simulator _sim;

		SimRenderer _simRenderer;

		void Awake() {
			_monoSim = GetComponent<MonoSimulator>();
			_simRenderer = GetComponent<SimRenderer>();
		}

		void Start() {
			_sim = _monoSim.simulator;
			var composite = MakeClothComposite(rowSize, row, colSize, col, particleDamping, springStiffness, gravity, Vector2.up * 5f, offset);

			for(var i = 0; i < composite.simElements.Count; ++i) {
				_sim.AddSimElement(composite.simElements[i]);
			}

			for(var i = 0; i < composite.renderingGroups.Count; ++i) {
				var group = composite.renderingGroups[i];
				_simRenderer.AddRenderingGroup(group.groupID, group.indices);
			}
		}

		Composite MakeClothComposite(float rowSize, int row, float colSize, int col, float particleDamping, float springStiffness, Vector2 gravity, Vector2 rootPosition, Vector2 offset) {
			float rowCellSize = rowSize / row;
			float colCellSize = colSize / col;

			Particle[] rowParticles = new Particle[row];

			var composite = new Composite();
			List<int> particleIndices = new List<int>();
			List<int> springIndices = new List<int>();

			for(var y = 0; y < col; ++y) {
				if(y == 0) {
					for(var x = 0; x < row; ++x) {
						var p = new Particle(rootPosition + new Vector2(rowCellSize * x, -colCellSize * y) + offset, particleDamping);
						particleIndices.Add(composite.simElements.Count);
						composite.simElements.Add(p);

						var pc = new PinConstraint(p);
						composite.simElements.Add(pc);

						if(x != 0) {
							var s = new SpringConstraint(rowParticles[x - 1], p, springStiffness);
							// var s = new DistanceConstraint(rowParticles[x - 1], p);
							springIndices.Add(composite.simElements.Count);
							composite.simElements.Add(s);
						}

						rowParticles[x] = p;
					}
				} else {
					for(var x = 0; x < row; ++x) {
						var p = new Particle(rootPosition + new Vector2(rowCellSize * x, -colCellSize * y) + offset, particleDamping);
						particleIndices.Add(composite.simElements.Count);
						composite.simElements.Add(p);

						var cfc = new ConstantForceConstraint(p, gravity);
						composite.simElements.Add(cfc);

						var s = new SpringConstraint(rowParticles[x], p, springStiffness);
						// var s = new DistanceConstraint(rowParticles[x], p);
						springIndices.Add(composite.simElements.Count);
						composite.simElements.Add(s);

						if(x != 0) {
							s = new SpringConstraint(rowParticles[x - 1], p, springStiffness);
							// s = new DistanceConstraint(rowParticles[x - 1], p);
							springIndices.Add(composite.simElements.Count);
							composite.simElements.Add(s);
						}

						rowParticles[x] = p;
					}
				}
			}

			// composite.simElements = SimElementHelper.ReorderSimElements(composite.simElementSs, "Particle", "SpringConstraint", "PinConstraint");

			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(1, springIndices));
			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(0, particleIndices));

			return composite;
		}
	}
}