using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D.Lab {

	public class EditableForm {

		public class SimElemGroup {

			public int loopGroupID;
			public string type;

			public bool useParticle;
			public bool useSpring;
			public bool useAngle;

			List<SimElemInfo> _elements;

			public int numElems { get { return _elements.Count; } }

			public SimElemGroup(int loopGroupID, string type) {
				this.loopGroupID = loopGroupID;
				this.type = type;
				this._elements = new List<SimElemInfo>();
			}

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
		 * Element related
		 */

		public void AddGroup(int id, int loopGroupID, string type) {
			if(_elemGroupDic.ContainsKey(id)) {
				return;
			}
			_elemGroupDic.Add(id, new SimElemGroup(loopGroupID, type));
		}

		void AddElement(int id, SimElemInfo elem) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				group.Add(elem);
			}
		}

		public SimElemInfo GetByUID(int id, int uid) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				return group.GetByUID(uid);
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
		 *  functions
		 */

		public List<SimElemInfo> SerializeSimeElements() {
			Dictionary<int, List<SimElemInfo>> simElems = new Dictionary<int, List<SimElemInfo>>();

			foreach(int id in _elemGroupDic.Keys) {
				if(!simElems.ContainsKey(id)) {
					simElems.Add(id, new List<SimElemInfo>());
				}
			}

			return null;
		}

		/*
		 * Export and import
		 */

		public FormText ExportFormText() {

			StringBuilder sb = new StringBuilder();
			string header;

			foreach(int id in _elemGroupDic.Keys) {
				var group = _elemGroupDic[id];
				header = Type2Header(group.type);

				for(var i = 0; i < group.numElems; ++i) {
					var e = group.GetAt(i);
					sb.AppendLine(string.Format("{0} {1}", header, e.ExportJson()));
				}
				sb.AppendLine();
			}

			var formText = new FormText();
			formText.text = sb.ToString();
			return formText;
		}

		string Type2Header(string type) {
			switch(type) {
			case "Particle":
				return "p";
			default:
				return null;
			}
		}
	}
}