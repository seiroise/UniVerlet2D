using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class SpringConstraintMarker : SpriteMarker {

		public override bool SetSimElemInfo(SimElemInfo info, float depth) {
			base.SetSimElemInfo(info, depth);

			var s = info as SpringConstraintInfo;
			if(s == null) {
				return false;
			}

			Vector3 pos = s.middlePos;
			pos.z = depth;
			transform.position = pos;
			transform.rotation = Quaternion.AngleAxis(s.a2bRadian * Mathf.Rad2Deg, Vector3.forward);
			transform.localScale = new Vector3(s.currentLength, 0.4f, 1f);
			return true;
		}
	}
}