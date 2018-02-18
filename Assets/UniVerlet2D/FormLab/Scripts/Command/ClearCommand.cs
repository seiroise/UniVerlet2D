using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class ClearCommand : ICommand {

		Simulator _sim;
		MarkerManager _marker;


		public string name { get { return "Clear"; } }

		public bool Do() {
			throw new System.NotImplementedException();
		}

		public void Undo() {
			throw new System.NotImplementedException();
		}
	}
}