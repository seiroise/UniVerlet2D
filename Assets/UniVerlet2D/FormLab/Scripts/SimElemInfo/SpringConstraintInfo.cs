using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class SpringConstraintInfo : SimElemInfo {

		/*
		 * Fields
		 */

		ParticleInfo _a, _b;

		[SerializeField]
		float _stiffness;
		[SerializeField]
		int _aUID, _bUID;

		/*
		 * Properties
		 */

		public ParticleInfo a { get { return _a; } }
		public ParticleInfo b { get { return _b; } }

		public float stiffness { get { return _stiffness; } set { _stiffness = value; } }
		public float currentLength { get { return (_a.pos - _b.pos).magnitude; } }
		public Vector2 middlePos { get { return (_a.pos + _b.pos) * 0.5f; } }
		public float a2bRadian { get { var rad = Mathf.Atan2(_b.pos.y - _a.pos.y, _b.pos.x - _a.pos.x); return float.IsNaN(rad) ? 0f : rad; } }

		/*
		 * Methods
		 */

		public override SimElement MakeSimElement(AlignedEditableForm aef, List<SimElement> simElements) {
			var a = simElements[aef.uid2idxDic[_aUID]] as Particle;
			var b = simElements[aef.uid2idxDic[_bUID]] as Particle;

			var sc = new SpringConstraint(a, b, _stiffness);
			sc.OverrideUID(uid);

			return sc;
		}

		public override bool SetParams(int uid, string profileID, object[] args) {
			base.SetParams(uid, profileID, args);

			if(args.Length != 2) {
				return false;
			}

			_a = args[0] as ParticleInfo;
			_b = args[1] as ParticleInfo;

			_aUID = _a.uid;
			_bUID = _b.uid;

			_stiffness = 1f;

			return true;
		}

		public override void AfterImportJson(EditableForm form) {
			_a = form.GetByUID(_aUID) as ParticleInfo;
			_b = form.GetByUID(_bUID) as ParticleInfo;
		}

		public override bool ContainsUID(int uid) {
			return _a.uid == uid || _b.uid == uid;
		}
	}
}