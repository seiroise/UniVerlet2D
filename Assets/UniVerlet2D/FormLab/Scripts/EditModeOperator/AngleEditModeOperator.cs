using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class AngleEditModeOperator : IEditModeOperator {

		FormLab _lab;

		List<SimElemMarker> _selectedMarkers;

		public AngleEditModeOperator(FormLab lab) {
			_lab = lab;
			_selectedMarkers = new List<SimElemMarker>();
		}

		public void Update() {
		}

		public void EnterMode() {
		}

		public void ExitMode() {
		}

		public void DownMarker(SimElemMarker marker) {
			switch(_lab.editMethod) {
			case FormLab.EditMethod.Make:
				if(marker.elemType == MarkerManager.PARTICLE_ID) {
					_selectedMarkers.Add(marker);
					if(_selectedMarkers.Count > 2) {
						_lab.MakeAngle(
							_selectedMarkers[0].uid,
							_selectedMarkers[2].uid,
							_selectedMarkers[1].uid, 1f);
						_selectedMarkers.Clear();
					}
				}
				break;
			case FormLab.EditMethod.Delete:
				if(marker.elemType == MarkerManager.ANGLE_ID) {
					_lab.DeleteAngle(marker.uid);
				}
				break;
			}
		}

		public void DownSpace(Vector3 pos) {
			
		}

		public void EnterMarker(SimElemMarker marker) {
			throw new System.NotImplementedException();
		}

		public void ExitMarker(SimElemMarker marker) {
			throw new System.NotImplementedException();
		}
	}
}