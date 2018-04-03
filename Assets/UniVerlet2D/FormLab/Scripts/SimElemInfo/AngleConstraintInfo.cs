using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class AngleConstraintInfo : SimElemInfo {

		/*
		 * Fields
		 */

		ParticleInfo _a, _b, _m;

		[SerializeField]
		float _stiffness;

		[SerializeField]
		int _aUID, _bUID, _mUID;

		/*
		 * Properties
		 */

		public ParticleInfo a { get { return _a; } }
		public ParticleInfo b { get { return _b; } }
		public ParticleInfo m { get { return _m; } }

		public float aAngle { get { var d = _a.pos - _m.pos; return Mathf.Atan2(d.y, d.x); } }
		public float bAngle { get { var d = _b.pos - _m.pos; return Mathf.Atan2(d.y, d.x); } }

		public float stiffness { get { return _stiffness; } set { _stiffness = value; } }

		/*
		 * Methods
		 */

		public override SimElement MakeSimElement(AlignedEditableForm aef, List<SimElement> simElements) {
			var a = simElements[aef.uid2idxDic[_aUID]] as Particle;
			var b = simElements[aef.uid2idxDic[_bUID]] as Particle;
			var m = simElements[aef.uid2idxDic[_mUID]] as Particle;

			var ac = new AngleConstraint(a, b, m, _stiffness);
			ac.OverrideUID(uid);

			return ac;
		}

		public override bool SetParams(int uid, string profileID, object[] args) {
			base.SetParams(uid, profileID, args);

			if(args.Length != 3) {
				return false;
			}

			_a = args[0] as ParticleInfo;
			_b = args[2] as ParticleInfo;
			_m = args[1] as ParticleInfo;

			_aUID = _a.uid;
			_bUID = _b.uid;
			_mUID = _m.uid;

			_stiffness = 1f;

			return true;
		}

		public override void AfterImportJson(EditableForm form) {
			_a = form.GetByUID(_aUID) as ParticleInfo;
			_b = form.GetByUID(_bUID) as ParticleInfo;
			_m = form.GetByUID(_mUID) as ParticleInfo;
		}

		public override bool ContainsUID(int uid) {
			return _a.uid == uid || _b.uid == uid || _m.uid == uid;
		}
	}
}