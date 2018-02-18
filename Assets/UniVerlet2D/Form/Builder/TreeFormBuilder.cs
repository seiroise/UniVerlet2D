using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Data {

	[System.Serializable]
	public class TreeFormBuilder {

		[System.Serializable]
		public class TreeInfo {
			public List<int> leaves;
			public List<int> branches;

			public TreeInfo() {
				leaves = new List<int>();
				branches = new List<int>();
			}

			public void Clear() {
				leaves.Clear();
				branches.Clear();
			}
		}

		/*
		 * Fields
		 */

		[Range(1, 16)]
		public int maxDepth = 4;

		[Header("Length")]
		[Range(0.1f, 100f)]
		public float baseBranchLength = 4f;
		[MinMaxRange(-10f, 10f)]
		public MinMax branchLength = new MinMax(-1f, 1f);
		[MinMaxRange(0.01f, 0.99f)]
		public MinMax sunkLengthScale = new MinMax(0.8f, 0.9f);
		[Range(0f, 10f)]
		public float lengthThreshold = 1f;

		[Header("Angle")]
		[MinMaxRange(0f, 90f)]
		public MinMax spreadedLeftBranchAngle = new MinMax(30f, 60f);
		[MinMaxRange(0f, 90f)]
		public MinMax spreadedRightBranchAngle = new MinMax(30, 60f);

		Simulator _sim;

		TreeInfo _treeInfo;

		/*
		 * Properties
		 */

		public TreeInfo treeInfo { get { return _treeInfo; } }

		/*
		 * Functions
		 */

		public Form Build(Vector2 rootPosition) {
			if(_sim == null) {
				_sim = new Simulator();
				_sim.Init();
			} else {
				_sim.Clear();
			}

			if(_treeInfo == null) {
				_treeInfo = new TreeInfo();
			} else {
				_treeInfo.Clear();
			}

			var p = _sim.MakeParticle(rootPosition);
			var tp = MakeBranch(p, baseBranchLength + branchLength.random, 90f, maxDepth);
			return _sim.ExportForm();
		}

		Particle MakeBranch(Particle particle, float length, float angle, int depth) {
			float rad = angle * Mathf.Deg2Rad;
			Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
			var tp = _sim.MakeParticle(particle.pos + dir * length);
			_sim.MakeSpring(particle, tp);

			if(depth > 0) {
				bool makeBranch = false;
				// left
				var leftLen = (length + branchLength.random) * sunkLengthScale.random;
				if(leftLen > lengthThreshold) {
					var lp = MakeBranch(tp, leftLen, angle + spreadedLeftBranchAngle.random, depth - 1);
					_sim.MakeAngle(particle, lp, tp, 1f);
					makeBranch = true;
				}
				// right
				var rightLen = (length + branchLength.random) * sunkLengthScale.random;
				if(rightLen > lengthThreshold) {
					var rp = MakeBranch(tp, rightLen, angle - spreadedRightBranchAngle.random, depth - 1);
					_sim.MakeAngle(particle, rp, tp, 1f);
					makeBranch = true;
				}

				if(!makeBranch) {
					_treeInfo.leaves.Add(_sim.numOfParticles - 1);
				}
			} else {
				_treeInfo.leaves.Add(_sim.numOfParticles - 1);
			}

			return tp;
		}
	}
}