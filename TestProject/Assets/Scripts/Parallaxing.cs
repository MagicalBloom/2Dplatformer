using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] Backgrounds; // List of images to be parallaxed
	private float[] ParallaxAmount; // Effect amount
	public float Smoothing = 1f;

	private Transform CurrentCameraPosition;
	private Vector3 PreviousCameraPosition;
	

	void Awake(){
		CurrentCameraPosition = Camera.main.transform;
	}


	void Start () {
		PreviousCameraPosition = CurrentCameraPosition.position;
		ParallaxAmount = new float[Backgrounds.Length];

		// Assign parrallax amount to each background
		for(int i = 0; i < Backgrounds.Length; i++){
			ParallaxAmount[i] = Backgrounds[i].position.z * -1;
		}
	}


	void Update () {
		for(int i = 0; i < Backgrounds.Length; i++){
			// Calculate parallax
			float parallax = (PreviousCameraPosition.x - CurrentCameraPosition.position.x) * ParallaxAmount[i];

			// Add parallax
			float backgroundTargetPositionX = Backgrounds[i].position.x + parallax;

			//  Calculate new position for background
			Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, Backgrounds[i].position.y, Backgrounds[i].position.z);

			// Fade between positions
			Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, backgroundTargetPosition, Smoothing * Time.deltaTime);
		}

		// Set previous camera position
		PreviousCameraPosition = CurrentCameraPosition.position;
	}
}
