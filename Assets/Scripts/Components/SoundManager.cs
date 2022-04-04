using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour 
{
	public static SoundManager instance;

	public GameObject[] muteMusicBtns; 
	public GameObject[] unmuteMusicBtns; 

	public GameObject[] muteSoundEffectsBtns; 
	public GameObject[] unmuteSoundEffectsBtns;
	public GameObject SoundButtonNonVideo, tourSoundButtonNonVideo;
	public GameObject SoundButtonVideo, tourSoundButtonVideo;

	//[HideInInspector]
	public List<AudioSource> musicAudioSources = new List<AudioSource>();
	[HideInInspector]
	public List<AudioSource> soundEffectAudioSources = new List<AudioSource>();


	public AudioSource attachedAudioSource;
	public AudioSource SoundManagerSource;

	public AudioSource alarmsound, alarmClocksound;

	void Awake() 
	{
		attachedAudioSource = new GameObject ("SoundsEffectsAudioSource", new Type[]{ typeof(AudioSource) }).GetComponent<AudioSource> ();
		instance = this;
	}


	void Start()
	{
		//PlayerPrefs.DeleteAll();

		GetAllAudioSources();
		//CheckForMusicMuteStatus ();
		//CheckForSoundEffectsMuteStatus ();
		//attachedAudioSource.transform.parent = transform;

	}

    [Serializable]
	public class AudioClipLastPlayedTime
	{
		public AudioClip audioClip;
		public float lastPlayedTime;
	}

	void Update()
	{
		if (PlayerPrefs.GetInt("SoundOffOn") == 0)
		{
			SoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(false);
			SoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(true);
			SoundButtonVideo.transform.GetChild(2).gameObject.SetActive(false);
			SoundButtonVideo.transform.GetChild(3).gameObject.SetActive(true);

			tourSoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(false);
			tourSoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(true);
			tourSoundButtonVideo.transform.GetChild(2).gameObject.SetActive(false);
			tourSoundButtonVideo.transform.GetChild(3).gameObject.SetActive(true);
		}
		if (PlayerPrefs.GetInt("SoundOffOn") == 1)
		{
			SoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(true);
			SoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(false);
			SoundButtonVideo.transform.GetChild(2).gameObject.SetActive(true);
			SoundButtonVideo.transform.GetChild(3).gameObject.SetActive(false);

			tourSoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(true);
			tourSoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(false);
			tourSoundButtonVideo.transform.GetChild(2).gameObject.SetActive(true);
			tourSoundButtonVideo.transform.GetChild(3).gameObject.SetActive(false);

		}
	}


    public void CheckForMusicMuteStatus () 
	{
		if(PlayerPrefs.GetString("MusicMute") == "No")
		{
			UnmuteMusic ();
		}
		else
		{
			MuteMusic ();
		}
	}

	public void CheckForSoundEffectsMuteStatus () 
	{
		if(PlayerPrefs.GetString("SoundEffectsMute") == "No")
		{
			UnmuteSoundEffects ();
		}
		else
		{
			MuteSoundEffects ();
		}
	}


	public void MuteMusic () 
	{
		for (int i = 0; i < muteMusicBtns.Length; i++) 
		{
			muteMusicBtns[i].SetActive(false);
			unmuteMusicBtns[i].SetActive(true);
		}

		for (int i = 0; i < musicAudioSources.Count; i++) {musicAudioSources[i].mute = true;}
		PlayerPrefs.SetString ("MusicMute","Yes");
	}

	public void UnmuteMusic () 
	{
		for (int i = 0; i < muteMusicBtns.Length; i++) 
		{
			muteMusicBtns[i].SetActive(true);
			unmuteMusicBtns[i].SetActive(false);
		}

		for (int i = 0; i < musicAudioSources.Count; i++) {musicAudioSources[i].mute = false;}
		PlayerPrefs.SetString ("MusicMute","No");
	}

	public void MuteSoundEffects () 
	{
		for (int i = 0; i < muteSoundEffectsBtns.Length; i++) 
		{
			muteSoundEffectsBtns[i].SetActive(false);
			unmuteSoundEffectsBtns[i].SetActive(true);
		}

		for (int i = 0; i < soundEffectAudioSources.Count; i++) {soundEffectAudioSources[i].mute = true;}
		PlayerPrefs.SetString ("SoundEffectsMute","Yes");
	}
	
	public void UnmuteSoundEffects () 
	{
		for (int i = 0; i < muteSoundEffectsBtns.Length; i++) 
		{
			muteSoundEffectsBtns[i].SetActive(true);
			unmuteSoundEffectsBtns[i].SetActive(false);
		}
		
		for (int i = 0; i < soundEffectAudioSources.Count; i++) {soundEffectAudioSources[i].mute = false;}
		PlayerPrefs.SetString ("SoundEffectsMute","No");
	}

	public void GetAllAudioSources () 
	{ 
		musicAudioSources.Clear ();
		soundEffectAudioSources.Clear ();
		GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName ("Poker").GetRootGameObjects ();
		for (int i = 0; i < rootGameObjects.Length; i++) 
		{
			AudioSource audioSource = rootGameObjects[i].transform.GetComponent<AudioSource>();
			if(audioSource != null)
			{
				if(audioSource.name.IndexOf("Music") > -1 || audioSource.name.IndexOf("music") > -1){musicAudioSources.Add(audioSource);}
				else{soundEffectAudioSources.Add(audioSource);}
			}
			GetAllAudioSourcesInChildren(rootGameObjects[i].transform);
		}
	}

	void GetAllAudioSourcesInChildren (Transform parent) 
	{ 
		for (int i = 0; i < parent.childCount; i++) 
		{
			AudioSource audioSource = parent.GetChild(i).GetComponent<AudioSource>();
			if(audioSource != null)
			{
				if(audioSource.name.IndexOf("Music") > -1 || audioSource.name.IndexOf("music") > -1){musicAudioSources.Add(audioSource);}
				else{soundEffectAudioSources.Add(audioSource);}
			}
			GetAllAudioSourcesInChildren(parent.GetChild(i));
		}
	}

	public void PlayPomeSound(AudioClip soundToPlay)
	{
		if (PlayerPrefs.GetInt("SoundOffOn") == 0)
		{
			attachedAudioSource.PlayOneShot(soundToPlay);
		}
	}

	public void PlaySound (AudioClip soundToPlay,float volume = 1) 
	{
		if(soundToPlay != null){StartCoroutine (PlayDelayedSoundCoroutine(soundToPlay,volume));}
	}

	public void PlayDelayedSound (AudioClip soundToPlay,float volume = 1,float delay = 0) 
	{
		StartCoroutine (PlayDelayedSoundCoroutine(soundToPlay,volume,delay));
	}

	private List<AudioClipLastPlayedTime> audioClipsLastPlayedTime = new List<AudioClipLastPlayedTime>();
	IEnumerator PlayDelayedSoundCoroutine (AudioClip soundToPlay,float volume = 1,float delay = 0) 
	{
		if(delay > 0){yield return new WaitForSeconds (delay);}

		bool audioFound = false;
		for (int i = 0; i < audioClipsLastPlayedTime.Count; i++) 
		{
			if(audioClipsLastPlayedTime[i].audioClip == soundToPlay)
			{
				audioFound = true;
				if((Time.realtimeSinceStartup - audioClipsLastPlayedTime[i].lastPlayedTime) > soundToPlay.length/4)
				{
					audioClipsLastPlayedTime[i].lastPlayedTime = Time.realtimeSinceStartup;
					attachedAudioSource.PlayOneShot (soundToPlay,volume);
					break;
				}
			}
		}

		if(!audioFound)
		{
			AudioClipLastPlayedTime audioClipLastPlayedTime = new AudioClipLastPlayedTime();
			audioClipLastPlayedTime.audioClip = soundToPlay;
			audioClipLastPlayedTime.lastPlayedTime = Time.realtimeSinceStartup;
			audioClipsLastPlayedTime.Add(audioClipLastPlayedTime);
			attachedAudioSource.PlayOneShot (soundToPlay,volume);
		}
	}


	public void PlayAudioSource(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play ();
        }
    }
	public void StopAudioSource(AudioSource audioSource)
    {
        audioSource.Stop ();
    }

	public void SoundOffOn()
	{
		print("SoundOffOn");
		if (PlayerPrefs.GetInt("SoundOffOn") == 0)
		{
			SoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(false);
			SoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(true);
			SoundButtonVideo.transform.GetChild(2).gameObject.SetActive(false);
			SoundButtonVideo.transform.GetChild(3).gameObject.SetActive(true);

			PlayerPrefs.SetInt("SoundOffOn", 1);
			print("VALUE: " + PlayerPrefs.GetInt("SoundOffOn"));

		}
		else if (PlayerPrefs.GetInt("SoundOffOn") == 1)
		{
			SoundButtonNonVideo.transform.GetChild(2).gameObject.SetActive(true);
			SoundButtonNonVideo.transform.GetChild(3).gameObject.SetActive(false);
			SoundButtonVideo.transform.GetChild(2).gameObject.SetActive(true);
			SoundButtonVideo.transform.GetChild(3).gameObject.SetActive(false);
			PlayerPrefs.SetInt("SoundOffOn", 0);
			print("VALUE2: " + PlayerPrefs.GetInt("SoundOffOn"));
		}
	}
}
