using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class DeleteSpringCommand : ICommand {

		Simulator _sim;
		MarkerManager _marker;

		int _uid;
		int _aUID, _bUID;
		float _stiffness;

		SpringConstraint _s;

		public string name { get { return "Delete spring"; } }

		public DeleteSpringCommand(Simulator sim, MarkerManager marker, int uid) {
			_sim = sim;
			_marker = marker;
			_uid = uid;

			_s = _sim.GetSpringByUID(uid);
			_aUID = _s.a.uid;
			_bUID = _s.b.uid;
			_stiffness = _s.stiffness;
		}

		public bool Do() {
			_sim.DeleteSpringByUID(_uid);
			_marker.DeleteSpringMarker(_s);
			return true;
		}

		public void Undo() {
			var current = _sim.uidDistributer.current;

			_s = _sim.MakeSpringByUID(_aUID, _bUID, _stiffness);
			_s.OverrideUID(_uid);

			_sim.uidDistributer.SetCounter(current);

			_marker.MakeSpringMarker(_s);
		}
	}
}