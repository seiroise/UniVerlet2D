using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class AngleConstraint : SimElement {

		/*
		 * Fields
		 */

		static readonly float TWO_PI = Mathf.PI + Mathf.PI;
		static readonly float MAX_ANGLE = Mathf.PI - 0.1f;

		Particle _a, _b, _m;

		[SerializeField]
		float _stiffness;
		[SerializeField]
		float _angle;

		/*
		 * Properties
		 */

		public Particle a { get { return _a; } }
		public Particle b { get { return _b; } }
		public Particle m { get { return _m; } }

		public float aAngle { get { var d = _a.pos - _m.pos; return Mathf.Atan2(d.y, d.x); } }
		public float bAngle { get { var d = _b.pos - _m.pos; return Mathf.Atan2(d.y, d.x); } }

		public float stiffness { get { return _stiffness; } set { _stiffness = value; } }

		/*
		 * Constructors
		 */

		public AngleConstraint(Particle a, Particle b, Particle m, float stiffness = 1f) {
			_a = a;
			_b = b;
			_m = m;

			_stiffness = stiffness;
			_angle = DeltaAngle(a.pos, b.pos, m.pos);
		}

		/*
		 * Functions
		 */

		float DeltaAngle(Vector2 a, Vector2 b, Vector2 m) {
			float angle1 = Mathf.Atan2(a.y - m.y, a.x - m.x);
			float angle2 = Mathf.Atan2(b.y - m.y, b.x - m.x);

			return DeltaAngle(angle1, angle2);
		}

		float DeltaAngle(float from, float to) {
			float delta = to - from;
			if(delta > Mathf.PI) {
				return delta - TWO_PI;
			} else if(delta < -Mathf.PI) {
				return delta + TWO_PI;
			}
			return delta;
		}

		Vector2 Rotate(Vector2 point, Vector2 axis, float angle) {
			float angleCos = Mathf.Cos(angle);
			float angleSin = Mathf.Sin(angle);
			float dx = point.x - axis.x;
			float dy = point.y - axis.y;
			return new Vector2(
				angleCos * dx - angleSin * dy + axis.x,
				angleSin * dx + angleCos * dy + axis.y);
		}

		public override void Step(float dt) {
			var angle = DeltaAngle(_a.pos, _b.pos, _m.pos);
			var diff = DeltaAngle(_angle, angle);
			diff = diff * dt * _stiffness;
			if(diff > MAX_ANGLE) {
				diff = MAX_ANGLE;
			} else if(diff < -MAX_ANGLE) {
				diff = -MAX_ANGLE;
			}

			_a.pos = Rotate(_a.pos, _m.pos, diff);
			_b.pos = Rotate(_b.pos, _m.pos, -diff);

			_m.pos = Rotate(_m.pos, _a.pos, diff);
			_m.pos = Rotate(_m.pos, _b.pos, -diff);
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid || _b.uid == p.uid || _m.uid == p.uid;
		}

		public override Matrix4x4 GetMatrix() {
			return _m.GetMatrix();
		}
	}
}