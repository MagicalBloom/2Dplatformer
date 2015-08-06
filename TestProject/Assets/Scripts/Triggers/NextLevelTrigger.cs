using UnityEngine;
using System.Collections;

public class NextLevelTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider2D){
		if(collider2D.tag == "Player"){
			GameMaster.StaticNextLevel(); // Load next level
		}
	}

}
