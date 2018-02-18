using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class MakeSpringCommand : ICommand {

		Simulator _sim;
		MarkerManager _marker;

		int _uid;
		int _aUID, _bUID;
		float _stiffness;

		SpringConstraint _s;

		public string name { get { return "Make spring"; } }

		public MakeSpringCommand(Simulator sim, MarkerManager marker, int aUID, int bUID, float stiffness) {
			_sim = sim;
			_marker = marker;
			_uid = -1;

			_aUID = aUID;
			_bUID = bUID;
			_stiffness = stiffness;
		}

		public bool Do() {
			if(_uid == -1) {
				_s = _sim.MakeSpringByUID(_aUID, _bUID, _stiffness);
				_uid = _s.uid;
			} else {
				var current = _sim.uidDistributer.current;

				_s = _sim.MakeSpringByUID(_aUID, _bUID, _stiffness);
				_s.OverrideUID(_uid);

				_sim.uidDistributer.SetCounter(current);
			}
			_marker.MakeSpringMarker(_s);
			return true;
		}

		public void Undo() {
			_sim.DeleteSpring(_s);
			_marker.DeleteSpringMarker(_s);
		}
	}
}