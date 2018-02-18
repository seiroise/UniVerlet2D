using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public interface IEditModeOperator {

		void Update();

		void EnterMode();
		void ExitMode();

		void DownMarker(SimElemMarker marker);
		void DownSpace(Vector3 pos);

		void EnterMarker(SimElemMarker marker);
		void ExitMarker(SimElemMarker marker);
	}
}