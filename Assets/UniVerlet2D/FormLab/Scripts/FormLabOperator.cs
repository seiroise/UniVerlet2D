using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class FormLabOperator {

		CommandStack _stack;

		public FormLabOperator() {
			_stack = new CommandStack();
		}

		public void Undo() {
			_stack.Undo();
		}

		public void Redo() {
			_stack.Redo();
		}

		public void MakeParticle(Simulator sim, MarkerManager marker, Vector2 pos) {
			_stack.ExeCommand(new MakeParticleCommand(sim, marker, pos));
		}

		public void DeleteParticle(Simulator sim, MarkerManager marker, int uid) {
			_stack.ExeCommand(new DeleteParticleCommand(sim, marker, uid));
		}

		public void MakeSpring(Simulator sim, MarkerManager marker, int aUID, int bUID, float stiffness) {
			_stack.ExeCommand(new MakeSpringCommand(sim, marker, aUID, bUID, stiffness));
		}

		public void DeleteSpring(Simulator sim, MarkerManager marker, int uid) {
			_stack.ExeCommand(new DeleteSpringCommand(sim, marker, uid));
		}

		public void MakeAngle(Simulator sim, MarkerManager marker, int aUID, int bUID, int mUID, float stiffness) {
			_stack.ExeCommand(new MakeAngleCommand(sim, marker, aUID, bUID, mUID, stiffness));
		}

		public void DeleteAngle(Simulator sim, MarkerManager marker, int uid) {
			_stack.ExeCommand(new DeleteAngleCommand(sim, marker, uid));
		}
	}
}