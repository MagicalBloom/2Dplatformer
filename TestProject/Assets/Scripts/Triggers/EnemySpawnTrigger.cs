using UnityEngine;
using System.Collections;

public class EnemySpawnTrigger : MonoBehaviour {

	public GameObject[] EnemyPrefabs;
	public Vector3[] EnemyPositions;
	public Vector3[] EnemyRotations;
	public float SpawnInterval = 0f;
	public float StartUpDelay = 0f;
	public float EnemyMovementDuration = 0f;

	private bool EnemySpawned = false;
	private Vector3 EnemyPosition;

	void OnTriggerEnter2D(Collider2D collider2D){
		if(!EnemySpawned && collider2D.tag == "Player"){
			StartCoroutine(SpawnEnemies());
			EnemySpawned = true;
		}
	}

	IEnumerator SpawnEnemies(){

		yield return new WaitForSeconds (StartUpDelay); // Start up delay

		for(int i = 0; i < EnemyPrefabs.Length; i++){
			yield return new WaitForSeconds(SpawnInterval); // Create the desired spawn delay

			// Spawn the enemies
			Quaternion enemyRotation = Quaternion.Euler (EnemyRotations[i].x, EnemyRotations[i].y, EnemyRotations[i].z); 
			GameObject EnemyClone1 = Instantiate (EnemyPrefabs[i], EnemyPositions[i], enemyRotation) as GameObject;
			EnemyClone1.GetComponent<Enemy>().MovementDuration = EnemyMovementDuration;
			EnemyClone1.GetComponent<Enemy>().EnemyMoving = true;
		}
	}
}
