using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UniVerlet2D.Data {

	[System.Serializable]
	public class Form {

		[System.Serializable]
		public struct SpringInfo {
			public int a, b;
			public float stiffness;

			public SpringInfo(int a, int b, float stiffness) {
				this.a = a;
				this.b = b;
				this.stiffness = stiffness;
			}
		}

		[System.Serializable]
		public struct AngleInfo {
			public int a, b, c;
			public float stiffness;

			public AngleInfo(int a, int b, int c, float stiffness) {
				this.a = a;
				this.b = b;
				this.c = c;
				this.stiffness = stiffness;
			}
		}

		[System.Serializable]
		public struct PinInfo {
			public int idx;
			public Vector2 pos;

			public PinInfo(int idx, Vector2 pos) {
				this.idx = idx;
				this.pos = pos;
			}
		}

		[System.Serializable]
		public struct StretchInfo {
			public int a, b;
			public float power;

			public StretchInfo(int a, int b, float power) {
				this.a = a;
				this.b = b;
				this.power = power;
			}
		}

		public const int VERSION = 2;
		public const string EXTENSION = ".frm";

		public List<Vector2> particles;

		public List<SpringInfo> springs;
		public List<AngleInfo> angles;
		public List<PinInfo> pins;

		public List<StretchInfo> stretchs;

		public Form() {
			particles = new List<Vector2>();
			springs = new List<SpringInfo>();
			angles = new List<AngleInfo>();
			pins = new List<PinInfo>();
		}

		public string GetFormattedText() {
			var sb = new StringBuilder();

			sb.AppendLine(string.Format("v {0}", VERSION));

			sb.AppendLine();

			for(var i = 0; i < particles.Count; ++i) {
				var p = particles[i];
				sb.AppendLine(string.Format("p {0} {1}", p.x, p.y));
			}
			for(var i = 0; i < springs.Count; ++i) {
				var s = springs[i];
				sb.AppendLine(string.Format("s {0} {1} {2}", s.a, s.b, s.stiffness));
			}
			for(var i = 0; i < angles.Count; ++i) {
				var a = angles[i];
				sb.AppendLine(string.Format("a {0} {1} {2} {3} ", a.a, a.b, a.c, a.stiffness));
			}
			for(var i = 0; i < pins.Count; ++i) {
				var p = pins[i];
				sb.AppendLine(string.Format("pi {0} {1} {2}", p.idx, p.pos.x, p.pos.y));
			}
			for(var i = 0; i < stretchs.Count; ++i) {
				var s = stretchs[i];
				sb.AppendLine(string.Format("si {0} {1} {2}", s.a, s.b, s.power));
			}
			return sb.ToString();
		}

		public static Form MakeFromFormattedText(string data) {
			var form = new Form();
			var lines = data.Split('\n');

			float x, y;
			int a, b, c;
			float f;

			for(var i = 0; i < lines.Length; ++i) {
				var s = lines[i].Split(' ');
				if(s.Length > 0) {
					switch(s[0]) {
					case "p":
						if(float.TryParse(s[1], out x) && float.TryParse(s[2], out y)) {
							form.particles.Add(new Vector2(x, y));
						}
						break;
					case "s":
						if(int.TryParse(s[1], out a) && int.TryParse(s[2], out b) && float.TryParse(s[3], out f)) {
							form.springs.Add(new SpringInfo(a, b, f));
						}
						break;
					case "a":
						if(int.TryParse(s[1], out a) && int.TryParse(s[2], out b) && int.TryParse(s[3], out c) && float.TryParse(s[4], out f)) {
							form.angles.Add(new AngleInfo(a, b, c, f));
						}
						break;
					case "pi":
						if(int.TryParse(s[1], out a) && float.TryParse(s[2], out x) && float.TryParse(s[3], out y)) {
							form.pins.Add(new PinInfo(a, new Vector2(x, y)));
						}
						break;
					case "si":
						if(int.TryParse(s[1], out a) && int.TryParse(s[2], out b) && float.TryParse(s[3], out f)) {
							form.stretchs.Add(new StretchInfo(a, b, f));
						}
						break;
					}
				}
			}
			return form;
		}
	}
}