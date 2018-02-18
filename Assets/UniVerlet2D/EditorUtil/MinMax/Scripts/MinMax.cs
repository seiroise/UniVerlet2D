using UnityEngine;
using System.Collections.Generic;

namespace UniVerlet2D {

	[System.Serializable]
	public struct MinMax {

		[SerializeField]
		private float _min;
		public float min { get { return _min; } set { _min = value; } }

		[SerializeField]
		private float _max;
		public float max { get { return _max; } set { _min = value; } }

		public float random { get { return Random.Range(min, max); } }
		public int randomInt { get { return (int)Random.Range(min, max); } }

		public MinMax(float min, float max) {
			this._min = min;
			this._max = max;
		}

		public float Lerp(float t) {
			return Mathf.Lerp(_min, _max, t);
		}

		public float Clamp(float value) {
			return Mathf.Clamp(value, min, max);
		}

		public bool CheckRange(float value) {
			return _min <= value && value <= _max;
		}
	}
}