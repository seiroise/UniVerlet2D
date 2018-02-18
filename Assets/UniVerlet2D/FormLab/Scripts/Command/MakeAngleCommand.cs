using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class MakeAngleCommand : ICommand {

		Simulator _sim;
		MarkerManager _marker;
		int _uid;
		int _aUID, _bUID, _mUID;
		float _stiffness;

		AngleConstraint _a;

		public string name { get { return "Make angle"; } }

		public MakeAngleCommand(Simulator sim, MarkerManager marker, int aUID, int bUID, int mUID, float stiffness) {
			_sim = sim;
			_marker = marker;
			_uid = -1;

			_aUID = aUID;
			_bUID = bUID;
			_mUID = mUID;
			_stiffness = stiffness;
		}

		bool ICommand.Do() {
			if(_uid == -1) {
				Debug.Log(_aUID + " -> " + _bUID + " + " + _mUID);
				_a = _sim.MakeAngleByUID(_aUID, _bUID, _mUID, _stiffness);
				_uid = _a.uid;
			} else {
				var current = _sim.uidDistributer.current;

				_a = _sim.MakeAngleByUID(_aUID, _bUID, _mUID, _stiffness);
				_a.OverrideUID(_uid);

				_sim.uidDistributer.SetCounter(current);

				_marker.MakeAngleMarker(_a);
			}

			_marker.MakeAngleMarker(_a);
			return true;
		}

		void ICommand.Undo() {
			_sim.DeleteAngle(_a);
			_marker.DeleteAngleMarker(_a);
		}
	}
}