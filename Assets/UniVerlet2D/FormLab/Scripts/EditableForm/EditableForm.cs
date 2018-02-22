using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D.Lab {

	public class EditableForm {

		/*
		 * Inner class
		 */

		public class SimElemGroup {

			/*
			 * Fields
			 */

			SimElemDefine.SimElemProfile _profile;
			List<SimElemInfo> _elements;

			/*
			 * Properties
			 */

			public SimElemDefine.SimElemProfile profile { get { return _profile; } }
			public int numElems { get { return _elements.Count; } }

			/*
			 * Constructor
			 */

			public SimElemGroup(SimElemDefine.SimElemProfile profile) {
				_profile = profile;
				this._elements = new List<SimElemInfo>();
			}

			/*
			 * Methods
			 */

			public void Add(SimElemInfo elem) {
				_elements.Add(elem);
			}

			public void Delete(SimElemInfo elem) {
				var idx = GetIdxByUID(elem.uid);
				if(idx > 0) {
					DeleteAt(idx);
				}
			}

			public void DeleteAt(int idx) {
				_elements.RemoveAt(idx);
			}

			public SimElemInfo GetAt(int idx) {
				return _elements[idx];
			}

			public SimElemInfo GetByUID(int uid) {
				var idx = GetIdxByUID(uid);
				if(idx > 0) {
					return GetAt(idx);
				}
				return null;
			}

			public int GetIdxByUID(int uid) {
				for(var i = 0; i < _elements.Count; ++i) {
					var e = GetAt(i);
					if(e.uid == uid) {
						return i;
					}
				}
				return -1;
			}

			public void Clear() {
				_elements.Clear();
			}
		}

		Dictionary<int, SimElemGroup> _elemGroupDic;

		/*
		 * Constructor
		 */

		public EditableForm() {
			_elemGroupDic = new Dictionary<int, SimElemGroup>();
		}

		/*
		 * Element related
		 */

		public void AddGroup(SimElemDefine.SimElemProfile profile) {
			int id = profile.tableID;
			if(_elemGroupDic.ContainsKey(id)) {
				return;
			}
			_elemGroupDic.Add(id, new SimElemGroup(profile));
		}

		public void AddElemInfo(SimElemInfo elemInfo) {
			var profile = SimElemDefine.GetProfile(elemInfo.profileID);
			SimElemGroup group;
			if(!_elemGroupDic.TryGetValue(profile.tableID, out group)) {
				AddGroup(profile);
				group = _elemGroupDic[profile.tableID];
			}
			group.Add(elemInfo);
		}

		public SimElemInfo GetByUID(int id, int uid) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				return group.GetByUID(uid);
			}
			return null;
		}

		public SimElemInfo GetByUID(int uid) {
			foreach(var group in _elemGroupDic.Values) {
				for(var i = 0; i < group.numElems; ++i) {
					var elem = group.GetAt(i);
					if(elem.uid == uid) {
						return group.GetAt(i);
					}
				}
			}
			return null;
		}

		public SimElemInfo GetAt(int id, int idx) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				return group.GetAt(idx);
			}
			return null;
		}

		/*
		 * methods
		 */

		public void ClearAllElemGroup() {
			_elemGroupDic.Clear();
		}

		public List<int> GetSortedKeyList() {
			List<int> keys = new List<int>(_elemGroupDic.Keys);
			keys.Sort((x, y) => x < y ? -1 : (x > y ? 1 : 0));
			return keys;
		}

		/*
		 * Export and import
		 */

		public string ExportFormattedText() {
			StringBuilder sb = new StringBuilder();
			string header = " ";

			foreach(int id in _elemGroupDic.Keys) {
				var group = _elemGroupDic[id];
				header = group.profile.header;

				for(var i = 0; i < group.numElems; ++i) {
					var e = group.GetAt(i);
					sb.AppendLine(string.Format("{0} {1}", header, e.ExportJson()));
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		public void ImportFormattedText(string text) {
			if(string.IsNullOrEmpty(text)) {
				return;
			}

			ClearAllElemGroup();

			var lines = text.Split('\n');

			for(var i = 0; i < lines.Length; ++i) {
				var separate = lines[i].Split(' ');
				if(separate.Length != 2) {
					continue;
				}

				var profile = SimElemDefine.GetProfileFromHeader(separate[0]);
				var info = JsonUtility.FromJson(separate[1], profile.makeSimElemInfoType) as SimElemInfo;
				info.AfterImportJson(this);

				AddElemInfo(info);
			}
		}

		public AlignedEditableForm ExportAlignedEditableForm() {
			List<SimElemInfo> alignedSimElemInfo = new List<SimElemInfo>();

			List<int> sortedKeys = GetSortedKeyList();
			Dictionary<int, int> uid2idxDic = new Dictionary<int, int>();

			List<int> renderedSimElemIdx = new List<int>();

			bool canRender;

			for(int i = 0; i < sortedKeys.Count; ++i) {
				var group = _elemGroupDic[sortedKeys[i]];
				canRender = group.profile.canRender;
				for(var j = 0; j < group.numElems; ++j) {
					if(canRender) {
						renderedSimElemIdx.Add(alignedSimElemInfo.Count);
					}
					var elemInfo = group.GetAt(j);
					uid2idxDic.Add(elemInfo.uid, alignedSimElemInfo.Count);
					alignedSimElemInfo.Add(elemInfo);
				}
			}

			var exportForm = new AlignedEditableForm();
			exportForm.simElemInfos = alignedSimElemInfo;
			exportForm.uid2idxDic = uid2idxDic;
			exportForm.renderedSimElemIdx = renderedSimElemIdx;

			return exportForm;
		}
	}
}