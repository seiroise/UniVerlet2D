using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class DistanceConstraint : SimElement {

		/*
		 * Fields
		 */

		Particle _a, _b;		// a -> b;

		[SerializeField]
		float _relaxLength;
		[SerializeField]
		float _sqrRelaxLength;

		/*
		 * Properties
		 */

		public Particle a { get { return _a; } private set { _a = value; } }
		public Particle b { get { return _b; } private set { _b = value; } }

		public float relaxLength { get { return _relaxLength; } private set { _relaxLength = value; } }
		public float sqrRelaxLength { get { return _sqrRelaxLength; } private set { _sqrRelaxLength = value; } }

		public float currentLength { get { return (_a.pos - _b.pos).magnitude; } }
		public Vector2 middlePos { get { return (_a.pos + _b.pos) * 0.5f; } }
		public float a2bRadian { get { var rad = Mathf.Atan2(_b.pos.y - _a.pos.y, _b.pos.x - _a.pos.x); return float.IsNaN(rad) ? 0f : rad; } }

		/*
		 * Constructor
		 */

		public DistanceConstraint(Particle a, Particle b) {
			this.a = a;
			this.b = b;
			this.relaxLength = (a.pos - b.pos).magnitude;
			this.sqrRelaxLength = relaxLength * relaxLength;
		}

		/*
		 * Methods
		 */

		public override void Step(float dt) {
			var direction = (b.pos - a.pos).normalized;
			b.pos = a.pos + direction * relaxLength;
		}

		public bool ContainParticle(Particle p) {
			return a.uid == p.uid || b.uid == p.uid;
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