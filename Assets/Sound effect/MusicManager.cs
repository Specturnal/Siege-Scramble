using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	[SerializeField]private float delay;
	[SerializeField]private AudioClip LoopClip;

	private AudioSource audios;

	private void Start(){
		audios = GetComponent<AudioSource> ();
		Invoke ("ChangeClip", delay);
	}

	private void ChangeClip(){
		audios.clip = LoopClip;
		audios.Play ();
		audios.loop = true;
	}
}
