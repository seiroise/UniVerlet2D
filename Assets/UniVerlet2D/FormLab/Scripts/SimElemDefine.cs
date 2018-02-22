using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	public static class SimElemDefine {

		// SimElementの属性
		public enum SimElemAttr {
			Particle,                           // パーティクル
			Spring,                             // スプリング
			Angle,                              // 角度
		}

		public enum SimElemType {
			Basic,                              // 基本要素
			Constraint,                         // 制約要素
			Interaction,                        // 作用要素
		}

		// SimElementの作成方法
		public enum SimElemMakeMethod {
			ClickSpace,                         // 何もない空間をクリックした場合
			ClickParticle,                      // パーティクルをクリックした場合(順)
			ClickParticleInParticularOrder,     // パーティクルをクリックした場合(順不同)
			ClickSpring,                        // バネをクリックした場合
		}

		public class SimElemProfile {
			// 基本情報
			public string profileID;        // profileを表すためのID
			public SimElemAttr attr;        // 外部からSimElementを把握するための属性
			public SimElemType type;        // 外部からSimElementを把握するための種類
			public Type makeSimElemType;    // 作成するSimElementを作成すルための型情報

			// 処理情報
			public int tableID;             // 所属するテーブルのID
			public int loopGroupID;         // シミュータ内で処理をするループグループID

			// 書き出し情報
			public string header;           // 外部データとして書き出す時のヘッダ

			// 編集情報
			public SimElemMakeMethod makeMethod;    // このSimElementを作成するための方法
			public SimElemAttr detectedMarkerAttr;  // 作成するときに検知する属性
			public int needMakingElemNum;           // makeMethodがClickParticleまたはClickParticleInParticularOrderの場合に使用する。
			public Type makeSimElemInfoType;        // 作成するSimElementを作成するための"仮実装クラス"の型情報
			public string markerID;                 // 対応するマーカーのID
			public float markerDepth;               // マーカーの表示深度

			// 描画情報
			public bool canRender;                  // 描画できるかどうか

			// デバッグ情報
			public bool canDrag;					// つまんで動かせる
		}

		static Dictionary<string, SimElemProfile> elemProfileDic = new Dictionary<string, SimElemProfile>() {
			{
				PARTICLE_ID,
				new SimElemProfile() {
					profileID = PARTICLE_ID,
					attr = SimElemAttr.Particle,
					type = SimElemType.Basic,
					makeSimElemType = typeof(Particle),

					tableID = 0,
					loopGroupID = 0,

					header = "p",

					makeMethod = SimElemMakeMethod.ClickSpace,
					needMakingElemNum = 0,
					makeSimElemInfoType = typeof(ParticleInfo),
					markerID = PARTICLE_ID,
					markerDepth = 0f,

					canRender = true,
				}
			},
			{
				SPRING_ID,
				new SimElemProfile() {
					profileID = SPRING_ID,
					attr = SimElemAttr.Spring,
					type = SimElemType.Constraint,
					makeSimElemType = typeof(SpringConstraint),

					tableID = 1,
					loopGroupID = 10,

					header = "sc",

					makeMethod = SimElemMakeMethod.ClickParticle,
					detectedMarkerAttr = SimElemAttr.Particle,
					needMakingElemNum = 2,
					makeSimElemInfoType = typeof(SpringConstraintInfo),
					markerID = SPRING_ID,
					markerDepth = 1,

					canRender = true,
				}
			},
			{
				ANGLE_ID,
				new SimElemProfile() {
					profileID = ANGLE_ID,
					attr = SimElemAttr.Angle,
					type = SimElemType.Constraint,
					makeSimElemType = typeof(AngleConstraint),

					tableID = 2,
					loopGroupID = 10,

					header = "ac",

					makeMethod = SimElemMakeMethod.ClickParticleInParticularOrder,
					detectedMarkerAttr = SimElemAttr.Particle,
					needMakingElemNum = 3,
					makeSimElemInfoType = typeof(AngleConstraintInfo),
					markerID = ANGLE_ID,
					markerDepth = 2,

					canRender = false,
				}
			},
			{
				PIN_ID,
				new SimElemProfile() {
					profileID = PIN_ID,
					attr = SimElemAttr.Particle,
					type = SimElemType.Constraint,
					makeSimElemType = typeof(PinConstraint),

					tableID = 3,
					loopGroupID = 10,

					header = "pc",

					makeMethod = SimElemMakeMethod.ClickParticle,
					detectedMarkerAttr = SimElemAttr.Particle,
					needMakingElemNum = 1,
					makeSimElemInfoType = typeof(PinConstraintInfo),
					markerID = PIN_ID,
					markerDepth = -1,

					canRender = false,
				}
			},
		};

		public const string PARTICLE_ID = "Particle";

		public const string SPRING_ID = "Spring";
		public const string ANGLE_ID = "Angle";
		public const string PIN_ID = "Pin";

		public const string STRETCH_ID = "Stretch";
		public const string HINGE_ID = "Hinge";
		public const string JET_ID = "Jet";

		public static SimElemProfile GetProfile(string profileID) {
			if(!elemProfileDic.ContainsKey(profileID)) {
				throw new System.Exception(string.Format("Not find {0} profile", profileID));
			}
			return elemProfileDic[profileID];
		}

		public static SimElemProfile GetProfileFromHeader(string header) {
			foreach(var profile in elemProfileDic.Values) {
				if(profile.header.Equals(header)) {
					return profile;
				}
			}
			return null;
		}
	}
}