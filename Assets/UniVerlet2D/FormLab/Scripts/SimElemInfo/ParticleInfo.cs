using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class ParticleInfo : SimElemInfo {

		/*
		 * Fields
		 */

		[SerializeField]
		Vector2 _pos;

		/*
		 * Properties
		 */

		public Vector2 pos { get { return _pos; } set { _pos = value; } }

		/*
		 * Methods
		 */

		public override bool SetParams(int uid, string profileID, object[] args) {
			base.SetParams(uid, profileID, args);

			if(args.Length != 1) {
				return false;
			}
			Vector2 pos = (args[0] as Nullable<Vector2>).Value;
			_pos = pos;
			return true;
		}

		public override SimElement MakeSimElement(Simulator sim) {
			var p = sim.MakeParticle(_pos);
			p.OverrideUID(uid);
			return p;
		}

		public override void AfterImportJson(EditableForm form) { }
	}
}