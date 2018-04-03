using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D;

namespace PP2D.Examples {

	public class Grassland : MonoComposite {

		[Header("Generate settings")]

		[Range(2, 32)]
		public int seeds = 10;
		public Vector2 start, end;

		[MinMaxRange(1, 10)]
		public MinMax jointsRange;
		[MinMaxRange(0.1f, 10f)]
		public MinMax branchLengthRange;
		[MinMaxRange(-90f, 360f)]
		public MinMax angleRange;

		Composite composite { get; set; }

		List<int> particleIndices { get; set; }
		List<int> branchSpringIndices { get; set; }
		List<int> leafSpringIndices { get; set; }

		public override Composite MakeComposite() {
			composite = new Composite();

			particleIndices = new List<int>();
			branchSpringIndices = new List<int>();
			leafSpringIndices = new List<int>();

			for(var i = 0; i <= seeds; ++i) {
				MakePlant(Vector2.Lerp(start, end, Mathf.InverseLerp(0, seeds, i)), jointsRange.randomInt, Mathf.PI * 0.5f);
			}

			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(2, branchSpringIndices));
			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(0, particleIndices));
			composite.renderingGroups.Add(new SimRenderer.SimRenderingGroup(1, leafSpringIndices));

			return composite;
		}

		void MakePlant(Vector2 basePosition, int joints, float baseAngle) {
			Particle root = new Particle(basePosition);
			particleIndices.Add(composite.simElements.Count);
			composite.simElements.Add(root);

			Particle prev = root;
			Particle prev2 = root;
			Particle current = root;
			PinConstraint firstPin = null;
			float angle = baseAngle;
			for(var i = 0; i <= joints; ++i) {
				angle += angleRange.random * Mathf.Deg2Rad;
				if(i == joints) {
					Particle leaf = new Particle(prev.pos + AngleToVector2(angle) * branchLengthRange.random);
					particleIndices.Add(composite.simElements.Count);
					composite.simElements.Add(leaf);

					SpringConstraint leafSpring = new SpringConstraint(prev, leaf, springStiffness);
					leafSpringIndices.Add(composite.simElements.Count);
					composite.simElements.Add(leafSpring);

					current = leaf;
				} else {
					Particle joint = new Particle(prev.pos + AngleToVector2(angle) * branchLengthRange.random);
					particleIndices.Add(composite.simElements.Count);
					composite.simElements.Add(joint);

					SpringConstraint spring = new SpringConstraint(prev, joint, springStiffness);
					branchSpringIndices.Add(composite.simElements.Count);
					composite.simElements.Add(spring);

					current = joint;

					if(i == 0) {
						firstPin = new PinConstraint(joint);
					}
				}
				if(i > 0) {
					AngleConstraint ac = new AngleConstraint(prev2, current, prev, angleStiffness);
					composite.simElements.Add(ac);
				}
				prev2 = prev;
				prev = current;
			}

			PinConstraint rootPin = new PinConstraint(root);
			composite.simElements.Add(rootPin);

			composite.simElements.Add(firstPin);
		}

		Vector2 AngleToVector2(float angle) {
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}