using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class SpringConstraint : SimElement {

		/*
		 * Fields
		 */

		Particle _a, _b;

		[SerializeField]
		float _stiffness;
		[SerializeField]
		float _relaxLength;
		[SerializeField]
		float _sqrRelaxLength;

		/*
		 * Properties
		 */

		public Particle a { get { return _a; } }
		public Particle b { get { return _b; } }

		public float stiffness { get { return _stiffness; } set { _stiffness = value; } }

		public float relaxLength { get { return _relaxLength; } }

		public float currentLength { get { return (_a.pos - _b.pos).magnitude; } }
		public Vector2 middlePos { get { return (_a.pos + _b.pos) * 0.5f; } }
		public float a2bRadian { get { var rad = Mathf.Atan2(_b.pos.y - _a.pos.y, _b.pos.x - _a.pos.x); return float.IsNaN(rad) ? 0f : rad; } }

		/*
		 * Constructor
		 */

		public SpringConstraint(Particle a, Particle b, float stiffness = 1f) {
			_a = a;
			_b = b;
			_stiffness = stiffness;
			_relaxLength = (a.pos - b.pos).magnitude;
			_sqrRelaxLength = (a.pos - b.pos).sqrMagnitude;
		}

		/*
		 * Methods
		 */

		public override void Step(float dt) {
			var diff = _a.pos - _b.pos;
			var sqrLength = diff.sqrMagnitude;
			diff *= ((_sqrRelaxLength - sqrLength) / sqrLength) * _stiffness * dt;
			_a.pos += diff;
			_b.pos -= diff;
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid || _b.uid == p.uid;
		}

		public override Matrix4x4 GetMatrix() {
			return Matrix4x4.TRS(
				middlePos, 
				Quaternion.AngleAxis(a2bRadian * Mathf.Rad2Deg, Vector3.forward), 
				new Vector3(currentLength, 1f, 1f)
			);
		}
	}
}