using UnityEngine;
using System.Collections;

public class Camera2DFollow : MonoBehaviour
{
	public static bool FreezeCamera = false;

    public Transform Target;
	public float PositionYOffset;
	public float MinXposition = 0f;

	private Vector3 velocity = Vector3.zero;
	

    // Use this for initialization
    private void Start()
    {
        transform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {
		// In case of the player Gameobject is destroyed
		if (Target == null)
			return;

		Vector3 newPosition = new Vector3 (Target.position.x, Target.position.y, transform.position.z);

		// Lock camera on y-axis and keep it from going back along x-axis
		if(MinXposition < newPosition.x){
			MinXposition = newPosition.x;
		}
		newPosition = new Vector3 (Mathf.Clamp(newPosition.x, MinXposition, Mathf.Infinity), newPosition.y + PositionYOffset, newPosition.z);

		if(!FreezeCamera){
			transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.2f); // I'm not sure if this is good or not. I kinda like it maybe? :D
		}
    }
}
