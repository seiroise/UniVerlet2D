using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class JetInteractionInfo : SimElemInfo {

		/*
		 * Fields
		 */

		ParticleInfo _a, _b;

		[SerializeField]
		float _power;
		[SerializeField]
		int _aUID, _bUID;

		public ParticleInfo a { get { return _a; } }
		public ParticleInfo b { get { return _b; } }

		public override SimElement MakeSimElement(AlignedEditableForm aef, List<SimElement> simElements) {
			var a = simElements[aef.uid2idxDic[_aUID]] as Particle;
			var b = simElements[aef.uid2idxDic[_bUID]] as Particle;

			var jc = new JetInteraction(a, b, _power);
			jc.OverrideUID(uid);

			return jc;
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

			_power = 1f;

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