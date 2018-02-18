using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public class MarkerManager : ObjectPool<SimElemMarker> {

		public const string PARTICLE_ID = "Particle";

		public const string SPRING_ID = "Spring";
		public const string ANGLE_ID = "Angle";
		public const string PIN_ID = "Pin";

		public const string STRETCH_ID = "Stretch";

		/*
		 * Fields
		 */

		[Header("Particle")]
		public SimElemMarker particleMarkerPref;
		public float particleMarkerDepth = 0f;

		[Header("Spring")]
		public SimElemMarker springMarkerPref;
		public float springMarkerDepth = 1f;

		[Header("Angle")]
		public SimElemMarker angleMarkerPref;
		public float angleMarkerDepth = 2f;

		[Header("Pin")]
		public SimElemMarker pinMarkerPref;
		public float pinMarkerDepth = -1f;

		[Header("Stretch")]
		public SimElemMarker stretchMarkerPref;
		public float stretchMarkerDepth = 0.5f;

		Dictionary<string, List<SimElemMarker>> _markerDic;

		/*
		 * Unity events
		 */

		protected override void Awake() {
			base.Awake();

			_markerDic = new Dictionary<string, List<SimElemMarker>>();
			RegistObject(PARTICLE_ID, particleMarkerPref);
			RegistObject(SPRING_ID, springMarkerPref);
			RegistObject(ANGLE_ID, angleMarkerPref);
			RegistObject(PIN_ID, pinMarkerPref);
		}

		/*
		 * Marker manage
		 */

		SimElemMarker MakeMarker(string id, SimElement simElem, Vector3 pos) {

			var marker = GetObject(id, pos);
			marker.elemType = id;
			marker.uid = simElem.uid;

			List<SimElemMarker> markers;
			if(!_markerDic.TryGetValue(id, out markers)) {
				markers = new List<SimElemMarker>();
				_markerDic.Add(id, markers);
			}
			markers.Add(marker);

			return marker;
		}

		void DeleteMarker(string id, SimElement simElem) {
			List<SimElemMarker> markers;
			if(!_markerDic.TryGetValue(id, out markers)) {
				return;
			}

			for(var i = 0; i < markers.Count; ++i) {
				if(markers[i].uid == simElem.uid) {
					markers[i].gameObject.SetActive(false);
					markers.RemoveAt(i);
					return;
				}
			}
		}

		void DeleteMarkers(string id) {
			List<SimElemMarker> markers;
			if(!_markerDic.TryGetValue(id, out markers)) {
				return;
			}
			for(var i = 0; i < markers.Count; ++i) {
				markers[i].gameObject.SetActive(false);
			}
			markers.Clear();
		}

		/*
		 * Particle related
		 */

		public void MakeParticleMarker(Particle p) {
			Vector3 pos = p.pos;
			pos.z = particleMarkerDepth;

			MakeMarker(PARTICLE_ID, p, pos);
		}

		public void DeleteParticleMarker(Particle p) {
			DeleteMarker(PARTICLE_ID, p);
		}

		public void DeleteParticleMarkers() {
			DeleteMarkers(PARTICLE_ID);
		}

		/*
		 * Spring related
		 */

		public void MakeSpringMarker(SpringConstraint s) {
			Vector3 pos = s.middlePos;
			pos.z = springMarkerDepth;

			var marker = MakeMarker(SPRING_ID, s, pos);
			marker.transform.rotation = Quaternion.AngleAxis(s.a2bRadian * Mathf.Rad2Deg, Vector3.forward);
			marker.transform.localScale = new Vector3(s.currentLength, 0.4f, 1f);
		}

		public void DeleteSpringMarker(SpringConstraint s) {
			DeleteMarker(SPRING_ID, s);
		}

		public void DeleteSpringMarkers() {
			DeleteMarkers(SPRING_ID);
		}

		/*
		 * Angle related
		 */

		public void MakeAngleMarker(AngleConstraint a) {
			Vector3 pos = a.m.pos;
			pos.z = angleMarkerDepth;

			var marker = (AngleMarker)MakeMarker(ANGLE_ID, a, pos);
			marker.SetAngle(a);
		}

		public void DeleteAngleMarker(AngleConstraint a) {
			DeleteMarker(ANGLE_ID, a);
		}

		public void DeleteAngleMarkers() {
			DeleteMarkers(ANGLE_ID);
		}

		/*
		 * Pin related
		 */

		public void MakePinMarker(PinConstraint p) {
			Vector3 pos = p.pos;
			pos.z = pinMarkerDepth;

			MakeMarker(ANGLE_ID, p, pos);
		}

		public void DeletePinMarker(PinConstraint p) {
			DeleteMarker(PIN_ID, p);
		}

		public void DeletePinMarkers() {
			DeleteMarkers(PIN_ID);
		}

		/*
		 * Stretch related
		 */

		public void MakeStretchMarker(StretchInteraction s) {
			Vector3 pos = s.middlePos;
			pos.z = stretchMarkerDepth;

			var marker = MakeMarker(SPRING_ID, s, pos);
			marker.transform.rotation = Quaternion.AngleAxis(s.a2bRadian * Mathf.Rad2Deg, Vector3.forward);
			marker.transform.localScale = new Vector3(s.currentLength, 0.4f, 1f);
		}

		public void DeleteStretchMarker(StretchInteraction s) {
			DeleteMarker(STRETCH_ID, s);
		}

		public void DeleteStretchMarkers() {
			DeleteMarkers(STRETCH_ID);
		}

		/*
		 * Methods
		 */

		public void Clear() {
			DeleteParticleMarkers();

			DeleteSpringMarkers();
			DeleteAngleMarkers();
			DeletePinMarkers();

			DeleteStretchMarkers();
		}

		public void MakeFromSim(Simulator sim) {
			for(var i = 0; i < sim.numOfParticles; ++i) {
				MakeParticleMarker(sim.GetParticleAt(i));
			}
			for(var i = 0; i < sim.numOfSprings; ++i) {
				MakeSpringMarker(sim.GetSpringAt(i));
			}
			for(var i = 0; i < sim.numOfAngles; ++i) {
				MakeAngleMarker(sim.GetAngleAt(i));
			}
			for(var i = 0; i < sim.numOfPins; ++i) {
				MakePinMarker(sim.GetPinAt(i));
			}
			// for(var i = 0; i < sim.numOfPins; ++i) {
			//	MakePinMarker(sim.GetPinAt(i));
			// }
		}

		public void Activate(string id, bool otherDisactivate = true) {
			if(otherDisactivate) {
				foreach(var key in _markerDic.Keys) {
					if(key.Equals(id)) {
						SwitchActive(_markerDic[key]);
					} else {
						SwitchDisactive(_markerDic[key]);
					}
				}
			} else {
				List<SimElemMarker> markers;
				if(_markerDic.TryGetValue(id, out markers)) {
					SwitchActive(markers);
				}
			}
		}

		void SwitchActive(List<SimElemMarker> markers) {
			for(var i = 0; i < markers.Count; ++i) {
				markers[i].Activate();
			}
		}

		void SwitchDisactive(List<SimElemMarker> markers) {
			for(var i = 0; i < markers.Count; ++i) {
				markers[i].Disactivate();
			}
		}
	}
}