using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D {

	[System.Serializable]
	public class Simulator {

		/*
		 * Fields
		 */

		[SerializeField]
		SimSettings _settings;

		// particle
		[HideInInspector, SerializeField]
		List<Particle> _particles;

		// constraint
		[HideInInspector, SerializeField]
		List<SpringConstraint> _springs;
		[HideInInspector, SerializeField]
		List<AngleConstraint> _angles;
		[HideInInspector, SerializeField]
		List<PinConstraint> _pins;

		// interactions
		[HideInInspector, SerializeField]
		List<StretchInteraction> _stretchs;
		[HideInInspector, SerializeField]

		UIDDistributer _uidDistributer;

		[SerializeField, HideInInspector]
		Data.Form _serializedForm;

		/*
		 * Properties
		 */

		public SimSettings settings { get { return _settings; } set { _settings = value; } }

		public int numOfParticles { get { return _particles != null ? _particles.Count : 0; } }

		public int numOfSprings { get { return _springs != null ? _springs.Count : 0; } }
		public int numOfAngles { get { return _angles != null ? _angles.Count : 0; } }
		public int numOfPins { get { return _pins != null ? _pins.Count : 0; } }

		public int numOfStretchs { get { return _stretchs != null ? _stretchs.Count : 0; } }

		public UIDDistributer uidDistributer { get { return _uidDistributer; } }

		public Form serializedForm { get { return _serializedForm; } }

		/*
		 * Methods
		 */

		public void Init(bool withClear = true) {
			if(!_settings) {
				_settings = new SimSettings();
			}
			if(withClear) {
				Clear();
			}
			_uidDistributer = new UIDDistributer();
		}

		public void Update(float dt) {
			// particles
			for(var i = 0; i < numOfParticles; ++i) {
				var p = _particles[i];
				p.Tick(dt);
				/*
				var velocity = (p.pos - p.oldPos) * _settings.damping;
				p.oldPos = p.pos;
				p.pos += _settings.gravity * dt;
				p.pos += velocity;
				*/
			}

			// interact
			for(var i = 0; i < numOfStretchs; ++i) {
				_stretchs[i].Apply(dt);
			}

			// relax
			for(var i = 0; i < numOfSprings; ++i) {
				_springs[i].Step(dt);
			}
			for(var i = 0; i < numOfAngles; ++i) {
				_angles[i].Relax(dt);
			}
			for(var i = 0; i < numOfPins; ++i) {
				_pins[i].Relax(dt);
			}
		}

		public void Clear() {
			// Debug.Log("Clear");
			if(_particles == null) {
				_particles = new List<Particle>();
			} else {
				_particles.Clear();
			}

			if(_springs == null) {
				_springs = new List<SpringConstraint>();
			} else {
				_springs.Clear();
			}

			if(_angles == null) {
				_angles = new List<AngleConstraint>();
			} else {
				_angles.Clear();
			}

			if(_pins == null) {
				_pins = new List<PinConstraint>();
			} else {
				_pins.Clear();
			}

			if(_stretchs == null) {
				_stretchs = new List<StretchInteraction>();
			} else {
				_stretchs.Clear();
			}

			if(_uidDistributer == null) {
				_uidDistributer = new UIDDistributer();
			} else {
				_uidDistributer.SetCounter(0);
			}
		}

		public BoundingCircle GetBoundingCircle(float particleRadius) {
			if(numOfParticles <= 1) {
				if(numOfParticles <= 0) {
					return BoundingCircle.zero;
				} else {
					return new BoundingCircle(GetParticleAt(0).pos, particleRadius);
				}
			}
			float xMin, xMax, yMin, yMax;
			Vector2 temp = GetParticleAt(0).pos;
			xMin = xMax = temp.x;
			yMin = yMax = temp.y;

			for(var i = 1; i < numOfParticles; ++i) {
				temp = GetParticleAt(i).pos;
				if(xMin > temp.x) {
					xMin = temp.x;
				} else if(xMax < temp.x) {
					xMax = temp.x;
				}
				if(yMin > temp.y) {
					yMin = temp.y;
				} else if(yMax < temp.y) {
					yMax = temp.y;
				}
			}
			float diameter = particleRadius * 2f;
			float dx = xMax - xMin + diameter;
			float dy = yMax - yMin + diameter;

			return new BoundingCircle(
				new Vector2((xMin + xMax) * 0.5f, (yMin + yMax) * 0.5f),
				Mathf.Sqrt(dx * dx + dy * dy) * 0.5f
			);
		}

		/*
		 * Function and method of particle
		 */

		public Particle MakeParticle(Vector2 pos) {
			var p = new Particle(this, pos);
			_particles.Add(p);
			return p;
		}

		public void DeleteParticle(Particle p) {
			DeleteParticleByUID(p.uid);
		}

		public void DeleteParticleByUID(int uid) {
			for(var i = 0; i < numOfParticles; ++i) {
				var p = GetParticleAt(i);
				if(p.uid == uid) {
					DeleteParticleAt(i);
					return;
				}
			}
		}

		public void DeleteParticleAt(int idx) {
			var p = GetParticleAt(idx);
			_particles.RemoveAt(idx);
			DeleteParticleRelated(p);
		}

		public void DeleteParticle(
			Particle p, ref List<SpringConstraint> relatedSprings,
			ref List<AngleConstraint> relatedAngles, ref List<PinConstraint> relatedPins
		) {
			for(var i = 0; i < numOfParticles; ++i) {
				var pp = GetParticleAt(i);
				if(pp.uid == p.uid) {
					DeleteParticleRelated(p, ref relatedSprings, ref relatedAngles, ref relatedPins);
					_particles.RemoveAt(i);
					return;
				}
			}
		}

		void DeleteParticleRelated(Particle p) {
			for(var i = numOfSprings - 1; i >= 0; --i) {
				var s = GetSpringAt(i);
				if(s.ContainParticle(p)) {
					DeleteSpringAt(i);
				}
			}
			for(var i = numOfAngles - 1; i >= 0; --i) {
				var s = GetAngleAt(i);
				if(s.ContainParticle(p)) {
					DeleteAngleAt(i);
				}
			}
			for(var i = numOfPins - 1; i >= 0; --i) {
				var s = GetPinAt(i);
				if(s.ContainParticle(p)) {
					DeletePinAt(i);
				}
			}
		}

		void DeleteParticleRelated(
			Particle p, ref List<SpringConstraint> relatedSprings,
			ref List<AngleConstraint> relatedAngles, ref List<PinConstraint> relatedPins
		) {
			for(var i = numOfSprings - 1; i >= 0; --i) {
				var s = GetSpringAt(i);
				if(s.ContainParticle(p)) {
					relatedSprings.Add(s);
					DeleteSpringAt(i);
				}
			}
			for(var i = numOfAngles - 1; i >= 0; --i) {
				var a = GetAngleAt(i);
				if(a.ContainParticle(p)) {
					relatedAngles.Add(a);
					DeleteAngleAt(i);
				}
			}
			for(var i = numOfPins - 1; i >= 0; --i) {
				var pi = GetPinAt(i);
				if(pi.ContainParticle(p)) {
					relatedPins.Add(pi);
					DeletePinAt(i);
				}
			}
		}

		public Particle GetParticleAt(int idx) {
			return _particles[idx];
		}

		public Particle GetParticleByUID(int uid) {
			for(var i = 0; i < numOfParticles; ++i) {
				var p = GetParticleAt(i);
				if(p.uid == uid) {
					return p;
				}
			}
			return null;
		}

		public void ClearParticles() {
			for(var i = numOfParticles - 1; i >= 0; --i) {
				DeleteParticleAt(i);
			}
			_particles.Clear();
		}

		/*
		 * Spring Functions
		 */

		public SpringConstraint MakeSpring(Particle a, Particle b, float stiffness = 1f) {
			var s = new SpringConstraint(this, a, b, stiffness);
			_springs.Add(s);
			return s;
		}

		public SpringConstraint MakeSpringByIdx(int aIdx, int bIdx, float stiffness = 1f) {
			var a = GetParticleAt(aIdx);
			var b = GetParticleAt(bIdx);
			return MakeSpring(a, b, stiffness);
		}

		public SpringConstraint MakeSpringByUID(int aUID, int bUID, float stiffness = 1f) {
			var a = GetParticleByUID(aUID);
			var b = GetParticleByUID(bUID);
			return MakeSpring(a, b, stiffness);
		}

		public void DeleteSpring(SpringConstraint s) {
			DeleteSpringByUID(s.uid);
		}

		public void DeleteSpringByUID(int uid) {
			for(var i = 0; i < numOfSprings; ++i) {
				var s = GetSpringAt(i);
				if(s.uid == uid) {
					DeleteSpringAt(i);
					return;
				}
			}
		}

		public void DeleteSpringAt(int idx) {
			_springs.RemoveAt(idx);
		}

		public SpringConstraint GetSpringAt(int idx) {
			return _springs[idx];
		}

		public SpringConstraint GetSpringByUID(int uid) {
			for(var i = 0; i < numOfSprings; ++i) {
				var s = GetSpringAt(i);
				if(s.uid == uid) {
					return s;
				}
			}
			return null;
		}

		public void ClearSprings() {
			_springs.Clear();
		}

		/*
		 * Angle Functions
		 */

		public AngleConstraint MakeAngle(Particle a, Particle b, Particle m, float stiffness = 1f) {
			var an = new AngleConstraint(this, a, b, m, stiffness);
			_angles.Add(an);
			return an;
		}

		public AngleConstraint MakeAngleByIdx(int aIdx, int bIdx, int mIdx, float stiffness = 1f) {
			// Debug.Log(aIdx + "-" + bIdx + "-" + mIdx);
			var a = GetParticleAt(aIdx);
			var b = GetParticleAt(bIdx);
			var m = GetParticleAt(mIdx);

			return MakeAngle(a, b, m, stiffness);
		}

		public AngleConstraint MakeAngleByUID(int aUID, int bUID, int mUID, float stiffness = 1f) {
			var a = GetParticleByUID(aUID);
			var b = GetParticleByUID(bUID);
			var m = GetParticleByUID(mUID);

			return MakeAngle(a, b, m, stiffness);
		}

		public void DeleteAngle(AngleConstraint a) {
			DeleteAngleByUID(a.uid);
		}

		public void DeleteAngleByUID(int uid) {
			for(var i = 0; i < numOfAngles; ++i) {
				var a = GetAngleAt(i);
				if(a.uid == uid) {
					DeleteAngleAt(i);
				}
			}
		}

		public void DeleteAngleAt(int idx) {
			_angles.RemoveAt(idx);
		}

		public AngleConstraint GetAngleAt(int idx) {
			return _angles[idx];
		}

		public AngleConstraint GetAngleByUID(int uid) {
			for(var i = 0; i < numOfAngles; ++i) {
				var a = GetAngleAt(i);
				if(a.uid == uid) {
					return a;
				}
			}
			return null;
		}

		public void ClearAngles() {
			_angles.Clear();
		}

		/*
		 * Pin related
		 */

		public PinConstraint MakePin(Particle p) {
			return MakePin(p, p.pos);
		}

		public PinConstraint MakePin(Particle p, Vector2 pinnedPos) {
			var pin = new PinConstraint(this, p, pinnedPos);
			_pins.Add(pin);
			return pin;
		}

		public PinConstraint MakePinByIdx(int idx, Vector2 pinnedPos) {
			var p = GetParticleAt(idx);
			return MakePin(p, pinnedPos);
		}

		public PinConstraint MakePinByUID(int uid, Vector2 pinnedPos) {
			var p = GetParticleByUID(uid);
			return MakePin(p, pinnedPos);
		}

		public void DeletePin(PinConstraint p) {
			DeletePinByUID(p.uid);
		}

		public void DeletePinByUID(int uid) {
			for(var i = 0; i < numOfPins; ++i) {
				var p = GetPinAt(i);
				if(p.uid == uid) {
					DeletePinAt(i);
				}
			}
		}

		public void DeletePinAt(int idx) {
			_pins.RemoveAt(idx);
		}

		public PinConstraint GetPinAt(int idx) {
			return _pins[idx];
		}

		public void ClearPins() {
			_pins.Clear();
		}

		/*
		 * Stretch related
		 */

		public StretchInteraction MakeStretch(Particle a, Particle b) {
			var stretch = new StretchInteraction(this, a, b);
			_stretchs.Add(stretch);
			return stretch;
		}

		public StretchInteraction MakeStretchByIdx(int aIdx, int bIdx) {
			var a = GetParticleAt(aIdx);
			var b = GetParticleAt(bIdx);
			return MakeStretch(a, b);
		}

		public StretchInteraction MakeStretchByUID(int aUID, int bUID) {
			var a = GetParticleByUID(aUID);
			var b = GetParticleByUID(bUID);
			return MakeStretch(a, b);
		}

		public void DeleteStretchAt(int idx) {
			_stretchs.RemoveAt(idx);
		}

		public void ClearStretchs() {
			_stretchs.Clear();
		}

		public StretchInteraction GetStretchAt(int idx) {
			return _stretchs[idx];
		}

		/*
		 * Utilities
		 */

		public bool GetOverlapParticle(Vector2 pos, float particleRadius, out int idx) {
			float sqrRadius = particleRadius * particleRadius;
			for(var i = 0; i < numOfParticles; ++i) {
				var p = GetParticleAt(i);
				if((pos - p.pos).sqrMagnitude <= sqrRadius) {
					idx = i;
					return true;
				}
			}
			idx = -1;
			return false;
		}

		/*
		 * Form related
		 */

		public Form ExportForm() {
			var form = new Form();

			var uid2idx = new Dictionary<int, int>();

			for(var i = 0; i < numOfParticles; ++i) {
				var p = GetParticleAt(i);
				form.particles.Add(p.pos);
				uid2idx.Add(p.uid, i);
			}
			for(var i = 0; i < numOfSprings; ++i) {
				var s = GetSpringAt(i);
				form.springs.Add(new Form.SpringInfo(uid2idx[s.a.uid], uid2idx[s.b.uid], s.stiffness));
			}
			for(var i = 0; i < numOfAngles; ++i) {
				var a = GetAngleAt(i);
				form.angles.Add(new Form.AngleInfo(uid2idx[a.a.uid], uid2idx[a.b.uid], uid2idx[a.m.uid], a.stiffness));
			}
			for(var i = 0; i < numOfPins; ++i) {
				var p = GetPinAt(i);
				form.pins.Add(new Form.PinInfo(uid2idx[p.a.uid], p.pos));
			}

			for(var i = 0; i < numOfStretchs; ++i) {
				var s = GetStretchAt(i);
				// form.pins.Add(new Form.StretchInfo(uid2idx[s.a.uid], uid2idx[s.b.uid], s.power));
			}
			return form;
		}

		public void ImportForm(Form form) {
			if(form == null) {
				return;
			}
			Clear();
			for(var i = 0; i < form.particles.Count; ++i) {
				MakeParticle(form.particles[i]);
			}

			for(var i = 0; i < form.springs.Count; ++i) {
				var s = form.springs[i];
				MakeSpringByIdx(s.a, s.b, s.stiffness);
			}
			for(var i = 0; i < form.angles.Count; ++i) {
				var a = form.angles[i];
				MakeAngleByIdx(a.a, a.b, a.c, a.stiffness);
			}
			for(var i = 0; i < form.pins.Count; ++i) {
				var p = form.pins[i];
				MakePinByIdx(p.idx, p.pos);
			}
		}

		public RelatedForm ExportRelatedForm() {
			var rForm = new RelatedForm();
			rForm.counter = _uidDistributer.current;
			for(var i = 0; i < numOfParticles; ++i) {
				var p = GetParticleAt(i);
				rForm.particles.Add(new RelatedForm.ParticleInfo(p.pos, p.uid));
			}
			for(var i = 0; i < numOfSprings; ++i) {
				var s = GetSpringAt(i);
				rForm.springs.Add(new RelatedForm.SpringInfo(s.uid, s.a.uid, s.b.uid, s.stiffness));
			}
			for(var i = 0; i < numOfAngles; ++i) {
				var a = GetAngleAt(i);
				rForm.angles.Add(new RelatedForm.AngleInfo(a.uid, a.a.uid, a.b.uid, a.m.uid, a.stiffness));
			}
			for(var i = 0; i < numOfPins; ++i) {
				var p = GetPinAt(i);
				rForm.pins.Add(new RelatedForm.PinInfo(p.uid, p.a.uid, p.pos));
			}

			return rForm;
		}

		public void ImportRelatedForm(RelatedForm rForm) {
			if(rForm == null) {
				return;
			}
			Clear();
			for(var i = 0; i < rForm.particles.Count; ++i) {
				var pi = rForm.particles[i];
				var p = MakeParticle(pi.pos);
				p.OverrideUID(pi.uid);
			}
			for(var i = 0; i < rForm.springs.Count; ++i) {
				var si = rForm.springs[i];
				var s = MakeSpringByUID(si.a, si.b, si.stiffness);
				s.OverrideUID(si.uid);
			}
			for(var i = 0; i < rForm.angles.Count; ++i) {
				var ai = rForm.angles[i];
				var s = MakeAngleByUID(ai.a, ai.b, ai.m, ai.stiffness);
				s.OverrideUID(ai.uid);
			}
			for(var i = 0; i < rForm.pins.Count; ++i) {
				var pi = rForm.pins[i];
				var p = MakePinByUID(pi.a, pi.pos);
				p.OverrideUID(pi.uid);
			}
		}

		public FormText ExportFormText() {
			FormText formText = new FormText();
			var sb = new System.Text.StringBuilder();
			for(var i = 0; i < numOfParticles; ++i) {
				var e = GetParticleAt(i);
				sb.AppendLine(string.Format("p {0}", e.ExportJson()));
			}
			sb.AppendLine();
			for(var i = 0; i < numOfSprings; ++i) {
				var e = GetSpringAt(i);
				sb.AppendLine(string.Format("sc {0}", e.ExportJson()));
			}
			sb.AppendLine();
			for(var i = 0; i < numOfAngles; ++i) {
				var e = GetAngleAt(i);
				sb.AppendLine(string.Format("ac {0}", e.ExportJson()));
			}
			sb.AppendLine();
			for(var i = 0; i < numOfPins; ++i) {
				var e = GetPinAt(i);
				sb.AppendLine(string.Format("pc {0}", e.ExportJson()));
			}

			formText.text = sb.ToString();
			return formText;
		}
	}
}