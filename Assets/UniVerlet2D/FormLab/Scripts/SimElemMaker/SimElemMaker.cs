using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UniVerlet2D.Lab {

	public class SimElemMaker : MonoBehaviour {

		[System.Serializable]
		public class SimElementEvent : UnityEvent<SimElemInfo> { }

		/*
		 * Fields
		 */

		public SimElementEvent onMakeSimElement;

		SimElemDefine.SimElemProfile _loadedProfile;        // 作成プロファイル

		bool _isClickedSpace;								// 空間をクリックしたか
		Vector2 _pos;										// 作成用の座標
		List<SimElemInfo> _infos;                           // クリックしたマーカーに対応するSimElemInfo

		UIDDistributer _uidDistributer;						// uid分配器

		/*
		 * Unity events
		 */

		void Awake() {
			_uidDistributer = new UIDDistributer();
			ClearMakingInfos();
		}

		/*
		 * Methods
		 */

		public void SetSimElemProfile(string profileID) {
			var profile = SimElemDefine.GetProfile(profileID);
			if(profile == null) {
				return;
			}
			_loadedProfile = profile;
		}

		bool CanMake() {
			switch(_loadedProfile.makeMethod) {
			case SimElemDefine.SimElemMakeMethod.ClickSpace:
				return _isClickedSpace;
			case SimElemDefine.SimElemMakeMethod.ClickParticle:
			case SimElemDefine.SimElemMakeMethod.ClickParticleInParticularOrder:
				return _loadedProfile.needMakingElemNum <= _infos.Count;
			case SimElemDefine.SimElemMakeMethod.ClickSpring:
				break;
			}
			return false;
		}

		object[] GetSimElemParams() {
			object[] elemParams;
			switch(_loadedProfile.makeMethod) {
			case SimElemDefine.SimElemMakeMethod.ClickSpace:
				elemParams = new object[1];
				elemParams[0] = _pos;
				return elemParams;
			case SimElemDefine.SimElemMakeMethod.ClickParticle:
			case SimElemDefine.SimElemMakeMethod.ClickParticleInParticularOrder:
				elemParams = new object[_infos.Count];
				for(var i = 0; i < _infos.Count; ++i) {
					elemParams[i] = _infos[i];
				}
				return elemParams;
			case SimElemDefine.SimElemMakeMethod.ClickSpring:
				break;
			}
			return null;
		}

		void MakeSimElement() {
			if(_loadedProfile == null) {
				return;
			}
			var simElemInfo = System.Activator.CreateInstance(_loadedProfile.makeSimElemInfoType) as SimElemInfo;

			var elemParams = GetSimElemParams();
			if(elemParams == null) {
				return;
			}
			simElemInfo.SetParams(_uidDistributer.next, _loadedProfile.profileID, elemParams);

			onMakeSimElement.Invoke(simElemInfo);
		}

		void ClearMakingInfos() {
			if(_infos == null) {
				_infos = new List<SimElemInfo>();
			} else {
				_infos.Clear();
			}
		}

		/*
		 * Callbacks
		 */

		public void OnDownSpace(Vector3 pos) {
			_isClickedSpace = true;
			_pos = pos;

			if(CanMake()) {
				MakeSimElement();
				ClearMakingInfos();
			}
		}

		public void OnDownMarker(SimElemMarker marker) {
			_isClickedSpace = false;
			if(marker.attr == _loadedProfile.detectedMarkerAttr) {
				_infos.Add(marker.simElemInfo);
				if(CanMake()) {
					MakeSimElement();
					ClearMakingInfos();
				}
			}
		}
	}
}