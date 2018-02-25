using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public static class SimElementHelper {

		public static List<int> FindSimElemIndices(List<SimElement> list, string filterName) {
			List<int> founds = new List<int>();
			for(var i = 0; i < list.Count; ++i) {
				if(list[i].name.Equals(filterName)) {
					founds.Add(i);
				}
			}
			return founds;
		}

		public static List<SimElement> FindSimElems(List<SimElement> list, string filterName) {
			List<SimElement> founds = new List<SimElement>();
			for(var i = 0; i < list.Count; ++i) {
				if(list[i].name.Equals(filterName)) {
					founds.Add(list[i]);
				}
			}
			return founds;
		}

		public static List<T> FindSimElems<T>(List<SimElement> list) where T : SimElement {
			List<T> founds = new List<T>();
			var findName = typeof(T).Name;
			for(var i = 0; i < list.Count; ++i) {
				if(list[i].name.Equals(findName)) {
					founds.Add(list[i] as T);
				}
			}
			return founds;
		}

		public static bool FindOverlapParticleIdx(List<Particle> list, Vector2 pos, float particleRadius, out int idx) {
			float sqrRadius = particleRadius * particleRadius;
			for(var i = 0; i < list.Count; ++i) {
				var p = list[i];
				if((pos - p.pos).sqrMagnitude <= sqrRadius) {
					idx = i;
					return true;
				}
			}
			idx = -1;
			return false;
		}

		public static List<SimElement> ReorderSimElements(List<SimElement> list, params string[] order) {
			HashSet<int> indicesSet = new HashSet<int>();

			for(var i = 0; i < order.Length; ++i) {
				var founds = FindSimElemIndices(list, order[i]);
				for(var j = 0; j < founds.Count; ++j) {
					indicesSet.Add(founds[j]);
				}
			}

			for(var i = 0; i < list.Count; ++i) {
				if(!indicesSet.Contains(i)) {
					indicesSet.Add(i);
				}
			}

			List<SimElement> reorderedList = new List<SimElement>();
			foreach(int i in indicesSet) {
				reorderedList.Add(list[i]);
			}
			return reorderedList;
		}
	}
}