using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PP2D.Examples {

	[RequireComponent(typeof(Rigidbody2D))]
	public class BallController : MonoBehaviour {

		[SerializeField]
		float speed = 10f;
		Rigidbody2D rbody2d { get; set; }

		void Awake() {
			rbody2d = GetComponent<Rigidbody2D>();
		}

		void Update() {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			rbody2d.AddForce(new Vector2(h, v) * speed);
		}
	}
}