using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float health = 300f;
	public float speed = 5f;
	public float padding = 0.5f;
	private float minX;
	private float maxX;

	public GameObject projectile;
	public float projectileSpeed = 5f;
	public float firingRate = 0.2f;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		minX = leftMost.x + padding;
		maxX = rightMost.x - padding;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke();
		}

		movePlayers();
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log("player hit");
		Projectile projectile = col.gameObject.GetComponent<Projectile>();
		if (projectile) {
			getHit(projectile);
		}
	}

	void movePlayers() {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			this.transform.position += Vector3.left * (speed) * Time.deltaTime;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			this.transform.position += Vector3.right * (speed) * Time.deltaTime;
		}

		//Restrict the player to the gamespace
		float newX = Mathf.Clamp(transform.position.x, minX, maxX);
		this.transform.position = new Vector3(newX, this.transform.position.y, this.transform.position.z);
	}

	void getHit(Projectile projectile) {
		health -= projectile.getDamage();
		projectile.hit();
		if (health <= 0) {
			Destroy(gameObject);
		}
	}

	void fire() {
		Vector3 offset = new Vector3(0, 1, 0);
		GameObject playerProjectile = Instantiate(projectile, this.transform.position + offset, Quaternion.identity) as GameObject;
		playerProjectile.rigidbody2D.velocity = new Vector2(0f, projectileSpeed);
	}
}
