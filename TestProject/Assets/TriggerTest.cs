using UnityEngine;
using System.Collections;

public class TriggerTest : MonoBehaviour {

	public GameObject EnemyRunInBackPrefab;
	public GameObject EnemyRunInFrontPrefab;

	private bool EnemySpawned = false;
	private Vector3 EnemyPosition;

	void OnTriggerEnter2D(){
		if(!EnemySpawned){
			// Enemy 1
			EnemyPosition = new Vector3 (0f, 6f, 0f);
			GameObject EnemyClone1 = Instantiate (EnemyRunInBackPrefab, EnemyPosition, EnemyRunInBackPrefab.transform.rotation) as GameObject;
			EnemyClone1.GetComponent<Enemy>().EnemyMoving = true;

			// Enemy 2
			EnemyPosition = new Vector3 (-2f, 6f, 0f);
			GameObject EnemyClone2 = Instantiate (EnemyRunInFrontPrefab, EnemyPosition, EnemyRunInFrontPrefab.transform.rotation) as GameObject;
			EnemyClone2.GetComponent<Enemy>().EnemyMoving = true;

			EnemySpawned = true;
		}
	}
}
