using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	public class PinConstraint : SimElement, IParticleHolder {

		Particle _a;

		[SerializeField]
		Vector2 _pos;
		[SerializeField]
		int _aUID;

		public Particle a { get { return _a; } }
		public Vector2 pos { get { return _pos; } }

		public PinConstraint(Simulator sim, Particle a, Vector2 pos) : base(sim) {
			_a = a;
			_pos = pos;
		}

		public void Relax(float dt) {
			_a.pos = _pos;
		}

		public override void Step(float dt) {
			_a.pos = _pos;
		}

		protected override void BeforeSerializeToJson() {
			_aUID = _a.uid;
		}

		public override void AfterDeserializeFromJson(Simulator sim) {
			_sim = sim;
			_a = sim.GetParticleByUID(_aUID);
		}

		public bool ContainParticle(Particle p) {
			return _a.uid == p.uid;
		}
	}
}