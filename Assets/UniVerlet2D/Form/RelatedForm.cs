using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Data {

	[System.Serializable]
	public class RelatedForm {

		[System.Serializable]
		public class ParticleInfo {
			public Vector2 pos;
			public int uid;

			public ParticleInfo(Vector2 pos, int uid) {
				this.pos = pos;
				this.uid = uid;
			}
		}

		[System.Serializable]
		public class SpringInfo {
			public int uid;
			public int a, b;
			public float stiffness;

			public SpringInfo(int uid, int a, int b, float stiffness) {
				this.uid = uid;
				this.a = a;
				this.b = b;
				this.stiffness = stiffness;
			}
		}

		[System.Serializable]
		public class AngleInfo {
			public int uid;
			public int a, b, m;
			public float stiffness;

			public AngleInfo(int uid, int a, int b, int m, float stiffness) {
				this.uid = uid;
				this.a = a;
				this.b = b;
				this.m = m;
				this.stiffness = stiffness;
			}
		}

		[System.Serializable]
		public class PinInfo {
			public int uid;
			public int a;
			public Vector2 pos;

			public PinInfo(int uid, int a, Vector2 pos) {
				this.uid = uid;
				this.a = a;
				this.pos = pos;
			}
		}

		public int counter;

		public List<ParticleInfo> particles;
		public List<SpringInfo> springs;
		public List<AngleInfo> angles;
		public List<PinInfo> pins;

		public RelatedForm() {
			particles = new List<ParticleInfo>();
			springs = new List<SpringInfo>();
			angles = new List<AngleInfo>();
			pins = new List<PinInfo>();
		}
	}
}