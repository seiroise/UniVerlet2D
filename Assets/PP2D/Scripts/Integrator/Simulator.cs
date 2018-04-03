using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PP2D {

	[System.Serializable]
	public class Simulator {

		public class SimulationOrder {
			public int order { get; set; }
			public List<int> indices { get; set; }

			public SimulationOrder(int order) {
				this.order = order;
				this.indices = new List<int>();
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

		List<SimElement> simElements { get { return _simElements; } set { _simElements = value; } }
		List<SimulationOrder> simulationOrders { get { return _simulationOrders; } set { _simulationOrders = value; } }
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
			simulationOrders = new List<SimulationOrder>();
		}

		public void Update(float dt) {
			/*
			for(var i = 0; i < _simElements.Count; ++i) {
				var element = _simElements[i];
				element.Step(dt);
			}
			*/
			for(var i = 0; i < simulationOrders.Count; ++i) {
				var order = simulationOrders[i];
				for(var j = 0; j < order.indices.Count; ++j) {
					simElements[order.indices[j]].Step(dt);
				}
			}

			if(isDirty && _onChangeComposition != null) {
				onChangeComposition.Invoke();
				isDirty = false;
			}
		}

		public void Clear() {
			if(simElements == null) {
				simElements = new List<SimElement>();
			} else {
				simElements.Clear();
			}
		}

		public bool IsRangeInside(int idx) {
			return 0 <= idx && idx < simElements.Count;
		}

		/*
		 * Sim element methods
		 */

		public void AddSimElement(SimElement simElem, int order = 0) {
			AddSimElementToOrder(order, simElements.Count);
			simElements.Add(simElem);
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

		void AddSimElementToOrder(int order, int index) {
			if(simulationOrders.Count == 0) {
				var simOrder = new SimulationOrder(order);
				simOrder.indices.Add(index);
				simulationOrders.Add(simOrder);
			} else {
				int insertPosition = 0;
				for(int i = 0; i < simulationOrders.Count; ++i) {
					if(simulationOrders[i].order == order) {
						simulationOrders[i].indices.Add(index);
						return;
					}
					if(simulationOrders[i].order < order) {
						insertPosition = i + 1;
					}
				}
				var simOrder = new SimulationOrder(order);
				simOrder.indices.Add(index);
				simulationOrders.Insert(insertPosition, simOrder);
			}
		}

		/*
		 * Import / Export
		 */

		public void ImportFromSimElementList(List<SimElement> simElements) {
			simElements.Clear();
			this.simElements = simElements;
		}
	}
}