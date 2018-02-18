using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	[RequireComponent(typeof(IFormLayer))]
	public class SimInteractionController : MonoBehaviour {

		[System.Serializable]
		public class InteractionGroup {
			public string triggerName;
			public List<int> interactionIdx;
		}

		/*
		 * Fields
		 */

		public KeyInputMap inputMap;

		Dictionary<string, InteractionGroup> _interactionGroupDic;

		Simulator _sim;

		/*
		 * Unity events
		 */

		void Awake() {
			_sim = GetComponent<IFormLayer>().sim;
			_interactionGroupDic = new Dictionary<string, InteractionGroup>();
		}

		void Update() {
			inputMap.InputCheck();
		}

		/*
		 * Callback
		 */

		public void OnDetectDown(string message) {
			InteractionGroup iGroup;
			if(_interactionGroupDic.TryGetValue(message, out iGroup)) {
				for(var i = 0; i < iGroup.interactionIdx.Count; ++i) {
					// var interaction = _sim.GetInteractionAt(iGroup.interactionIdx[i]);
					// interaction.TurnOn();
				}
			}
		}

		public void OnDetectUp(string message) {
			InteractionGroup iGroup;
			if(_interactionGroupDic.TryGetValue(message, out iGroup)) {
				for(var i = 0; i < iGroup.interactionIdx.Count; ++i) {
					// var interaction = _sim.GetInteractionAt(iGroup.interactionIdx[i]);
					// interaction.TurnOff();
				}
			}
		}

		/*
		 * Methods
		 */

		public void AddInteraction(string triggerName, int idx) {
			InteractionGroup group;
			if(_interactionGroupDic.TryGetValue(triggerName, out group)) {
				group.interactionIdx.Add(idx);
			}
		}
	}
}