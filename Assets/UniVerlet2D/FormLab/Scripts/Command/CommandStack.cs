using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniVerlet2D.Lab {

	[System.Serializable]
	public class CommandStack {

		[System.Serializable]
		public class CommandPtrEvent : UnityEvent<int> { }

		/*
		 * Fields
		 */

		List<ICommand> _commands;
		[SerializeField, ReadOnly]
		int _cmdPtr = -1;
		[SerializeField]
		CommandPtrEvent _onChangeCommandPtr;

		/*
		 * Properties
		 */

		public bool canUndo { get { return (0 <= _cmdPtr && _cmdPtr < _commands.Count); } }
		public bool canRedo { get { return (0 <= _cmdPtr + 1 && _cmdPtr + 1 < _commands.Count); } }

		/*
		 * Constructors
		 */

		public CommandStack() {
			_commands = new List<ICommand>();
		}

		/*
		 * Methods
		 */

		void Push(ICommand command) {
			if(_commands.Count - 1 != _cmdPtr) {
				var range = _commands.Count - (_cmdPtr + 1);
				_commands.RemoveRange(_cmdPtr + 1, range);
			}
			_commands.Add(command);
			_cmdPtr++;

			if(_onChangeCommandPtr != null) {
				_onChangeCommandPtr.Invoke(_cmdPtr);
			}
		}

		public void ExeCommand(ICommand command) {
			if(command.Do()) {
				Push(command);
			}
		}

		public void Undo() {
			if(canUndo) {
				_commands[_cmdPtr].Undo();
				if(_onChangeCommandPtr != null) {
					_onChangeCommandPtr.Invoke(_cmdPtr);
				}
				_cmdPtr--;
			}
		}

		public void Redo() {
			if(canRedo) {
				_cmdPtr++;
				if(_onChangeCommandPtr != null) {
					_onChangeCommandPtr.Invoke(_cmdPtr);
				}
				_commands[_cmdPtr].Do();
			}
		}

	}
}