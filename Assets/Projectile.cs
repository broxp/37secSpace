using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float projectileSpeed = 0.1f;
	public Misc misc;
	public PlayerMovement player;
	
	void Update () {
		if(misc.gameOver) {
			return;
		}
		var fwd2 = new Vector3(projectileSpeed, 0, 0);
		fwd2 = transform.rotation * fwd2;
		transform.position = (transform.position + fwd2);
		//misc.Wrap
		if(misc.IsOutside(transform.position) && gameObject.name != "Shot") { 
			Destroy(gameObject);
			player.projectiles.Remove(gameObject);
		} else {
			GameObject remove = null;
			foreach(var item in misc.targets) {
				if(gameObject != null && item != null)
				{
					var dist = misc.Length(transform.position - item.transform.position);
					//print(dist);
					if(dist < misc.distThreshold){
						Destroy(gameObject);
						player.projectiles.Remove(gameObject);
						Destroy(item);
						remove = item;
						misc.NoticeTargetHit();
						break;
					}
				}
			}
			if(remove != null) {
				misc.targets.Remove(remove);
			}
		}
	}

	//void OnTriggerEnter2D(Collider2D collider) {
	/*void OnCollisionEnter2D(Collision2D coll) {
		print("coll " + coll);
	}*/
}
