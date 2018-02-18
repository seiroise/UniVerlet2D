using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class DeleteAngleCommand : ICommand {

		Simulator _sim;
		MarkerManager _marker;
		int _uid;
		int _aUID, _bUID, _mUID;
		float _stiffness;

		AngleConstraint _a;

		public string name { get { return "Delete angle"; } }

		public DeleteAngleCommand(Simulator sim, MarkerManager marker, int uid) {
			_sim = sim;
			_marker = marker;
			_uid = uid;

			_a = _sim.GetAngleByUID(uid);
			_aUID = _a.a.uid;
			_bUID = _a.b.uid;
			_mUID = _a.m.uid;
			_stiffness = _a.stiffness;
		}

		bool ICommand.Do() {
			_sim.DeleteAngle(_a);
			_marker.DeleteAngleMarker(_a);
			return true;
		}

		void ICommand.Undo() {
			var current = _sim.uidDistributer.current;

			_a = _sim.MakeAngleByUID(_aUID, _bUID, _mUID, _stiffness);
			_a.OverrideUID(_uid);

			_sim.uidDistributer.SetCounter(current);

			_marker.MakeAngleMarker(_a);
		}
	}
}