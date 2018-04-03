using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PP2D {

	[System.Serializable]
	public class Simulator {

		public class SimulationOrder {
			public int order;
			public List<SimElement> simElements;

			public SimulationOrder(int order) {
				this.order = order;
				this.simElements = new List<SimElement>();
			}
		}

		[System.Serializable]
		public class ChangeCompositionEvent : UnityEvent { }

		/*
		 * Fields
		 */

		List<SimElement> _simElements;
		[SerializeField]
		ChangeCompositionEvent _onChangeComposition;
		bool isDirty;

		List<SimulationOrder> _simulationOrders = new List<SimulationOrder>();

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
			_simulationOrders = new List<SimulationOrder>();
		}

		public void Update(float dt) {
			for(var i = 0; i < _simElements.Count; ++i) {
				var element = _simElements[i];
				element.Step(dt);
			}

			if(isDirty && _onChangeComposition != null) {
				_onChangeComposition.Invoke();
				isDirty = false;
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
			isDirty = true;
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
				isDirty = true;
			}
		}

		public List<T> GetSimElems<T>() where T : SimElement {
			return SimElementHelper.GetSimElems<T>(_simElements);
		}

		public List<int> GetSimElemIndices<T>() where T : SimElement {
			return SimElementHelper.GetSimElemIndices<T>(_simElements);
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