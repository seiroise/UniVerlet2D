using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Data {

	[System.Serializable]
	public class SpinalFormBuilder {

		/*
		 * Fields
		 */

		[Range(1, 100)]
		public int boneNum = 10;
		[Range(1f, 100f)]
		public float bodyLength = 10f;

		Simulator _sim;

		/*
		 * Functions
		 */

		public Form Build(Vector2 rootPosition) {
			if(_sim == null) {
				_sim = new Simulator();
				_sim.Init();
			} else {
				_sim.Clear();
			}

			float boneLength = bodyLength / boneNum;

			var old = _sim.MakeParticle(rootPosition);
			var dir = new Vector2(0f, -1f);

			for(var i = 1; i < boneNum; ++i) {
				var p = _sim.MakeParticle(old.pos + dir * boneLength);
				_sim.MakeSpring(old, p);
				old = p;
			}
			for(var i = 2; i < boneNum; ++i) {
				_sim.MakeAngleByIdx(i - 2, i, i - 1);
			}

			return _sim.ExportForm();
		}
	}
}