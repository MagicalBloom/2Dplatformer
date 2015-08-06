using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public static GUIManager guiManager;

	public GameObject GameOverMenu;
	public GameObject GameOverForRealMenu;
	public GameObject PauseMenu;
	public GameObject ExitMenu;
	public GameObject OptionsMenu;
	public GameObject PlayerHealthbar;
	public GameObject PlayerAmmo;
	public GameObject PlayerLives;

	public Text CanvasTextPlayer; // Place where we put our player dialogue
	public Text CanvasTextBoss; // Place where we put our boss dialogue

	public Texture2D CustomCursor;

	Slider PlayerHealthbarSlider;


	void Start () {

	}

	void Awake() {
		if(guiManager == null){
			guiManager = GetComponent<GUIManager>();
		}

		// Disable dialogue elements
		if(CanvasTextPlayer != null){
			CanvasTextPlayer.enabled = false;
		}
		if(CanvasTextBoss != null){
			CanvasTextBoss.enabled = false;
		}

		// Set cursor with a slight delay
		Invoke ("SetCustomCursor", 0.1f);
		
		PlayerHealthbarSlider = PlayerHealthbar.GetComponent<Slider>();
	}

	void Update () {
		// Display pause menu and pause the game
		if(Input.GetKeyDown(KeyCode.Escape)){
			Time.timeScale = 0; // This is probably a bad idea but it will do for now
			PauseMenu.SetActive(true); // Need to do toggle
		}
	}

	// Set custom cursor
	public void SetCustomCursor(){
		Vector2 hotspot = new Vector2 (CustomCursor.width / 2, CustomCursor.height / 2);
		Cursor.SetCursor (CustomCursor, hotspot, CursorMode.Auto);
		Debug.Log ("Custom cursor has been set");
	}

	// Display yes/no prompt of the pause menu and hide the pause menu
	public void DisplayExitMenu(){
		PauseMenu.SetActive (false);
		ExitMenu.SetActive (true);
	}
	
	// Hide yes/no prompt and display the pause menu
	public void ExitMenuNo() {
		ExitMenu.SetActive (false);
		PauseMenu.SetActive (true);
	}
	
	// Hide the pause menu
	public void ClosePauseMenu(){
		PauseMenu.SetActive (false);
		Time.timeScale = 1f;
	}
	
	// Display the game over menu
	public void DisplayGameOverMenu(){
		GameOverMenu.SetActive (true);
	}

	// Display the game over menu when there is no lives left
	public void DisplayGameOverForRealMenu(){
		GameOverForRealMenu.SetActive (true);
	}

	// Set new value to the healtbar
	public static void UpdatePlayerHealthbar(int newValue){
		guiManager.PlayerHealthbarSlider.value = newValue; 
	}

	// Set new valie to the ammo display
	public static void UpdatePlayerAmmo(int clipSize, int remainingAmmo){
		//Debug.Log ("aaa");
		guiManager.PlayerAmmo.transform.GetChild(0).GetComponent<Text> ().text = remainingAmmo + "/" + clipSize;
	}

	// Set new value to the player lives
	public static void UpdatePlayerLives(int newValue){
		//guiManager.PlayerLivesSlider.value = newValue;
		guiManager.PlayerLives.transform.GetChild(0).GetComponent<Text> ().text = "Lives: " + newValue;
	}
}
