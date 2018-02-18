using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D.Examples {

	[RequireComponent(typeof(MonoSimulator), typeof(SimRenderer))]
	public class MonoSimTree : MonoBehaviour {

		[SerializeField]
		TreeFormBuilder _treeFormBuilder;

		MonoSimulator _monoSim;
		SimRenderer _simRenderer;

		public MonoSimulator monoSim { get { return _monoSim == null ? _monoSim = GetComponent<MonoSimulator>() : _monoSim; } }

		void Awake() {
			_monoSim = GetComponent<MonoSimulator>();
			_simRenderer = GetComponent<SimRenderer>();
		}

		public void BuildForm() {
			var monoSim = this.monoSim;
			monoSim.startWithClear = false;

			var sim = monoSim.sim;
			sim.ImportForm(_treeFormBuilder.Build(Vector2.zero));
			var root = sim.GetParticleAt(0);
			sim.MakePin(root);
			var branch = sim.GetParticleAt(1);
			sim.MakePin(branch);

			var tInfo = _treeFormBuilder.treeInfo;

			_simRenderer.renderingType = SimRenderer.RenderingType.Specified;
			_simRenderer.specifiedParticles = tInfo.leaves;
			_simRenderer.SetRenderedMesh(sim);
		}
	}
}