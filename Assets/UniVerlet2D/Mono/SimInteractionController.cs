using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D.Lab;

namespace UniVerlet2D {

	public class SimInteractionController : MonoBehaviour {

		[System.Serializable]
		public class InteractionGroup {
			public string triggerName;
			public List<int> interactionUID;

			public List<int> interactionIdx;

			public InteractionGroup(string triggerName) {
				this.triggerName = triggerName;
				interactionUID = new List<int>();

				interactionIdx = new List<int>();
			}
		}

		/*
		 * Fields
		 */

		public KeyInputMap inputMap;
		public SimpleSim _sim;

		Dictionary<string, InteractionGroup> _interactionGroupDic;

		/*
		 * Unity events
		 */

		void Awake() {
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
				Debug.Log(message);
				for(int i = 0; i < iGroup.interactionIdx.Count; ++i) {
					var interaction = _sim.GetSimElementAt(iGroup.interactionIdx[i]) as Interaction;
					interaction.TurnOn();
				}
			}
		}

		public void OnDetectUp(string message) {
			InteractionGroup iGroup;
			if(_interactionGroupDic.TryGetValue(message, out iGroup)) {
				for(int i = 0; i < iGroup.interactionIdx.Count; ++i) {
					var interaction = _sim.GetSimElementAt(iGroup.interactionIdx[i]) as Interaction;
					interaction.TurnOff();
				}
			}
		}

		/*
		 * Methods
		 */

		public void AddInteractionUID(string triggerName, int uid) {
			InteractionGroup group;
			if(!_interactionGroupDic.TryGetValue(triggerName, out group)) {
				group = new InteractionGroup(triggerName);
				_interactionGroupDic.Add(triggerName, group);
			}
			group.interactionUID.Add(uid);
		}

		public void ConnectUID2IDX(AlignedEditableForm aef) {
			foreach(var group in _interactionGroupDic.Values) {
				for(var i = 0; i < group.interactionUID.Count; ++i) {
					group.interactionIdx.Add(aef.uid2idxDic[group.interactionUID[i]]);
				}
			}
		}

		public void SetSimulator(SimpleSim sim) {
			_sim = sim;
		}
	}
}