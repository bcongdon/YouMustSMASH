using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	AudioSource source;
	List<AudioClip> audioQueue;

	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
		audioQueue = new List<AudioClip> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying && audioQueue.ToArray().Length > 0) {
			source.clip = audioQueue[0];
			audioQueue.RemoveAt(0);
			source.Play();
		}
	}

	public void playSound(string sound){
		audioQueue.Add (Resources.Load (sound) as AudioClip);
	}
}
