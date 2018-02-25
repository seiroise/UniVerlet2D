using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PP2D {

	[System.Serializable]
	public class Simulator {

		[System.Serializable]
		public class ChangeCompositionEvent : UnityEvent { }

		/*
		 * Fields
		 */

		List<SimElement> _simElements;
		[SerializeField]
		ChangeCompositionEvent _onChangeComposition;

		/*
		 * Properties
		 */

		public int numElements { get { return _simElements.Count; } }
		public ChangeCompositionEvent onChangeComposition { get { return _onChangeComposition != null ? _onChangeComposition : _onChangeComposition = new ChangeCompositionEvent(); } }

		/*
		 * Constructor
		 */

		public Simulator() {
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
			if(_onChangeComposition != null) {
				_onChangeComposition.Invoke();
			}
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
				if(_onChangeComposition != null) {
					_onChangeComposition.Invoke();
				}
			}
		}

		public List<T> FindSimElems<T>() where T : SimElement {
			return SimElementHelper.FindSimElems<T>(_simElements);
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