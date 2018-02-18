using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D.Lab {

	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteMarker : SimElemMarker {

		public Color activeColor = Color.white;
		public Color disactiveColor = Color.gray;

		SpriteRenderer _spriteRenderer;

		protected override void Awake() {
			base.Awake();
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public override void Activate() {
			_spriteRenderer.color = activeColor;
			markerCollider.enabled = true;
		}

		public override void Disactivate() {
			_spriteRenderer.color = disactiveColor;
			markerCollider.enabled = false;
		}
	}
}