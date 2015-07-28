using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
	// This class handles gui changes, scene changes and different events

	public static GameMaster gm;

	public void Start(){
		if(gm == null){
			gm = GetComponent<GameMaster>();
		}
	}

	public void Update(){

	}

	// Restart current scene
	public void RestartLevel() {
		Application.LoadLevel(Application.loadedLevel); // reload current scene
	}

	// Load next scene
	public void NextLevel(){
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	// Exit from the game
	public void ExitGame(){
		Application.Quit (); // quit the game
	}

	// Destroy the player gameobject and display the game over menu with DisplayGameOverMenu method
	public static void KillPlayer(Player player){
		Destroy (player.gameObject); // just delete the player for now
		GUIManager.guiManager.DisplayGameOverMenu ();
	}

	public static void KillEnemy(Enemy enemy){
		Destroy (enemy.gameObject); // just delete the enemy for now
	}	
}
