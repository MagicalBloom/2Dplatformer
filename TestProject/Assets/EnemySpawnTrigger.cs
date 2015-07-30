using UnityEngine;
using System.Collections;

public class EnemySpawnTrigger : MonoBehaviour {

	public GameObject[] EnemyPrefabs;
	public Vector3[] EnemyPositions;
	public Vector3[] EnemyRotations;

	private bool EnemySpawned = false;
	private Vector3 EnemyPosition;
	private Quaternion DefaultRotation = Quaternion.Euler(0, 0, 0);

	void OnTriggerEnter2D(){
		if(!EnemySpawned){
			for(int i = 0; i < EnemyPrefabs.Length; i++){
				Quaternion enemyRotation = Quaternion.Euler (EnemyRotations[i].x, EnemyRotations[i].y, EnemyRotations[i].z); 
				GameObject EnemyClone1 = Instantiate (EnemyPrefabs[i], EnemyPositions[i], enemyRotation) as GameObject;
				EnemyClone1.GetComponent<Enemy>().EnemyMoving = true;
			}
			EnemySpawned = true;
		}
	}
}
