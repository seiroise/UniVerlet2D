using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	[RequireComponent(typeof(Collider2D))]
	public abstract class SimElemMarker : MonoBehaviour, IPoolableItem {

		/*
		 * Fields
		 */

		[SerializeField, ReadOnly]
		SimElemDefine.SimElemAttr _attr;
		[SerializeField, ReadOnly]
		int _uid;
		[SerializeField, ReadOnly]
		string _elemType;

		SimElemInfo _simElemInfo;

		Collider2D _markerCollider;

		/*
		 * Properties
		 */

		public SimElemDefine.SimElemAttr attr { get { return _attr; } set { _attr = value; } }
		public int uid { get { return _uid; } set { _uid = value; } }
		public string elemType { get { return _elemType; } set { _elemType = value; } }

		public SimElemInfo simElemInfo { get { return _simElemInfo; } }

		public Collider2D markerCollider { get { return _markerCollider; } }

		/*
		 * Unity events
		 */

		protected virtual void Awake() {
			_markerCollider = GetComponent<Collider2D>();
		}

		/*
		 * Interface
		 */
		
		public virtual void AwakeItem() { }

		/*
		 * Abstract methods
		 */
		
		public virtual bool SetSimElemInfo(SimElemInfo info, float depth) {
			_simElemInfo = info;
			return true;
		}

		public abstract void Activate();

		public abstract void Disactivate();
	}
}