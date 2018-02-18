using UnityEngine;
using System;
using System.Collections.Generic;

namespace UniVerlet2D.Lab {

	/// <summary>
	/// オブジェクトプール
	/// </summary>
	public abstract class ObjectPool<T> : MonoBehaviour where T : Component, IPoolableItem {

		/// <summary>
		/// 内部プール
		/// </summary>
		[Serializable]
		public class InnerPool {

			[SerializeField]
			private T origin;
			private int currentIndex;
			private int currentSize;
			private int addNum;
			private Transform parent;
			private List<T> pool;

			public InnerPool(T origin, int initNum, int addNum, Transform parent) {
				this.origin = origin;
				this.currentIndex = 0;
				this.currentSize = 0;
				this.addNum = addNum;
				this.parent = parent;
				this.pool = new List<T>();
				AddObjects(initNum);
			}

			/// <summary>
			/// オブジェクトの追加
			/// </summary>
			/// <param name="num">Number.</param>
			private void AddObjects(int num) {
				for(int i = 0; i < num; ++i) {
					GameObject obj = Instantiate(origin.gameObject);
					obj.name = origin.name;
					obj.transform.SetParent(parent);
					obj.SetActive(false);
					pool.Add(obj.GetComponent<T>());
				}
				currentSize = pool.Count;
			}

			/// <summary>
			/// オブジェクトの追加
			/// </summary>
			/// <returns>The object.</returns>
			/// <param name="position">Position.</param>
			public T GetObject(Vector3 position) {
				T obj;
				for(int i = 0; i < currentSize; ++i) {
					currentIndex = (currentIndex + 1) % currentSize;
					obj = pool[currentIndex];
					if(!obj.gameObject.activeInHierarchy) {
						return ActivateObj(obj, position); ;
					}
				}
				//要素の追加
				currentIndex = currentSize;
				AddObjects(addNum);
				return ActivateObj(pool[currentIndex], position); ;
			}

			/// <summary>
			/// 複数のオブジェクトの取得
			/// </summary>
			/// <returns>The objects.</returns>
			/// <param name="num">Number.</param>
			/// <param name="position">Position.</param>
			public T[] GetObjects(int num, Vector3 position) {
				T[] objs = new T[num];
				for(int i = 0; i < num; ++i) {
					objs[i] = GetObject(position);
				}
				return objs;
			}

			/// <summary>
			/// オブジェクトの有効化
			/// </summary>
			/// <returns>The object.</returns>
			/// <param name="obj">Object.</param>
			/// <param name="position">Position.</param>
			T ActivateObj(T obj, Vector3 position) {
				obj.gameObject.SetActive(true);
				obj.transform.position = position;
				obj.AwakeItem();
				return obj;
			}
		}

		/*
		 * Fields
		 */

		[SerializeField, Range(1, 128)]
		private int initNum = 16;
		[SerializeField, Range(1, 128)]
		private int addNum = 16;

		private Dictionary<string, InnerPool> poolDic;

		/*
		 * Unity events
		 */

		protected virtual void Awake() {
			poolDic = new Dictionary<string, InnerPool>();
		}

		/*
		 * Functions
		 */

		public InnerPool RegistObject(string name, T obj) {
			if(obj == null) return null;
			InnerPool pool;
			if(!poolDic.TryGetValue(name, out pool)) {
				pool = new InnerPool(obj, initNum, addNum, transform);
				poolDic.Add(name, pool);
			}
			return pool;
		}

		public InnerPool GetPool(T obj) {
			InnerPool pool;
			poolDic.TryGetValue(obj.name, out pool);
			return pool;
		}

		public InnerPool GetPool(string name) {
			InnerPool pool;
			poolDic.TryGetValue(name, out pool);
			return pool;
		}

		public T GetObject(string name, Vector3 pos) {
			InnerPool pool;
			poolDic.TryGetValue(name, out pool);
			return pool != null ? pool.GetObject(pos) : null;
		}
	}
}