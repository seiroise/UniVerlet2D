using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class Particle : SimElement {

		[SerializeField]
		Vector2 _pos;
		Vector2 _oldPos;

		float _damping = 0.9f;

		public Vector2 pos { get { return _pos; } set { _pos = value; } }
		public Vector2 oldPos { get { return _oldPos; } set { _oldPos = value; } }

		public Particle() : base() { }

		public Particle(Vector2 pos, float damping = 0.9f) {
			this._oldPos = this._pos = pos;
		}

		public override void Step(float dt) {
			var velocity = (_pos - _oldPos) * _damping;
			_oldPos = _pos;
			_pos += velocity;
		}

		public override Matrix4x4 GetMatrix() {
			return Matrix4x4.TRS(_pos, Quaternion.identity, Vector3.one);
		}
	}
}