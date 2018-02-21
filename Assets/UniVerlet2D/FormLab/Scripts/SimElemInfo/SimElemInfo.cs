using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public abstract class SimElemInfo  {

		int _uid;		// 自身を表すためのID
		int _profileID; // 対応するSimElementのprofileのID

		public int uid { get { return _uid; } set { _uid = value; } }
		public int profileID { get { return _profileID; } }

		public abstract SimElement MakeSimElement(Simulator sim);

		public string ExportJson() {
			return JsonUtility.ToJson(this);
		}
	}
}