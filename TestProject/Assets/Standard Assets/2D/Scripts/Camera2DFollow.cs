using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform Target;
		public float PositionYOffset;
		public float MinXposition = 0f;
		

        // Use this for initialization
        private void Start()
        {
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
			// In case the player Gameobject is destroyed
			if (Target == null)
				return;

			Vector3 newPosition = new Vector3 (Target.position.x, Target.position.y, transform.position.z);;

			// Lock camera on y-axis and keep it from going back along x-axis
			if(MinXposition < newPosition.x){
				MinXposition = newPosition.x;
			}
			newPosition = new Vector3 (Mathf.Clamp(newPosition.x, MinXposition, Mathf.Infinity), newPosition.y + PositionYOffset, newPosition.z);

			transform.position = newPosition;

        }
    }
}
