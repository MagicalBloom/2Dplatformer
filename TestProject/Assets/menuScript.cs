using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;

public class menuScript : MonoBehaviour 
{
	public Canvas QuitMenu;
	public Canvas ControlsMenu;
	public Canvas AboutMenu;
	public Button StartText;
	public Button ControlsText;
	public Button AboutText;
	public Button ExitText;
	
	void Start () {
		QuitMenu = QuitMenu.GetComponent<Canvas>();
		ControlsMenu = ControlsMenu.GetComponent<Canvas>();
		AboutMenu = AboutMenu.GetComponent<Canvas>();

		StartText = StartText.GetComponent<Button> ();
		ExitText = ExitText.GetComponent<Button> ();

		QuitMenu.enabled = false;
		ControlsMenu.enabled = false;
		AboutMenu.enabled = false;
	}
	
	public void ExitPress() { //this function will be used on our Exit button
		QuitMenu.enabled = true; //enable the Quit menu when we click the Exit button
		StartText.enabled = false; //then disable the Play and Exit buttons so they cannot be clicked
		ExitText.enabled = false;
	}

	public void ControlsPress() { //this function will be used on our Controls button _____muista addaa juttuja________
		ControlsMenu.enabled = true;
		StartText.enabled = false;
		ExitText.enabled = false;
	}

	public void AboutPress() { //this function will be used on our About button
		AboutMenu.enabled = true;
		StartText.enabled = false;
		ExitText.enabled = false;
	}

	public void OkPress() {
		QuitMenu.enabled = false;
		ControlsMenu.enabled = false;
		AboutMenu.enabled = false;
		StartText.enabled = true;
		ExitText.enabled = true;
	}
	
	public void NoPress() { //this function will be used for our "NO" button in our Quit Menu
		QuitMenu.enabled = false; //we'll disable the quit menu, meaning it won't be visible anymore
		StartText.enabled = true; //enable the Play and Exit buttons again so they can be clicked
		ExitText.enabled = true;
	}
	
	public void StartLevel () { //this function will be used on our Play button	
		//Application.LoadLevel ("level1"); //this will load our first level from our build settings. "1" is the second scene in our game
		AutoFade.LoadLevel ("level1", 2, 1, Color.black);
	}
	
	public void ExitGame () { //This function will be used on our "Yes" button in our Quit menu
		Application.Quit(); //this will quit our game. Note this will only work after building the game
	}
	
}