using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class SpringConstraint : SimElement, IParticleHolder {

		/*
		 * Fields
		 */

		Particle _a, _b;

		[SerializeField]
		float _stiffness;
		[SerializeField]
		float _length;
		[SerializeField]
		float _sqrLength;

		/*
		 * Properties
		 */

		public Particle a { get { return _a; } }
		public Particle b { get { return _b; } }

		public float stiffness { get { return _stiffness; } set { _stiffness = value; } }

		public float length { get { return _length; } }

		public float currentLength { get { return (_a.pos - _b.pos).magnitude; } }
		public Vector2 middlePos { get { return (_a.pos + _b.pos) * 0.5f; } }
		public float a2bRadian { get { var rad = Mathf.Atan2(_b.pos.y - _a.pos.y, _b.pos.x - _a.pos.x); return float.IsNaN(rad) ? 0f : rad; } }
		public Matrix4x4 worldMatrix { get { return Matrix4x4.TRS(middlePos, Quaternion.AngleAxis(a2bRadian * Mathf.Rad2Deg, Vector3.forward), new Vector3(currentLength, 1f, 1f)); } }

		/*
		 * Constructor
		 */

		public SpringConstraint(Particle a, Particle b, float stiffness = 1f) {
			_a = a;
			_b = b;
			_stiffness = stiffness;
			_length = (a.pos - b.pos).magnitude;
			_sqrLength = (a.pos - b.pos).sqrMagnitude;
		}

		/*
		 * Methods
		 */

		public override void Step(float dt) {
			var normal = _a.pos - _b.pos;
			var sqrLength = normal.sqrMagnitude;
			normal *= ((_sqrLength - sqrLength) / sqrLength) * _stiffness * dt;
			_a.pos += normal;
			_b.pos -= normal;
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid || _b.uid == p.uid;
		}

		public override Matrix4x4 GetMatrix() {
			return worldMatrix;
		}
	}
}