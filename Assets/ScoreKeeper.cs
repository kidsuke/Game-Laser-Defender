using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	private static int score;
	private Text scoreDisplay;

	// Use this for initialization
	void Start () {
		scoreDisplay = gameObject.GetComponent<Text> ();
		resetScore();
	}
	
	// Update is called once per frame
	void Update () {
		EnemyBehavior enemy = GameObject.FindObjectOfType<EnemyBehavior> ();
		if (!enemy) {
			//No enemy left
			resetScore();
		}
	}

	public void gainScore(int points) {
		score += points;
		scoreDisplay.text = score.ToString();
	}

	void resetScore() {
		score = 0;
		scoreDisplay.text = score.ToString();
	}
}
