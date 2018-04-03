using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class SimpleSimHelper {

		public static List<Particle> GetParticlesByIdx(List<int> particleIdxs, SimpleSim sim) {
			List<Particle> particles = new List<Particle>(particleIdxs.Count);
			for(var i = 0; i < particleIdxs.Count; ++i) {
				particles.Add(sim.GetSimElementAt(i) as Particle);
			}
			return particles;
		}

		public static bool GetOverlapParticleIdx(List<Particle> particles, Vector2 pos, float particleRadius, out int idx) {
			float sqrRadius = particleRadius * particleRadius;
			for(var i = 0; i < particles.Count; ++i) {
				var p = particles[i];
				if((pos - p.pos).sqrMagnitude <= sqrRadius) {
					idx = i;
					return true;
				}
			}
			idx = -1;
			return false;
		}
	}
}