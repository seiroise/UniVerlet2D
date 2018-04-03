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
		[SerializeField]
		float _damping;

		/*
		 * Properties
		 */

		public Vector2 pos { get { return _pos; } set { _pos = value; } }
		public float damping { get { return _damping; } set { _damping = value; } }

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

			_damping = 0.9f;

			return true;
		}

		public override SimElement MakeSimElement(AlignedEditableForm aef, List<SimElement> simElements) {
			var p = new Particle(_pos, _damping);
			p.OverrideUID(uid);

			return p;
		}

		public override void AfterImportJson(EditableForm form) { }

		public override bool ContainsUID(int uid) {
			return false;
		}
	}
}