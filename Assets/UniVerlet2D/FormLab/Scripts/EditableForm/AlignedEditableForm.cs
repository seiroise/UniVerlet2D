using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class AlignedEditableForm {
		public List<SimElemInfo> simElemInfos;
		public Dictionary<int, int> uid2idxDic;
		public List<int> renderedSimElemIdx;

		public List<SimElement> ExportSimElements() {
			List<SimElement> simElements = new List<SimElement>();

			for(var i = 0; i < simElemInfos.Count; ++i) {
				simElements.Add(simElemInfos[i].MakeSimElement(this, simElements));
			}

			return simElements;
		}
	}
}