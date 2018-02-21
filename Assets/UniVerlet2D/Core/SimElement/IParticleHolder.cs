using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public interface IParticleHolder {

		bool ContainParticle(Particle p);
	}
}