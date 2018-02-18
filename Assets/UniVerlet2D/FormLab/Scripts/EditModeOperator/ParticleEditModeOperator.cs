using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class ParticleEditModeOperator : IEditModeOperator {

		FormLab _lab;

		public ParticleEditModeOperator(FormLab lab) {
			_lab = lab;
		}

		public void Update() {
			
		}

		public void EnterMode() {

		}

		public void ExitMode() {

		}

		public void DownMarker(SimElemMarker marker) {
			switch(_lab.editMethod) {
			case FormLab.EditMethod.Delete:
				if(marker.elemType == MarkerManager.PARTICLE_ID) {
					_lab.DeleteParticle(marker.uid);
				}
				break;
			}
		}

		public void DownSpace(Vector3 pos) {
			switch(_lab.editMethod) {
			case FormLab.EditMethod.Make:
				_lab.MakeParticle(pos);
				break;
			}
		}

		public void EnterMarker(SimElemMarker marker) {
			
		}

		public void ExitMarker(SimElemMarker marker) {
			
		}
	}
}