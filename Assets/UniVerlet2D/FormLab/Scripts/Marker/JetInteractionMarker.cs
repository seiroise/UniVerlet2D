using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class JetInteractionMarker : SpriteMarker {

		public override bool SetSimElemInfo(SimElemInfo info, float depth) {
			base.SetSimElemInfo(info, depth);

			var jInfo = info as JetInteractionInfo;
			if(jInfo == null) {
				return false;
			}

			Vector3 pos = jInfo.a.pos;
			pos.z = depth;
			transform.position = pos;

			return true;
		}
	}
}