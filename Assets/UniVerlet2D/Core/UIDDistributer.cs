using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class UIDDistributer {

		int _counter;

		public int current { get { return _counter; } }
		public int next { get { return _counter++; } }

		public void SetCounter(int count) {
			_counter = count;
		}
	}
}