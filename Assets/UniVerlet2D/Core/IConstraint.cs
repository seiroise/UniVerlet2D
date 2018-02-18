using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public interface IConstraint {

		void Relax(float dt);

		bool ContainParticle(Particle p);
	}
}