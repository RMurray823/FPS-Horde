using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public Rigidbody player;
	private Rigidbody body;
	public float maxSpeed = 1f;

	void Start () {
		body = GetComponent<Rigidbody> ();
	}


	void FixedUpdate() {
		body.velocity = Seek(player.position);
	}

	Vector3 Seek(Vector3 targetPos) {
		Vector2 targetPos2D = new Vector2(targetPos.x, targetPos.z);
		Vector2 localPos2D = new Vector2 (body.position.x, body.position.z);

		Vector2 desiredVelocity = targetPos2D-localPos2D;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

		Vector3 result = new Vector3 (desiredVelocity.x - body.velocity.x, 0, desiredVelocity.y - body.velocity.z);

		return result;
	}

	void Shot() {
		Destroy (gameObject);
	}
}