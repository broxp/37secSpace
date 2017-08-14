using UnityEngine;
using System.Collections;

public class TargetMovement : MonoBehaviour {
	public Transform target;
	public float speed = 1;
	public Misc misc;

	void Update () {
		if(misc.gameOver) {
			return;
		}
		Vector3 x = (target.position - transform.position);
		x.Normalize();
		transform.position += x * speed;
	}
}
