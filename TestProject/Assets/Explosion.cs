using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	[System.Serializable]
	public class ExplosionStats{
		public float ExplosionSize;
		public int ExplosionDamage;
		public float ExplosionDelay;
		public float ExplosionSpeed;
		public float TroughWallsTime;
	}

	public ExplosionStats explosionStats = new ExplosionStats();
	public GameObject ExplosionEffect;
	public AudioClip ExplosionSoundEffect;
	
	private AudioSource audioSource;
	private Player player;
	private CircleCollider2D ExplosionRadius;
	private CircleCollider2D ParentCollider;
	private float CurrentExplosionRadius;
	private bool Explode = false;
	private bool Exploded = false;
	private float ColliderTimer;

	void Awake () {
		ExplosionRadius = GetComponent<CircleCollider2D> ();

		// Disable collider so it can pass trough the floor when boss trows them. (This isn't good if we want to use this class later for other explosives :/)
		ParentCollider = transform.parent.GetComponent<CircleCollider2D> ();
		ParentCollider.enabled = false;

		player = GameObject.Find ("Player").GetComponent<Player> ();
		audioSource = GameObject.Find ("AudioManager/EffectsAudio").GetComponent<AudioSource> ();
	}

	void Update () {
		explosionStats.ExplosionDelay -= Time.deltaTime;
		ColliderTimer += Time.deltaTime;

		// Enable the collider after defined amount of time
		if(ColliderTimer > explosionStats.TroughWallsTime && Explode == false){
			ParentCollider.enabled = true;
		}

		if(explosionStats.ExplosionDelay < 0){
			Explode = true;
		}
	}

	void FixedUpdate(){
		if(Explode){
			if(CurrentExplosionRadius < explosionStats.ExplosionSize){
				CurrentExplosionRadius += explosionStats.ExplosionSpeed;
			} else if(CurrentExplosionRadius >= explosionStats.ExplosionSize && Exploded == false) {
				Exploded = true;
				audioSource.PlayOneShot (ExplosionSoundEffect, 0.3f);
				GameObject clone = Instantiate(ExplosionEffect, transform.position, transform.rotation) as GameObject;
				clone.transform.SetParent(transform);
				Destroy(this.transform.parent.gameObject, 0.2f);
			}
			ExplosionRadius.radius = CurrentExplosionRadius;
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(Explode){
			if(collider.gameObject.GetComponent<Rigidbody2D>()){
				// Additional if statement for future uses
				if(collider.tag == "Player"){
					player.DamagePlayer(explosionStats.ExplosionDamage);
				}
			}
		}
	}
}
