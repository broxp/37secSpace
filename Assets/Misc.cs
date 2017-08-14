using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Misc : MonoBehaviour {
	public float W = 4;
	public float H = 8;
	public GameObject target;
	public int targetCount = 10;
	public List<GameObject> targets = new List<GameObject>();
	public int points = 0;
	public float time = 37;
	private float startTime;
	public Text text;
	private int timeNow;
	public int refreshAtTargetMinimum = 3;
	public bool gameOver = true;
	public float distThreshold = 0.4f;
	public float distThresholdPlayer = 0.2f;
	public float spawnDistTimesAway = 3;
	public PlayerMovement player;

	public void Start() {
		if(startTime == 0) {
			startTime = time;
		}
	}
	public void NoticeTargetHit() {
		points++;
		UpdateText();
	}
	public void NotifyStart() {
		gameOver = false;
		time = startTime;
		points = 0;
		while(targets.Count > 0) {
			Destroy(targets[0]);
			targets.RemoveAt(0);
		}
		GenerateTargets();
		timeNow = getNow();
		UpdateText();
	}
	public void GenerateTargets() {
		for(int i = targets.Count; i < targetCount; i++) {
			var pos = transform.position;
			var dx = Random.value * W - W/2;
			var dy = Random.value * H - H/2;
			pos += new Vector3(dx, dy, 0);
			if(Length(player.transform.position - pos) < spawnDistTimesAway * distThreshold) {
				i--;
				continue;
			}
			var t = Instantiate(target, pos, transform.rotation) as GameObject;
			targets.Add(t);
		}
	}
	public void UpdateText() {
		text.text = time + " sec\n" + points + " points";
	}
	public void GameOver() {
		gameOver = true;
		text.text = "game over\nyou scored " + points + " points\n";
		
		var key = "spaces-score";
        var i = PlayerPrefs.GetInt(key, -1);
		if(points > i) {
			PlayerPrefs.SetInt(key, points);
			PlayerPrefs.Save();
			text.text += "new highscore: " + points;
		} else {
			text.text += "highscore was: " + i;
		}
		text.text += "\nshoot to restart";
	}

	public void Update() {
		if(gameOver) {
			return;
		}
		if(targets.Count <= refreshAtTargetMinimum) {
			GenerateTargets();
		}
		int newTime = getNow();
		if(newTime != timeNow) {
			timeNow = newTime;
			time--;
			if(time < 0) {
				GameOver();
			} else {
				UpdateText();
			}
		}
	}
	int getNow() {
		return (int)System.Math.Round(Time.realtimeSinceStartup);
	}
	public bool IsOutside(Vector3 pos) {
		return pos != Wrap(pos);
	}
	
	public Vector3 Wrap(Vector3 pos) {
		if(pos.x > W/2)
		{
			pos.x = -W/2;
		}
		if(pos.x < -W/2)
		{
			pos.x = W/2;
		}
		if(pos.y > H/2)
		{
			pos.y = -H/2;
		}
		if(pos.y < -H/2)
		{
			pos.y = H/2;
		}
		return pos;
	}

	public float Length(Vector3 pos){
		return Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y);
	}

	public void NoticePlayerCollided() {
		points = 0;
		UpdateText();
	}
}
