using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int CalculationOffsetX = 2; // To give calculations some time
	public bool HasRightTile = false;
	public bool HasLeftTile = false;
	public bool ReverseScale = false; // In case of non-repeatable sprites

	private Transform MyTransform;
	private float SpriteWidth = 0f;


	void Awake(){
		MyTransform = transform;
	}
	
	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		SpriteWidth = spriteRenderer.sprite.bounds.size.x;
	}

	void Update () {
		// Is tiling necessary?
		if(HasLeftTile == false || HasRightTile == false){
			// Calculate cameras extent (half of screen)
			float cameraHorizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

			// Calculate the x position where the camera can see the edge of the sprite
			float edgeVisiblePositionRight = (MyTransform.position.x + SpriteWidth / 2) - cameraHorizontalExtent;
			float edgeVisiblePositionLeft = (MyTransform.position.x - SpriteWidth / 2) + cameraHorizontalExtent;

			// Check if we see the edge of the sprite
			if(Camera.main.transform.position.x >= edgeVisiblePositionRight - CalculationOffsetX && HasRightTile == false){
				MakeNewTile(1);
				HasRightTile = true;
			} else if(Camera.main.transform.position.x <= edgeVisiblePositionLeft + CalculationOffsetX && HasLeftTile == false){
				MakeNewTile(-1);
				HasLeftTile = true;
			}
		}
	}

	void MakeNewTile(int direction){
		// Calculate position for new tile and instantiate a new tile
		Vector3 newPosition = new Vector3 (MyTransform.position.x + SpriteWidth * direction, MyTransform.position.y, MyTransform.position.z);
		Transform newTile = Instantiate (MyTransform, newPosition, MyTransform.rotation) as Transform;

		// Reverse the tile if it's needed
		if(ReverseScale){
			newTile.localScale = new Vector3(newTile.localScale.x * -1, newTile.localScale.y, newTile.localScale.z);
		}

		newTile.parent = MyTransform.parent;

		// Tell new tiles that they have tile next to them
		if (direction > 0) {
			newTile.GetComponent<Tiling> ().HasLeftTile = true;
		} else {
			newTile.GetComponent<Tiling> ().HasRightTile = true;
		}
	}
}
