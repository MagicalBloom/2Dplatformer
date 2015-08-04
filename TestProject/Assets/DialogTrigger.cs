using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour {

	public Text CanvasText; // Place where we put our text
	public TextAsset DialogTextFile; // File where we pull the text from

	private string[] DialogTextLines; // Array for storing the text lines
	private int LineCount = 0;
	private int CurrentLine = 0;

	private GameObject PlayerObject; // Player gameobject for disabling movement

	private bool DialogTriggered = false;

	void Awake(){
		if(DialogTextFile != null){
			DialogTextLines = DialogTextFile.text.Split('\n');
		}

		CanvasText.enabled = false;

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
			CanvasText.enabled = true;
			CanvasText.text = DialogTextLines[CurrentLine];
		}
	}

	void Update(){

		// Check if player has triggered the event
		if(DialogTriggered){

			// Advance trough dialogue with space
			if(Input.GetKeyDown(KeyCode.Space)){
				if(CurrentLine < LineCount){
					CanvasText.text = DialogTextLines[CurrentLine];
					CurrentLine ++;
				}
				else {
					// Enable player controls
					PlayerObject.GetComponent<Platformer2DUserControl> ().enabled = true;
					PlayerObject.GetComponent<PlatformerCharacter2D> ().enabled = true;
				}
			}
		}
	}
}
