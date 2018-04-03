using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PP2D.Examples;

namespace PP2D.Plantae {

	public class GroundCover0 : MonoComposite {

		public Vector3 from;
		public Vector3 to;

		public override Composite MakeComposite() {
			Composite composite = new Composite();

			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(2, new List<int>()));
			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(0, new List<int>()));
			composite.AddRenderingGroup(new SimRenderer.SimRenderingGroup(1, new List<int>()));

			Particle root = new Particle(Vector2.zero, particleDamping);
			// composite.renderingGroups[1].indices.Add(composite.elemNum);
			composite.AddSimElement(new PinConstraint(root));
			Particle next = MakeLimb(composite, root, 1f, Mathf.PI * 0.5f);
			composite.AddSimElement(new PinConstraint(next));

			return composite;
		}

		Particle MakeLimb(Composite composite, Particle baseParticle, float branchLength, float angle) {
			Particle p = new Particle(baseParticle.pos + AngleToVector2(angle) * branchLength);
			SpringConstraint s = new SpringConstraint(baseParticle, p, springStiffness);
			return p;
		}

		Vector2 AngleToVector2(float angle) {
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}