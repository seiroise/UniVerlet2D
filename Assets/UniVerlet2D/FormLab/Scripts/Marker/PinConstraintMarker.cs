using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class PinConstraintMarker : SpriteMarker {
		
		public override bool SetSimElemInfo(SimElemInfo info, float depth) {
			base.SetSimElemInfo(info, depth);

			var a = info as PinConstraintInfo;
			if(a == null) {
				return false;
			}

			Vector3 pos = a.pos;
			pos.z = depth;
			transform.position = pos;

			return true;
		}

	}
}