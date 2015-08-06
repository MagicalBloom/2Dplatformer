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

	private GameObject PlayerObject;
	private GameObject BossObject;

	private bool DialogTriggered = false;

	void Awake(){
		if(DialogTextFile != null){
			// Split whole text to lines of text
			DialogTextLines = DialogTextFile.text.Split('\n');
		}

		CanvasTextPlayer.enabled = false;
		CanvasTextBoss.enabled = false;

		PlayerObject = GameObject.Find ("Player");
		BossObject = GameObject.Find ("EnemyBossTest2");

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
			PlayerObject.transform.GetComponentInChildren<Weapon>().enabled = false;

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
					PlayerObject.transform.GetComponentInChildren<Weapon>().enabled = true;

					// Disable texts
					CanvasTextPlayer.enabled = false;
					CanvasTextBoss.enabled = false;
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

			// Set the position of the text
			CanvasTextPlayer.rectTransform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y + 1.3f, PlayerObject.transform.position.z);

			// Find the tag in front of the line and remove it
			int firstBracket = DialogTextLines[CurrentLine].IndexOf("[");
			int secondBracket = DialogTextLines[CurrentLine].IndexOf("]");
			DialogTextLines[CurrentLine] = DialogTextLines[CurrentLine].Remove(firstBracket, secondBracket + 1);

			// Display current line and advance to the next line
			CanvasTextPlayer.text = DialogTextLines[CurrentLine];
			CurrentLine ++;
		}
		else {

			// Disable and enable canvas elements accordingly
			CanvasTextPlayer.enabled = false;
			CanvasTextBoss.enabled = true;

			// Set the position of the text
			CanvasTextBoss.rectTransform.position = new Vector3(BossObject.transform.position.x, BossObject.transform.position.y + 1.3f, BossObject.transform.position.z);

			// Find the tag in front of the line and remove it
			int firstBracket = DialogTextLines[CurrentLine].IndexOf("[");
			int secondBracket = DialogTextLines[CurrentLine].IndexOf("]");
			DialogTextLines[CurrentLine] = DialogTextLines[CurrentLine].Remove(firstBracket, secondBracket + 1);

			// Display current line and advance to the next line
			CanvasTextBoss.text = DialogTextLines[CurrentLine];
			CurrentLine ++;
		}
	}
}
