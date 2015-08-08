using UnityEngine;
using System.Collections;

public class RestrictCameraTrigger : MonoBehaviour {

	private bool AllEnemiesKilled = false;
	private bool PlayerEntered = false;
	private GameObject[] Enemies;
	private GameObject SingleEnemy;

	private float Timer;

	void OnTriggerEnter2D(Collider2D collider2D){
		if(collider2D.tag == "Player" && AllEnemiesKilled == false){
			Camera2DFollow.FreezeCamera = true;
			PlayerEntered = true;
		}
	}

	void Update(){
		if (PlayerEntered) {
			if(Timer > 7f){
				StartCoroutine (EnemiesVisible ());
			}
			else{
				Timer += Time.deltaTime;
			}
		}

		if(AllEnemiesKilled){
			Camera2DFollow.FreezeCamera = false;
		}
	}

	IEnumerator EnemiesVisible(){
		yield return new WaitForSeconds(2);
		Enemies = GameObject.FindGameObjectsWithTag ("enemy");

		AllEnemiesKilled = true;

		for(int i = 0; i < Enemies.Length; i++){
			SingleEnemy = Enemies[i].gameObject;
			if(SingleEnemy.GetComponentInChildren<SpriteRenderer>().isVisible){
				AllEnemiesKilled = false;
			}
		}
	}
}
