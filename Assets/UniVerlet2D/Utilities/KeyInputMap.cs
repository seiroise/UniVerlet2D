using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniVerlet2D {

	[Serializable]
	public class KeyEventTrigger {
		public string message;
		public KeyCode triggerKeycode;
		public bool useCtrl;
		public bool useShift;
	}

	[Serializable]
	public class KeyInputMap {

		[Serializable]
		public class DetectKeyEvent : UnityEvent<string> { }

		[SerializeField]
		List<KeyEventTrigger> _triggers;
		[SerializeField, ReadOnly]
		bool _isPressedCtrl;
		[SerializeField, ReadOnly]
		bool _isPressedShift;

		[SerializeField]
		DetectKeyEvent _onDetectDown;
		[SerializeField]
		DetectKeyEvent _onDetectUp;

		int _startKeyCode = (int)KeyCode.A;
		int _endKeyCode = (int)KeyCode.Z;

		public void InputCheck() {
			_isPressedCtrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			_isPressedShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

			for(var i = 0; i < _triggers.Count; ++i) {
				if(Input.GetKeyDown(_triggers[i].triggerKeycode)) {
					if((_triggers[i].useCtrl == _isPressedCtrl) && (_triggers[i].useShift == _isPressedShift)) {
						_onDetectDown.Invoke(_triggers[i].message);
					}
				} else if(Input.GetKeyUp(_triggers[i].triggerKeycode)) {
					_onDetectUp.Invoke(_triggers[i].message);
				}
			}
		}
	}
}