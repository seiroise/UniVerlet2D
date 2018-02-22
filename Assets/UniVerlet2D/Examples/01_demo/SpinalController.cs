using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVerlet2D.Data;

namespace UniVerlet2D.Examples {

	[RequireComponent(typeof(MonoSimulator), typeof(SimRenderer))]
	public class SpinalController : MonoBehaviour {

		/*
		 * Fields
		 */

		public SpinalFormBuilder spinalBuilder;

		MonoSimulator _monoSim;
		SimRenderer _simRenderer;

		[SerializeField, HideInInspector]
		TranslationInteraction _translation;

		/*
		 * Properties
		 */

		public MonoSimulator monoSim { get { return _monoSim ? _monoSim : _monoSim = GetComponent<MonoSimulator>(); } }

		/*
		 * Unity events
		 */

		void Awake() {
			var sim = monoSim.sim;
			_simRenderer = GetComponent<SimRenderer>();
			_translation = new TranslationInteraction(sim.GetParticleAt(0), new Vector2(0f, 0f));
			// sim.AddInteraction(_translation);
		}

		void Update() {
			float h, v;
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");

			_translation.velocity = new Vector2(h, v);
		}

		/*
		 * Methods
		 */

		public void BuildForm() {
			monoSim.startWithClear = false;

			var sim = monoSim.sim;
			sim.ImportForm(spinalBuilder.Build(Vector2.zero));

			_simRenderer.renderingType = SimRenderer.RenderingType.All;
			_simRenderer.SetRenderedMesh(sim);
		}
	}
}