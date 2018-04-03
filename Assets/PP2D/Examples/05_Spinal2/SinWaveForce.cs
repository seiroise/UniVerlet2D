using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public class SinWaveForce : SimElement {

		public Particle a { get; private set; }
		public Vector2 direction { get; set; }
		public float scale { get; set; }
		public float timeScale { get; set; }

		float time { get; set; }

		public SinWaveForce(Particle a, Vector2 direction, float scale, float timeScale, float timeOffset) {
			this.a = a;
			this.direction = direction;
			this.scale = scale;
			this.timeScale = timeScale;
			time = timeOffset;
		}

		public override void Step(float dt) {
			a.pos += Mathf.Sin(time) * direction * scale;
			time += dt * timeScale;
		}

		public override Matrix4x4 GetMatrix() {
			throw new System.NotImplementedException();
		}
	}
}