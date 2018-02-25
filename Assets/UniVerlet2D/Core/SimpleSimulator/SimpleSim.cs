using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class SimpleSim {

		/*
		 * Fields
		 */

		[SerializeField]
		SimSettings _settings;
		List<SimElement> _simElements;

		public SimpleSim() {
			Clear();
		}

		/*
		 * Methods
		 */

		public void Init(bool withClear = true) {
			_simElements = new List<SimElement>();
		}

		public void Update(float dt) {
			for(var i = 0; i < _simElements.Count; ++i) {
				var element = _simElements[i];
				element.Step(dt);
			}
		}

		public void Clear() {
			if(_simElements == null) {
				_simElements = new List<SimElement>();
			} else {
				_simElements.Clear();
			}
		}

		public bool IsRangeInside(int idx) {
			return 0 <= idx && idx < _simElements.Count;
		}

		/*
		 * Sim element methods
		 */

		public void AddSimElement(SimElement simElem) {
			_simElements.Add(simElem);
		}

		public SimElement GetSimElementAt(int idx) {
			if(IsRangeInside(idx)) {
				return _simElements[idx];
			}
			return null;
		}

		public void RemoveSimElementAt(int idx) {
			if(IsRangeInside(idx)) {
				_simElements.RemoveAt(idx);
			}
		}

		/*
		 * Import / Export
		 */

		public void ImportFromSimElementList(List<SimElement> simElements) {
			_simElements.Clear();
			_simElements = simElements;
		}
	}
}