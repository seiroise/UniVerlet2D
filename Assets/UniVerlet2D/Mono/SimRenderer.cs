using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MonoSimulator))]
	public class SimRenderer : MonoBehaviour {

		public enum RenderingType {
			All,
			Debug,
			Specified
		}

		/*
		 * Fields
		 */

		[SerializeField]
		bool _updateMesh = true;
		[SerializeField]
		RenderingType _renderingType = RenderingType.All;

		[Header("All")]
		public Color particleColor = Color.white;
		public Color springColor = Color.white;

		[Header("Debug")]
		public Gradient springStressGradient;

		[Header("Specified")]
		public List<int> specifiedParticles;

		[Header("Mesh template")]

		public Vector4[] particlePositions = {
			new Vector4(-0.5f, 0f, 0f, 1f),
			new Vector4(0f, 0.5f, 0f, 1f),
			new Vector4(0f, -0.5f, 0f, 1f),
			new Vector4(0.5f, 0f, 0f, 1f)
		};

		public Vector2[] particleUVs = {
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};

		public Vector4[] springPositions = {
			new Vector4(-0.5f, -0.3f, 0f, 1f),
			new Vector4(-0.5f, 0.3f, 0f, 1f),
			new Vector4(0.5f, -0.3f, 0f, 1f),
			new Vector4(0.5f, 0.3f, 0f, 1f)
		};

		public Vector2[] springUVs = {
			new Vector2(0.5f, 0f),
			new Vector2(0.5f, 1f),
			new Vector2(0.5f, 0f),
			new Vector2(0.5f, 1f)
		};

		MonoSimulator _monoSim;

		MeshFilter _filter;
		MeshBuilder _builder;

		Action<Simulator> _addParticleMeshRoutine;
		Action<Simulator> _addSpringMeshRoutine;

		/*
		 * Properties
		 */

		public bool updateMesh { get { return _updateMesh; } set { _updateMesh = value; } }
		public RenderingType renderingType { get { return _renderingType; } set { _renderingType = value; } }

		/*
		 * Unity events
		 */

		void Awake() {
			_monoSim = GetComponent<MonoSimulator>();
			_filter = GetComponent<MeshFilter>();
			_builder = new MeshBuilder("SimMesh", true, useColor: true, dynamic: true);
		}

		void OnWillRenderObject() {
			if(_updateMesh) {
				SetRenderedMesh(_monoSim.sim);
			}
		}

		/*
		 * Methods
		 */

		public void SetRenderedMesh(Simulator sim) {
			if(_builder == null) {
				_filter = GetComponent<MeshFilter>();
				_builder = new MeshBuilder("SimMesh", true, useColor: true, dynamic: true);
			}
			_builder.Clear();

			SetEachTypeMesh(sim);

			_builder.Apply();
			_filter.mesh = _builder.mesh;
		}

		public void Clear() {
			_builder.Clear();
			_builder.Apply();
			_filter.mesh = _builder.mesh;
		}

		void SetEachTypeMesh(Simulator sim) {
			switch(_renderingType) {
			case RenderingType.All:
				AddSpringMesh(sim);
				AddParticleMesh(sim);
				break;
			case RenderingType.Debug:
				AddSpringDebugMesh(sim);
				AddParticleDebugMesh(sim);
				break;
			case RenderingType.Specified:
				AddSpringMesh(sim);
				AddSpecifiedParticleMesh(sim);
				break;
			}
		}

		/*
		 * AddMesh methods
		 */

		void AddParticleMesh(Simulator sim) {
			Matrix4x4 mat;
			for(var i = 0; i < sim.numOfParticles; ++i) {
				mat = sim.GetParticleAt(i).worldMatrix;
				_builder.AddQuad(
					mat * particlePositions[0],
					mat * particlePositions[1],
					mat * particlePositions[2],
					mat * particlePositions[3]
				);
				_builder.AddQuadUV(
					particleUVs[0],
					particleUVs[1],
					particleUVs[2],
					particleUVs[3]
				);
				_builder.AddQuadColor(particleColor);
			}
		}

		void AddSpringMesh(Simulator sim) {
			Matrix4x4 mat;
			for(var i = 0; i < sim.numOfSprings; ++i) {
				mat = sim.GetSpringAt(i).worldMatrix;
				_builder.AddQuad(
					mat * springPositions[0],
					mat * springPositions[1],
					mat * springPositions[2],
					mat * springPositions[3]
				);
				_builder.AddQuadUV(
					springUVs[0],
					springUVs[1],
					springUVs[2],
					springUVs[3]
				);
				_builder.AddQuadColor(springColor);
			}
		}

		void AddParticleDebugMesh(Simulator sim) {
			Matrix4x4 mat;
			for(var i = 0; i < sim.numOfParticles; ++i) {
				mat = sim.GetParticleAt(i).worldMatrix;
				_builder.AddQuad(
					mat * particlePositions[0],
					mat * particlePositions[1],
					mat * particlePositions[2],
					mat * particlePositions[3]
				);
				_builder.AddQuadUV(
					particleUVs[0],
					particleUVs[1],
					particleUVs[2],
					particleUVs[3]
				);
				_builder.AddQuadColor(particleColor);
			}
		}

		void AddSpringDebugMesh(Simulator sim) {
			Matrix4x4 mat;
			for(var i = 0; i < sim.numOfSprings; ++i) {
				var s = sim.GetSpringAt(i);
				mat = s.worldMatrix;
				_builder.AddQuad(
					mat * springPositions[0],
					mat * springPositions[1],
					mat * springPositions[2],
					mat * springPositions[3]
				);
				_builder.AddQuadUV(
					springUVs[0],
					springUVs[1],
					springUVs[2],
					springUVs[3]
				);

				var stress = s.currentLength / (s.length * 2f);
				_builder.AddQuadColor(springStressGradient.Evaluate(stress));
			}
		}

		void AddSpecifiedParticleMesh(Simulator sim) {
			Matrix4x4 mat;
			for(var i = 0; i < specifiedParticles.Count; ++i) {
				mat = sim.GetParticleAt(specifiedParticles[i]).worldMatrix;
				_builder.AddQuad(
					mat * particlePositions[0],
					mat * particlePositions[1],
					mat * particlePositions[2],
					mat * particlePositions[3]
				);
				_builder.AddQuadUV(
					particleUVs[0],
					particleUVs[1],
					particleUVs[2],
					particleUVs[3]
				);
				_builder.AddQuadColor(particleColor);
			}
		}
	}
}