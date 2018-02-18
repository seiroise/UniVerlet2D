using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public interface ICommand {

		string name { get; }
		bool Do();
		void Undo();
	}
}