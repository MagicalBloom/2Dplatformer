﻿using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private AudioSource audioSource;
	public AudioClip BackgroundMusicTitle;
	public AudioClip BackgroundMusicLevel1;
	public AudioClip BackgroundMusicLevel2;
	public AudioClip BackgroundMusicBoss;

	void Start(){
		// Play the title music
		if(Application.loadedLevel == 0){
			audioSource.clip = BackgroundMusicTitle;
			audioSource.volume = 0.6f;
			audioSource.Play();
		}
	}

	void Awake(){
		audioSource = GameObject.Find ("AudioManager/MusicAudio").GetComponent<AudioSource> ();
	}

	void Update(){
		/*
		if(Input.GetKeyDown(KeyCode.M)){
			audioSource.clip = BackgroundMusicTitle;
			audioSource.volume = 0.5f;
			audioSource.Play();
		}

		if(Input.GetKeyDown(KeyCode.K)){
			audioSource.clip = BackgroundMusicLevel1;
			audioSource.volume = 0.5f;
			audioSource.Play();
		}

		if(Input.GetKeyDown(KeyCode.N)){
			audioSource.Stop();
		}
		*/

	}

	void OnLevelWasLoaded(int level){
		switch (level) {
		case 1: // Level 1
			audioSource.clip = BackgroundMusicLevel1;
			audioSource.volume = 0.5f;
			audioSource.Play();
			break;
		case 2: // Level 2
			audioSource.clip = BackgroundMusicLevel2;
			audioSource.volume = 0.5f;
			audioSource.Play();
			break;
		case 3: // Boss
			break;
		}
	}

}