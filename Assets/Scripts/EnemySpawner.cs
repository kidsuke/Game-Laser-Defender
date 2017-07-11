using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float speed = 1f;
	public float width = 6f;
	public float height = 5f;
	public float padding = 0.5f;
	private float minX, maxX;
	private bool movingLeft = true;
	public float spawnDelay = 0.5f;
	
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(this.transform.position, new Vector3(width, height));
	}

	// Use this for initialization
	void Start () {
		spawnEnemiesUntilFull();
		createHorizontalGamespace();
	}
	
	// Update is called once per frame
	void Update () {
		moveFormation();
		if (areAllEnemiesDead()) {
			spawnEnemiesUntilFull();
		}
	}

	void moveFormation() {
		if (movingLeft) {
			this.transform.position += Vector3.left * (speed) * Time.deltaTime;
		} else {
			this.transform.position += Vector3.right * (speed) * Time.deltaTime;
		}
		if (this.transform.position.x <= minX) {
			movingLeft = false;
		} else if (this.transform.position.x >= maxX) {
			movingLeft = true;
		}
	}

	void createHorizontalGamespace() {
		float distanceToCamera = this.transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
		minX = leftMost.x + (this.transform.position.x - getTheLeftMostEnemyPosition().x) + padding;
		maxX = rightMost.x - (getTheRightMostEnemyPosition().x - this.transform.position.x) - padding;
	}

	Vector3 getTheLeftMostEnemyPosition() {
		Vector3 position = this.transform.position;
		foreach (Transform child in transform) {
			if (child.transform.position.x < position.x) {
				position = child.transform.position;
			}
		}
		return position;
	}

	Vector3 getTheRightMostEnemyPosition() {
		Vector3 position = this.transform.position;
		foreach (Transform child in transform) {
			if (child.transform.position.x > position.x) {
				position = child.transform.position;
			}
		}
		return position;
	}

	Transform nextFreePosition() {
		foreach(Transform child in this.transform) {
			if (child.childCount <= 0) {
				return child;
			}
		}
		return null;
	}

	bool areAllEnemiesDead() {
		foreach(Transform child in this.transform) {
			if (child.childCount > 0) {
				return false;
			}
		}
		return true;
	}

	void spawnEnemies() {
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	void spawnEnemiesUntilFull() {
		Transform freePosition = nextFreePosition();
		if (freePosition) {
			GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if (nextFreePosition()) {
			Invoke("spawnEnemiesUntilFull", spawnDelay);
		}
	}

}
