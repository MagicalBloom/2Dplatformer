﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public static GUIManager guiManager;

	public GameObject GameOverMenu;
	public GameObject PauseMenu;
	public GameObject ExitMenu;
	public GameObject OptionsMenu;
	public GameObject PlayerHealthbar;

	public Texture2D CustomCursor;

	Slider PlayerHealthbarSlider;


	void Start () {
		// Set cursor with a slight delay
		Invoke ("SetCustomCursor", 0.1f);

		if(guiManager == null){
			guiManager = GetComponent<GUIManager>();
		}
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
		GameOverMenu.SetActive (true); // reveal the game over menu
	}
	
	// Set new value to the healtbar
	public static void UpdatePlayerHealthbar(int newValue){
		guiManager.PlayerHealthbarSlider.value = newValue; 
	}
}