using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniVerlet2D {

	[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
	public class Mover : MonoBehaviour {

		public float speed = 10f;

		Rigidbody2D _rbody2d;

		// Use this for initialization
		void Start() {
			_rbody2d = GetComponent<Rigidbody2D>();
			_rbody2d.gravityScale = 0f;
		}

		// Update is called once per frame
		void FixedUpdate() {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			_rbody2d.AddForce(new Vector2(h, v) * speed);
		}
	}
}