using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
	// This class handles gui changes, scene changes and different events

	public static GameMaster gm;

	public GameObject GameOverMenu;
	public GameObject PauseMenu;
	public GameObject ExitMenu;
	public GameObject OptionsMenu;
	public GameObject PlayerHealthbar;
	Slider PlayerHealthbarSlider;

	public void Start(){
		if(gm == null){
			gm = GameObject.Find("GM").GetComponent<GameMaster>();
		}
		PlayerHealthbarSlider = PlayerHealthbar.GetComponent<Slider>();
	}

	public void Update(){
		// Display pause menu and pause the game
		if(Input.GetKeyDown(KeyCode.Escape)){
			Time.timeScale = 0; // This is probably a bad idea but it will do for now
			PauseMenu.SetActive(true); // Need to do toggle
		}
	}

	public void RestartLevel() {
		Application.LoadLevel(Application.loadedLevel); // reload current scene
	}

	public void ExitGame(){
		Application.Quit (); // quit the game
	}

	public void DisplayExitMenu(){
		PauseMenu.SetActive (false);
		ExitMenu.SetActive (true);
	}

	public void ExitMenuNo() {
		ExitMenu.SetActive (false);
		PauseMenu.SetActive (true);
	}

	public void ClosePauseMenu(){
		PauseMenu.SetActive (false);
		Time.timeScale = 1f;
	}

	public void DisplayGameOverMenu(){
		GameOverMenu.SetActive (true); // reveal the game over menu
	}

	public static void UpdatePlayerHealthbar(int newValue){
		gm.PlayerHealthbarSlider.value = newValue; 
	}

	public static void KillPlayer(Player player){
		Destroy (player.gameObject); // just delete the player for now
		gm.DisplayGameOverMenu ();
	}
}
