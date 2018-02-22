using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class PinConstraintInfo : SimElemInfo {

		/*
		 * Fields
		 */

		ParticleInfo _a;

		[SerializeField]
		Vector2 _pos;
		[SerializeField]
		int _aUID;

		/*
		 * Properties
		 */

		public ParticleInfo a { get { return _a; } }
		public Vector2 pos { get { return _pos; } }

		/*
		 * Methods
		 */

		public override SimElement MakeSimElement(Simulator sim) {
			return null;
		}

		public override bool SetParams(int uid, string profileID, object[] args) {
			base.SetParams(uid, profileID, args);

			if(args.Length != 2) {
				return false;
			}

			var a = args[0] as ParticleInfo;
			var pos = (args[1] as Nullable<Vector2>).Value;

			_a = a;
			_pos = pos;

			throw new System.NotImplementedException();
		}

		public override void AfterImportJson(EditableForm form) {
			form.GetByUID(_aUID);
		}
	}
}