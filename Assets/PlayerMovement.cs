using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {
	public Vector3 forward = Vector3.zero;
	public float decay = 0.97f;
	public float deg = 0f;
	public float speed = 0.08f;
	public float projectileSpeed = 0.08f;
	public float projectileSpawnDist = 0.7f;
	public float turnSpeed = 3f;
	public float turnConstAdd = 270f;
	public int projDeleteCount = 16;
	public GameObject projectile;
	private Vector3 startPos;
	private Quaternion startRota;
	public Misc misc;
	public List<GameObject> projectiles = new List<GameObject>();
	//void OnTriggerEnter2D(Collider2D collider) {
	//void OnCollisionEnter2D(Collision2D coll) {
			//Debug.DrawRay(collision.point, contact.normal, Color.white);
	//	print(coll);
	//}
	int gameOverTimer = 30;
	void Start() {
		startPos = transform.position;
		startRota = transform.rotation;
	}
	void Update () {
		var isJump = Input.GetButtonDown("Jump");
		
		if(misc.gameOver) {
			gameOverTimer--;
			if(isJump && gameOverTimer < 0) {
				gameOverTimer = 30;
				forward = Vector3.zero;
				misc.NotifyStart();
				transform.position = startPos;
				transform.rotation = startRota;
			}
			return;
		}
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");
		//Debug.DrawRay(new Vector3(v, h, 0), new Vector3(0, 1, 0), Color.white);
		if(v > 0) {
			forward = new Vector3(speed, 0, 0);
		} else {
			forward *= decay;
		}
		if(h < 0) {
			deg += turnSpeed;
		}
		if(h > 0) {
			deg -= turnSpeed;
		}
		if(v < 0) {
			forward = new Vector3(-speed, 0, 0);
		}
		var q = Quaternion.AngleAxis(deg, new Vector3(0,0,1));
		gameObject.transform.rotation = q;
		var withConstQ = Quaternion.AngleAxis(deg + turnConstAdd, new Vector3(0,0,1));
		var fwd = withConstQ * forward;
		var pos = gameObject.transform.position + fwd;
		pos = misc.Wrap(pos);
		gameObject.transform.position = pos;

		if(isJump){
			var projPos = gameObject.transform.position;
			projPos += withConstQ * new Vector3(projectileSpawnDist,0,0);
            var newProjectile = Instantiate(projectile, projPos, withConstQ)  as GameObject;
			newProjectile.GetComponent<Projectile>().misc = misc;
			projectiles.Add(newProjectile);
		}
		/*
		foreach(var item in projectiles) {
			float projSpeed = projectileSpeed; 
			//Mathf.Sqrt(fwd.x * fwd.x + fwd.y * fwd.y);
			//projSpeed = Mathf.Max(0.001f, projSpeed) * projectileSpeed;
			var fwd2 = new Vector3(projSpeed, 0, 0);
			fwd2 = item.transform.rotation * fwd2;
			var pos2 = item.transform.position;
			pos2 = wrap(pos2 + fwd2);
			item.transform.position = pos2;
			print(len(pos - pos2));
			//if(sqLen(pos - pos2) < distThreshold * distThreshold) {
				//W/2,H/2
			//	gameObject.transform.position = new Vector3(0,0,0);
			//}
		}*/
		while(projectiles.Count > projDeleteCount) {
			Destroy(projectiles[0]);
			projectiles.RemoveAt(0);
		}

		foreach(var item in misc.targets) {
			if(item != null) {
				var dist = misc.Length(transform.position - item.transform.position);
				if(dist < misc.distThresholdPlayer) {
					misc.GameOver();
				}
			}
		}
	}
}
