using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class ParticleMarker : SpriteMarker {

		public override bool SetSimElemInfo(SimElemInfo info, float depth) {
			base.SetSimElemInfo(info, depth);

			var pInfo = info as ParticleInfo;
			if(pInfo == null) {
				return false;
			}

			Vector3 pos = pInfo.pos;
			pos.z = depth;
			transform.position = pos;

			return true;
		}

	}
}