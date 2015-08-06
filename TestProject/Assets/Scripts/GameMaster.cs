using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	private Player player;

	public void Start(){
		if(gm == null){
			gm = GetComponent<GameMaster>();
		}
		if(player == null){
			player = GameObject.Find("Player").GetComponent<Player>();
		}
	}

	// Restart current scene
	public void RestartLevel() {
		//Application.LoadLevel(Application.loadedLevel); // reload current scene
		AutoFade.LoadLevel (Application.loadedLevel, 2, 1, Color.black);
	}

	// Restart game from the beginning
	public void RestartGame (){
		AutoFade.LoadLevel (1, 2, 1, Color.black);

		// Reset lives
		PlayerPrefs.SetInt("PlayerLivesRemaining", player.playerStats.MaxLives);
	}

	// Load next scene
	public void NextLevel(){
		//Application.LoadLevel (Application.loadedLevel + 1);
		AutoFade.LoadLevel (Application.loadedLevel + 1, 2, 1, Color.black);
	}

	// Exit from the game
	public void ExitGame(){
		Application.Quit (); // quit the game
	}

	// This is kinda confusing way of doing this :D
	// Just to make this wait method available everywhere easily
	IEnumerator WaitSomeSeconds2(float seconds){
		yield return new WaitForSeconds(seconds);
	}
	public static void WaitSomeSeconds(float seconds){
		gm.WaitSomeSeconds2 (seconds);
	}

	// Destroy the player gameobject and display the game over menu with DisplayGameOverMenu method
	public static void KillPlayer(Player player){
		Destroy (player.gameObject); // just delete the player for now

		if (PlayerPrefs.GetInt ("PlayerLivesRemaining") <= 0) {
			GUIManager.guiManager.DisplayGameOverForRealMenu ();
		} else {
			GUIManager.guiManager.DisplayGameOverMenu ();
		}
	}

	public static void KillEnemy(Enemy enemy){
		Destroy (enemy.gameObject); // just delete the enemy for now
	}

	public static void KillBoss(Boss boss){
		Destroy (boss.gameObject); // just delete the boss for now
	}

	public static void StaticNextLevel(){
		gm.NextLevel ();
	}
}
