using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public static class CommandHelper {

		/*
		 * Particle related
		 */

		public static void MakeParticleMarker(
			MarkerManager marker, Particle p,
			List<SpringConstraint> springs, List<AngleConstraint> angles, List<PinConstraint> pins
		) {
			marker.MakeParticleMarker(p);
			for(var i = 0; i < springs.Count; ++i) {
				marker.MakeSpringMarker(springs[i]);
			}
			for(var i = 0; i < angles.Count; ++i) {
				marker.MakeAngleMarker(angles[i]);
			}
			for(var i = 0; i < pins.Count; ++i) {
				marker.MakePinMarker(pins[i]);
			}
		}

		public static void DeleteParticleMarker(
			MarkerManager marker, Particle p,
			List<SpringConstraint> springs, List<AngleConstraint> angles, List<PinConstraint> pins
		) {
			marker.DeleteParticleMarker(p);
			for(var i = 0; i < springs.Count; ++i) {
				marker.DeleteSpringMarker(springs[i]);
			}
			for(var i = 0; i < angles.Count; ++i) {
				marker.DeleteAngleMarker(angles[i]);
			}
		}
	}
}