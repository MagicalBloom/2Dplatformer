using UnityEngine;
using System.Collections;

public class TriggerTest : MonoBehaviour {

	public GameObject EnemyRunInBackPrefab;
	public GameObject EnemyRunInFrontPrefab;
	public GameObject EnemyStillPrefab;

	private bool EnemySpawned = false;
	private Vector3 EnemyPosition;
	private Quaternion DefaultRotation = Quaternion.Euler(0, 0, 0);

	void OnTriggerEnter2D(){
		if(!EnemySpawned){
			// Enemy from back
			EnemyPosition = new Vector3 (3f, 6f, 0f);
			GameObject EnemyClone1 = Instantiate (EnemyRunInBackPrefab, EnemyPosition, DefaultRotation) as GameObject;
			EnemyClone1.GetComponent<Enemy>().EnemyMoving = true;

			// Enemy from front
			EnemyPosition = new Vector3 (-3f, 6f, 0f);
			GameObject EnemyClone2 = Instantiate (EnemyRunInFrontPrefab, EnemyPosition, DefaultRotation) as GameObject;
			EnemyClone2.GetComponent<Enemy>().EnemyMoving = true;

			// Enemy still
			EnemyPosition = new Vector3 (0f, 6f, 0f);
			GameObject EnemyClone3 = Instantiate (EnemyStillPrefab, EnemyPosition, DefaultRotation) as GameObject;
			EnemyClone3.GetComponent<Enemy>().EnemyMoving = true;

			EnemySpawned = true;
		}
	}
}
