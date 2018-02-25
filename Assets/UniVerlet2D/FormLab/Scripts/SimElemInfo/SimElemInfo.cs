using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public abstract class SimElemInfo  {

		/*
		 * Fields
		 */

		[SerializeField]
		int _uid;				// 自身を表すためのID
		[SerializeField]
		string _profileID; 		// 対応するSimElementのprofileのID

		/*
		 * Properties
		 */

		public int uid { get { return _uid; } set { _uid = value; } }
		public string profileID { get { return _profileID; } }

		/*
		 * Methods
		 */

		public string ExportJson() {
			return JsonUtility.ToJson(this);
		}

		public virtual bool SetParams(int uid, string profileID, object[] args) {
			_uid = uid;
			_profileID = profileID;
			return true;
		}

		public abstract SimElement MakeSimElement(AlignedEditableForm aef, List<SimElement> simElements);

		public abstract void AfterImportJson(EditableForm form);

		public abstract bool ContainsUID(int uid);
	}
}