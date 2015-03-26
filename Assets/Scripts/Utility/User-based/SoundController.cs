using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KofTools;

public class SoundController : MonoBehaviour {

	AudioSource audioSource;
	int audioClip;
	bool playing;
	
	public static bool musicOn;
	public static bool effectsOn;
	static ArrayList effects;
	public static SoundController soundControl;
	public static float volume;
	
	static Vector3 position;
	
	// Use this for initialization
	void Start () {
		audioClip = Random.Range(0, 5);
		playing = false;
		effectsOn = false;
		musicOn = true;
		effects = new ArrayList();
		soundControl = this;
		volume = 0.3f;

		if (!this.GetComponent<AudioSource>()){
			this.gameObject.AddComponent<AudioSource>();
		}
		audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		position = this.transform.position;
		if (musicOn){
			if (!playing){
				try {
					//GameObject.Destroy(audio);
				} catch {
					
				}
				
				playing = true;
				audioClip += Random.Range(1, 3);
				
				if (audioClip  > 4){
					audioClip = 0;
				}
				
				if (audioClip == 0){
					audioSource.clip = (AudioClip)(Resources.Load("Sound/Music/Industrial Revolution"));
				} else if (audioClip == 1){
					audioSource.clip = (AudioClip)(Resources.Load("Sound/Music/Mechanolith"));
					//audio.Play();
				} else if (audioClip == 2){
					audioSource.clip = (AudioClip)(Resources.Load("Sound/Music/Final Count"));
					//audio.Play();
				} else if (audioClip == 3){
					audioSource.clip = (AudioClip)(Resources.Load("Sound/Music/Gloom Horizon"));
					//audio.Play();
				} else if (audioClip == 4){
					audioSource.clip = (AudioClip)(Resources.Load("Sound/Music/Redletter"));
					//audio.Play();
				}
			}
			playing = audioSource.isPlaying;
			updateSounds();
		}
		
		if (effectsOn){
			List<GameObject> tbd = new List<GameObject>();
			foreach (GameObject aud in effects){
				if (!aud.GetComponent("AudioSource").GetComponent<AudioSource>().isPlaying || aud.GetComponent("AudioSource").GetComponent<AudioSource>().mute){
					tbd.Add(aud);
					GameObject.Destroy(aud);
				}
			}
			
			foreach (GameObject aud in tbd){
				effects.Remove(aud);
			}
			
			tbd = null;
		}
	}
	
	public void updateSounds(){
		if (musicOn){
			try {
				GetComponent<AudioSource>().volume = volume;
				GetComponent<AudioSource>().mute = false;
			} catch {
				
			}
		} else {
			try {
				GetComponent<AudioSource>().mute = true;
			} catch {
				
			}
		}
		foreach (GameObject obj in effects){
			obj.GetComponent<AudioSource>().volume = volume + 0.2f;
		}
	}
	
	public static void playEffect(string effectName, bool playAnyway){
		if (effectsOn){
			bool soundPlaying = false;
			
			foreach (GameObject obj in effects){
				if (obj.name.Equals(effectName + "Player(Clone)")){
					soundPlaying = true;
				} else {
					//Debug.Log(obj.name);
				}
			}
			
			try {
				if (soundPlaying){
					if (playAnyway){
						GameObject obj = ((GameObject)AudioSource.Instantiate(Resources.Load("Sound/Effects/" + effectName + "Player")));
						obj.transform.position = position;
						obj.AddComponent<FollowCamera>();
						effects.Add(obj);
					}
				} else {
					GameObject obj = ((GameObject)AudioSource.Instantiate(Resources.Load("Sound/Effects/" + effectName + "Player")));
					obj.transform.position = position;
					obj.AddComponent<FollowCamera>();
					effects.Add(obj);
				}
			} catch {
				Debug.Log("Improper effect name: " + effectName);
			}
		}
	}
	
	public static GameObject playEffect(string effectName, bool playAnyway, bool returning){
		if (effectsOn){
			bool soundPlaying = false;
			
			foreach (GameObject obj in effects){
				if (obj.name.Equals(effectName + "Player(Clone)")){
					soundPlaying = true;
				} else {
					//Debug.Log(obj.name);
				}
			}
			
			try {
				if (soundPlaying){
					if (playAnyway){
						GameObject obj = ((GameObject)AudioSource.Instantiate(Resources.Load("Sound/Effects/" + effectName + "Player")));
						obj.transform.position = position;
						obj.AddComponent<FollowCamera>();
						effects.Add(obj);
						return obj;
					}
				} else {
					GameObject obj = ((GameObject)AudioSource.Instantiate(Resources.Load("Sound/Effects/" + effectName + "Player")));
					obj.transform.position = position;
					obj.AddComponent<FollowCamera>();
					effects.Add(obj);
					return obj;
				}
			} catch {
				Debug.Log("Improper effect name: " + effectName);
			}
		}
		
		return null;
		
	}
	
	public static void playEffect(string effectName, bool playAnyway, Vector3 pos){
		if (effectsOn){
			bool soundPlaying = false;
			
			foreach (GameObject obj in effects){
				if (obj.name.Equals(effectName + "Player(Clone)")){
					soundPlaying = true;
				} else {
					//Debug.Log(obj.name);
				}
			}
			
			try {
				if (soundPlaying){
					if (playAnyway){
						GameObject obj = ((GameObject)AudioSource.Instantiate(Resources.Load("Sound/Effects/" + effectName + "Player")));
						obj.transform.position = pos;
						effects.Add(obj);
					}
				} else {
					GameObject obj = ((GameObject)AudioSource.Instantiate(Resources.Load("Sound/Effects/" + effectName + "Player")));
					obj.transform.position = pos;
					effects.Add(obj);
				}
			} catch {
				Debug.Log("Improper effect name: " + effectName);
			}
		}
	}
}
