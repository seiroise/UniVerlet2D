using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UniVerlet2D.Lab {

	public class MarkerDetector : MonoBehaviour {

		[System.Serializable]
		public class MarkerDetectionEvent : UnityEvent<SimElemMarker> { }

		[System.Serializable]
		public class SpaceClickedEvent : UnityEvent<Vector3> { }

		/*
		 * Fields
		 */

		public EventSystem uiLayer;
		public LayerMask layerMask;
		[ReadOnly]
		public Collider2D _hitCollider;
		[ReadOnly]
		public SimElemMarker _overMarker;

		[Header("Event")]
		public MarkerDetectionEvent onEnterMarker;
		public MarkerDetectionEvent onExitMarker;
		public MarkerDetectionEvent onDownMarker;
		public MarkerDetectionEvent onUpMarker;

		public SpaceClickedEvent onDownSpace;

		Vector3 _wmPos;

		/*
		 * Properties
		 */

		public Vector3 wmPos { get { return _wmPos; } }

		/*
		 * Methods
		 */

		void Update() {
			UpdateSelected();
			HandleMouseInput();
		}

		void UpdateSelected() {
			Vector3 mPos = Input.mousePosition;
			var cam = Camera.main;
			mPos.z = -cam.transform.position.z;
			_wmPos = cam.ScreenToWorldPoint(mPos);
			var hit = Physics2D.Raycast(_wmPos, _wmPos, 0f, layerMask.value, -10f, 10f);
			if(hit.collider != _hitCollider) {
				if(hit.collider) {
					var overMarker = hit.collider.GetComponent<SimElemMarker>();
					if(_overMarker) {
						onExitMarker.Invoke(_overMarker);
					}
					if(overMarker) {
						onEnterMarker.Invoke(overMarker);
					}
					_overMarker = overMarker;
				} else {
					_overMarker = null;
				}
				_hitCollider = hit.collider;
			}
		}

		void HandleMouseInput() {
			if(_overMarker) {
				if(uiLayer && !uiLayer.IsPointerOverGameObject()) {
					if(Input.GetMouseButtonDown(0)) {
						onDownMarker.Invoke(_overMarker);
					} else if(Input.GetMouseButtonUp(0)) {
						onUpMarker.Invoke(_overMarker);
					}
				}
			} else {
				if(Input.GetMouseButtonDown(0) && uiLayer && !uiLayer.IsPointerOverGameObject()) {
					onDownSpace.Invoke(_wmPos);
				}
			}
		}
	}
}