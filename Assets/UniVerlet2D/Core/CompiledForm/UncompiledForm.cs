using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D {

	public class UncompiledForm {

		public class SimElemGroup {
			
			public int groupID;
			public string type;

			public bool useParticle;
			public bool useSpring;
			public bool useAngle;

			List<SimElement> _elements;

			public int numElems { get { return _elements.Count; } }

			public SimElemGroup(int groupID, string type) {
				this.groupID = groupID;
				this.type = type;
				this._elements = new List<SimElement>();
			}

			public void Add(SimElement elem) {
				_elements.Add(elem);
			}

			public void Delete(SimElement elem) {
				var idx = GetIdxByUID(elem.uid);
				if(idx > 0) {
					DeleteAt(idx);
				}
			}

			public void DeleteAt(int idx) {
				_elements.RemoveAt(idx);
			}

			public SimElement GetAt(int idx) {
				return _elements[idx];
			}

			public SimElement GetByUID(int uid) {
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

		public void AddGroup(int id, int groupID, string type) {
			if(_elemGroupDic.ContainsKey(id)) {
				return;
			}
			_elemGroupDic.Add(id, new SimElemGroup(groupID, type));
		}

		void AddElement(int id, SimElement elem) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				group.Add(elem);
			}
		}

		public SimElement GetByUID(int id, int uid) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				return group.GetByUID(uid);
			}
			return null;
		}

		public SimElement GetAt(int id, int idx) {
			SimElemGroup group;
			if(_elemGroupDic.TryGetValue(id, out group)) {
				return group.GetAt(idx);
			}
			return null;
		}

		/*
		 *  functions
		 */

		public List<SimElement> SerializeSimeElements() {
			Dictionary<int, List<SimElement>> simElems = new Dictionary<int, List<SimElement>>();

			foreach(int id in _elemGroupDic.Keys) {
				if(!simElems.ContainsKey(id)) {
					simElems.Add(id, new List<SimElement>());
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