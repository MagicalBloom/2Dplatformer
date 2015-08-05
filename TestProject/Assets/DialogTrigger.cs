using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour {

	public Text CanvasTextPlayer; // Place where we put our player dialogue
	public Text CanvasTextBoss; // Place where we put our boss dialogue
	public TextAsset DialogTextFile; // File where we pull the text from

	private string[] DialogTextLines; // Array for storing the text lines
	private int LineCount = 0;
	private int CurrentLine = 0;

	private GameObject PlayerObject; // Player gameobject for disabling movement

	private bool DialogTriggered = false;

	void Awake(){
		if(DialogTextFile != null){
			// Split whole text to lines of text
			DialogTextLines = DialogTextFile.text.Split('\n');
		}

		CanvasTextPlayer.enabled = false;
		CanvasTextBoss.enabled = false;

		PlayerObject = GameObject.Find ("Player");

		LineCount = DialogTextLines.Length;
	}


	void OnTriggerEnter2D(Collider2D collider2D){
		// Do not do anything if the collided object was not the player
		if (collider2D.name != "Player")
			return;

		if(!DialogTriggered){
			// Disable player controls
			PlayerObject.GetComponent<Platformer2DUserControl> ().enabled = false;
			PlayerObject.GetComponent<PlatformerCharacter2D> ().enabled = false;

			DialogTriggered = true;

			// Show first line of dialogue
			DisplayDialogue();
		}
	}

	void Update(){

		// Check if player has triggered the event
		if(DialogTriggered){

			// Advance trough dialogue with space
			if(Input.GetKeyDown(KeyCode.Space)){
				if(CurrentLine < LineCount){
					DisplayDialogue();
				}
				else {
					// Enable player controls
					PlayerObject.GetComponent<Platformer2DUserControl> ().enabled = true;
					PlayerObject.GetComponent<PlatformerCharacter2D> ().enabled = true;
				}
			}
		}
	}

	void DisplayDialogue(){
		// Check whose line is going to be displayed next
		if(DialogTextLines[CurrentLine].Contains("[player]")){

			// Disable and enable canvas elements accordingly
			CanvasTextPlayer.enabled = true;
			CanvasTextBoss.enabled = false;

			// Display current line and advance to the next line
			CanvasTextPlayer.text = DialogTextLines[CurrentLine];
			CurrentLine ++;
		}
		else {

			// Disable and enable canvas elements accordingly
			CanvasTextPlayer.enabled = false;
			CanvasTextBoss.enabled = true;

			// Display current line and advance to the next line
			CanvasTextBoss.text = DialogTextLines[CurrentLine];
			CurrentLine ++;
		}
	}
}
