using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health = 50;
	public GameObject projectile;
	public float projectileSpeed = 5f;
	public float shotsPerSeconds = 0.5f;
	private ScoreKeeper scoreKeeper;

	public AudioClip fireSound;
	public AudioClip destroySound;

	void Start() {
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper> ();
		if (!scoreKeeper) {
			Debug.LogError("No score keeper is found");
		}
	}

	void Update() {
		float probability = Time.deltaTime * shotsPerSeconds;
		if (Random.value < probability) {
			fire();
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile>();
		if (projectile) {
			getHit(projectile);
		}
	}

	void getHit(Projectile projectile) {
		health -= projectile.getDamage();
		projectile.hit();
		if (health <= 0) {
			Destroy(gameObject);
			AudioSource.PlayClipAtPoint(destroySound, gameObject.transform.position);
			scoreKeeper.gainScore(1);
		}
	}

	void fire() {
		Vector3 projectilePostition = this.transform.position + new Vector3 (0, -1, 0);
		GameObject enemyProjectile = Instantiate(projectile, projectilePostition, Quaternion.identity) as GameObject;
		AudioSource.PlayClipAtPoint(fireSound, projectilePostition);
		enemyProjectile.rigidbody2D.velocity = new Vector2(0f, -projectileSpeed);
	}
}
