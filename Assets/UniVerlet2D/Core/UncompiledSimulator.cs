using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class UncompiledSimulator {

		public class SimElemGroup {
			public int groupID;
			public List<SimElement> elements;

			public SimElemGroup(int groupID) {
				this.groupID = groupID;
				this.elements = new List<SimElement>();
			}

			public void Add(SimElement elem) {
				elements.Add(elem);
			}

			public void Delete(SimElement elem) {
				var idx = GetIdxByUID(elem.uid);
				if(idx > 0) {
					DeleteAt(idx);
				}
			}

			public void DeleteAt(int idx) {
				elements.RemoveAt(idx);
			}

			public SimElement GetAt(int idx) {
				return elements[idx];
			}

			public SimElement GetByUID(int uid) {
				var idx = GetIdxByUID(uid);
				if(idx > 0) {
					return GetAt(idx);
				}
				return null;
			}

			public int GetIdxByUID(int uid) {
				for(var i = 0; i < elements.Count; ++i) {
					var e = GetAt(i);
					if(e.uid == uid) {
						return i;
					}
				}
				return -1;
			}

			public void Clear() {
				elements.Clear();
			}
		}

		Dictionary<int, SimElemGroup> _elemGroupDic;

		/*
		 * Methods
		 */

		public void AddGroup(int id, int groupID) {
			if(_elemGroupDic.ContainsKey(id)) {
				return;
			}
			_elemGroupDic.Add(id, new SimElemGroup(groupID));
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
	}
}