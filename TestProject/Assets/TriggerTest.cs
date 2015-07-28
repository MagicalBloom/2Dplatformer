using UnityEngine;
using System.Collections;

public class TriggerTest : MonoBehaviour {

	public GameObject EnemyRunInBackPrefab;

	void OnTriggerEnter2D(){
		Vector3 EnemyPosition = new Vector3 (0f, 6f, 0f);
		Instantiate (EnemyRunInBackPrefab, EnemyPosition, EnemyRunInBackPrefab.transform.rotation);
		Debug.Log ("Enemy spawned");
	}
}
