using UnityEngine;
using System.Collections.Generic;

namespace UniVerlet2D {

	[System.Serializable]
	public struct MinMax {

		[SerializeField]
		float _min;
		[SerializeField]
		float _max;

		public float min { get { return _min; } set { _min = value; } }
		public float max { get { return _max; } set { _max = value; } }

		public float delta { get { return max - min; } }
		public float absDelta { get { return Mathf.Abs(max - min); } }

		public float random { get { return Random.Range(min, max); } }
		public int randomInt { get { return (int)Random.Range(min, max); } }

		public MinMax(float min, float max) {
			this._min = min;
			this._max = max;
		}

		public float Lerp(float t) {
			return Mathf.Lerp(min, max, t);
		}

		public float Clamp(float value) {
			return Mathf.Clamp(value, min, max);
		}

		public bool CheckRange(float value) {
			return min <= value && value <= max;
		}

		public IEnumerable<float> Step(float step) {
			if(step == 0) {
				throw new System.ArgumentException("stepには0以外を設定してください。");
			}
			float temp = min;
			while(temp <= max) {
				yield return temp += step;
			}
		}

		public IEnumerable<float> DivStep(int div) {
			if(div < 1) {
				throw new System.ArgumentException("divには1より大きい値を設定してください。");
			}
			return Step(delta / (div - 1));
		}
	}
}