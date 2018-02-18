using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class DeleteParticleCommand : ICommand {

		Simulator _sim;
		MarkerManager _marker;
		Vector2 _pos;
		int _uid;

		Particle _p;

		List<SpringConstraint> _relatedSprings;
		List<AngleConstraint> _relatedAngles;
		List<PinConstraint> _relatedPins;

		public string name { get { return "Delete particle"; } }

		public DeleteParticleCommand(Simulator sim, MarkerManager marker, int uid) {
			_sim = sim;
			_marker = marker;
			_uid = uid;

			_p = sim.GetParticleByUID(uid);
			_pos = _p.pos;
		}

		bool ICommand.Do() {
			if(_relatedSprings == null) {
				_relatedSprings = new List<SpringConstraint>();
				_relatedAngles = new List<AngleConstraint>();
				_relatedPins = new List<PinConstraint>();
				_sim.DeleteParticle(_p, ref _relatedSprings, ref _relatedAngles, ref _relatedPins);
			} else {
				_sim.DeleteParticle(_p);
			}
			CommandHelper.DeleteParticleMarker(_marker, _p, _relatedSprings, _relatedAngles, _relatedPins);
			return true;
		}

		void ICommand.Undo() {
			var current = _sim.uidDistributer.current;

			_p = _sim.MakeParticle(_pos);
			_p.OverrideUID(_uid);

			for(var i = 0; i < _relatedSprings.Count; ++i) {
				var s = _sim.MakeSpringByUID(
					_relatedSprings[i].a.uid,
					_relatedSprings[i].b.uid,
					_relatedSprings[i].stiffness
				);
				s.OverrideUID(_relatedSprings[i].uid);
			}

			for(var i = 0; i < _relatedAngles.Count; ++i) {
				var a = _sim.MakeAngleByUID(
					_relatedAngles[i].a.uid,
					_relatedAngles[i].b.uid,
					_relatedAngles[i].m.uid,
					_relatedAngles[i].stiffness
				);
				a.OverrideUID(_relatedAngles[i].uid);
			}
			for(var i = 0; i < _relatedPins.Count; ++i) {
				var a = _sim.MakePinByUID(
					_relatedPins[i].a.uid,
					_relatedPins[i].pos
				);
				a.OverrideUID(_relatedAngles[i].uid);
			}

			_sim.uidDistributer.SetCounter(current);

			CommandHelper.MakeParticleMarker(_marker, _p, _relatedSprings, _relatedAngles, _relatedPins);
		}
	}
}