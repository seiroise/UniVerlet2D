using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D {

	public static class SimElementHelper {

		/// <summary>
		/// 指定した型名のSimElementのインデックスリストを取得する
		/// </summary>
		/// <returns>The sim element indices.</returns>
		/// <param name="list">List.</param>
		/// <param name="typeName">Filter name.</param>
		public static List<int> GetSimElemIndices(List<SimElement> list, string typeName) {
			List<int> founds = new List<int>();
			for(var i = 0; i < list.Count; ++i) {
				if(list[i].name.Equals(typeName)) {
					founds.Add(i);
				}
			}
			return founds;
		}

		/// <summary>
		/// 指定した型のSimElementのインデックスリストを取得する
		/// </summary>
		/// <returns>The sim element indices.</returns>
		/// <param name="list">List.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<int> GetSimElemIndices<T>(List<SimElement> list) where T : SimElement {
			return GetSimElemIndices(list, typeof(T).Name);
		}

		/// <summary>
		/// 指定した型名のSimElementのリストを取得する
		/// </summary>
		/// <returns>The sim elems.</returns>
		/// <param name="list">List.</param>
		/// <param name="typeName">Filter name.</param>
		public static List<SimElement> GetSimElems(List<SimElement> list, string typeName) {
			List<SimElement> founds = new List<SimElement>();
			for(var i = 0; i < list.Count; ++i) {
				if(list[i].name.Equals(typeName)) {
					founds.Add(list[i]);
				}
			}
			return founds;
		}

		/// <summary>
		/// 指定した型のSimElementのリストを取得する
		/// </summary>
		/// <returns>The sim elems.</returns>
		/// <param name="list">List.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> GetSimElems<T>(List<SimElement> list) where T : SimElement {
			List<T> founds = new List<T>();
			var findName = typeof(T).Name;
			for(var i = 0; i < list.Count; ++i) {
				if(list[i].name.Equals(findName)) {
					founds.Add(list[i] as T);
				}
			}
			return founds;
		}

		/// <summary>
		/// 指定した座標がパーティクルに重なっている場合はtrueを返しそのindexを出力する
		/// </summary>
		/// <returns><c>true</c>, if overlap particle index was found, <c>false</c> otherwise.</returns>
		/// <param name="list">List.</param>
		/// <param name="pos">Position.</param>
		/// <param name="particleRadius">Particle radius.</param>
		/// <param name="idx">Index.</param>
		public static bool FindOverlapParticleIdx(List<Particle> list, Vector2 pos, float particleRadius, out int idx) {
			float sqrRadius = particleRadius * particleRadius;
			for(var i = 0; i < list.Count; ++i) {
				var p = list[i];
				if((pos - p.pos).sqrMagnitude <= sqrRadius) {
					idx = i;
					return true;
				}
			}
			idx = -1;
			return false;
		}

		/// <summary>
		/// 指定した座標がパーティクルに重なっている場合はtrueを返し、そのindexを出力する
		/// </summary>
		/// <returns><c>true</c>, if overlap particle index was found, <c>false</c> otherwise.</returns>
		/// <param name="particles">Particles.</param>
		/// <param name="mat">Mat.</param>
		/// <param name="pos">Position.</param>
		/// <param name="particleRadius">Particle radius.</param>
		/// <param name="idx">Index.</param>
		public static bool FindOverlapParticleIdx(List<Particle> particles, Matrix4x4 mat, Vector2 pos, float particleRadius, out int idx) {
			float sqrRadius = particleRadius * particleRadius;
			Vector3 tempPos = pos;
			for(var i = 0; i < particles.Count; ++i) {
				var p = particles[i];
				if((tempPos - mat.MultiplyPoint3x4(p.pos)).sqrMagnitude <= sqrRadius) {
					idx = i;
					return true;
				}
			}
			idx = -1;
			return false;
		}

		/// <summary>
		/// 指定した型名のオーダーにしたがってSimElementのリストをソートし、それを返す。
		/// </summary>
		/// <returns>The sim elements.</returns>
		/// <param name="list">List.</param>
		/// <param name="order">Order.</param>
		public static List<SimElement> ReorderSimElements(List<SimElement> list, params string[] order) {
			HashSet<int> indicesSet = new HashSet<int>();
			Dictionary<int, int> indexMap = new Dictionary<int, int>();

			for(var i = 0; i < order.Length; ++i) {
				var founds = GetSimElemIndices(list, order[i]);
				for(var j = 0; j < founds.Count; ++j) {
					indicesSet.Add(founds[j]);
				}
			}

			for(var i = 0; i < list.Count; ++i) {
				if(!indicesSet.Contains(i)) {
					indicesSet.Add(i);
				}
			}

			List<SimElement> reorderedList = new List<SimElement>();
			foreach(int i in indicesSet) {
				reorderedList.Add(list[i]);
			}
			return reorderedList;
		}

		/// <summary>
		/// パーティクルの集合の包括領域を計算し、それを返す
		/// </summary>
		/// <returns>The particle bounds.</returns>
		/// <param name="particles">Particles.</param>
		/// <param name="particleRadius">Particle radius.</param>
		public static Bounds ComputeParticlesBounds(List<Particle> particles, float particleRadius) {
			Bounds bounds = new Bounds();

			if(particles.Count <= 0) {
				return bounds;
			} else if(particles.Count <= 1) {
				return new Bounds(particles[0].pos, Vector3.one * particleRadius);
			}
			float xMin, xMax, yMin, yMax;
			Vector2 temp = particles[0].pos;
			xMin = xMax = temp.x;
			yMin = yMax = temp.y;

			for(var i = 1; i < particles.Count; ++i) {
				temp = particles[i].pos;
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
			float dx = xMax - xMin;
			float dy = yMax - yMin;

			return new Bounds(
				new Vector3(dx * 0.5f + xMin , dy * 0.5f + yMin),
				new Vector3(dx + diameter, dy + diameter)
			);
		}

		/// <summary>
		/// パーティクルの集合の包括円を計算し、それを返す。
		/// </summary>
		/// <returns>The particles bounds.</returns>
		/// <param name="particles">Particles.</param>
		/// <param name="particleRadius">Particle radius.</param>
		public static BoundingCircle ComputeParticlesBoundingCircle(List<Particle> particles, float particleRadius) {
			if(particles.Count <= 0) {
				return BoundingCircle.zero;
			} else if(particles.Count <= 1) {
				return new BoundingCircle(particles[0].pos, particleRadius);
			}
			float xMin, xMax, yMin, yMax;
			Vector2 temp = particles[0].pos;
			xMin = xMax = temp.x;
			yMin = yMax = temp.y;

			for(var i = 1; i < particles.Count; ++i) {
				temp = particles[i].pos;
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
	}
}