using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

	public int moveSpeed = 230;
	public int trailDuration = 1;

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed); // Simple way to move the trail... other way would be rigidbody
		Destroy (gameObject, trailDuration); // Destroy trail clones
	}
}
