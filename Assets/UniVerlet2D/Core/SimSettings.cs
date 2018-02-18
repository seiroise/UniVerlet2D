using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	[System.Serializable]
	[CreateAssetMenu(fileName = "SimSettings", menuName = "UniVerlet2D/SimSettings")]
	public class SimSettings : ScriptableObject {

		/*
		 * Fields
		 */

		public bool applySpring = true;
		public bool applyAngle = true;

		[SerializeField, Range(1, 10)]
		public int _iteration = 1;

		[SerializeField, Header("Constants")]
		Vector2 _gravity = Vector2.zero;

		[SerializeField, Range(0.01f, 0.99f)]
		float _damping = 0.9f;

		[SerializeField, Range(0.01f, 10f)]
		float _springConstant = 0.9f;

		[SerializeField, Range(0.01f, 10f)]
		float _angleConstant = 0.1f;

		/*
		 * Properties
		 */

		public int iteration { get { return _iteration; } set { _iteration = Mathf.Clamp(1, 10, value); } }
		public Vector2 gravity { get { return _gravity; } set { _gravity = value; } }

		public float damping { get { return _damping; } set { _damping = Mathf.Clamp(value, 0.01f, 0.99f); } }
		public float springConstant { get { return _springConstant; } set { _springConstant = Mathf.Clamp(value, 0.01f, 10f); } }
		public float angleConstant { get { return _angleConstant; } set { _angleConstant = Mathf.Clamp(value, 0.01f, 10f); } }
	}
}