using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioMixerGroup mainMixer;
    public SoundVariables[] sounds;
    Scene scene;



    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (SoundVariables s in sounds)
        {
            //Sets the variables of the audio sources in the gameobject
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = mainMixer;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.rolloffMode = AudioRolloffMode.Linear;

            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.spatialBlend = s.SpatialBlend;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        
        
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadVolume();
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == "BackgroundAmbience");
        SoundVariables trafficLight = Array.Find(sounds, SoundVariables => SoundVariables.name == "BackgroundTraffic");
        s.source.Play();
        trafficLight.source.Play();
        if(scene.buildIndex == 2)
        {
            Stop("Cyber");
            SoundVariables c2 = Array.Find(sounds, SoundVariables => SoundVariables.name == "C2");
            c2.source.Play();
        }
        else if(scene.buildIndex == 3)
        {
            Stop("C2");
            SoundVariables c3 = Array.Find(sounds, SoundVariables => SoundVariables.name == "C3");
            c3.source.Play();
        }
        else if(scene.buildIndex == 4)
        {
            Stop("C3");
            SoundVariables c4 = Array.Find(sounds, SoundVariables => SoundVariables.name == "BossMusic");
            c4.source.Play();
        }
    }

    //Plays a specified sound anywhere without needing a location
    public void PlaySoundSimplified(string SoundName)
    {
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == SoundName);

        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }



        s.source.Play();


    }

    //Stops all sounds running
    public void StopAllSounds()
    {
        foreach (SoundVariables s in sounds)
        {
            s.source.Stop();
        }
    }


    private void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat(SettingsController.MasterVolume, 1);



        //Loads the volume saved in the volume settings prefabs
        mainMixer.audioMixer.SetFloat(SettingsController.MasterVolume, Mathf.Log10(volume) * 20);

        
    }
    private void Start()
    {
        LoadVolume();
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == "BackgroundAmbience");
        s.source.Play();
    }
    public void Play(string name, Transform location, bool playAtPoint)
    {
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == name);
        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }

        if (playAtPoint)
        {
            //Plays audio at a specific location
            PlayAt(s.source, location.position);
        }
        else if (!playAtPoint)
        {

            s.source.Play();
        }

    }


    public void Stop(string name)
    {
        SoundVariables s = Array.Find(sounds, SoundVariables => SoundVariables.name == name);
        if (s == null)
        {
            Debug.Log(name + " Not Found");
            return;
        }
        //Stops music from playing
        s.source.Stop();
    }

    private AudioSource PlayAt(AudioSource audioSource, Vector3 audioLocation)
    {
        //Creates a temporary gameObject called TempAudio
        GameObject tempAudio = new GameObject("TempAudio");
        //sets its position to the value given
        tempAudio.transform.position = new Vector3(audioLocation.x, audioLocation.y, -10);
        //Adds a audio source
        AudioSource tempSource = tempAudio.AddComponent<AudioSource>();

        //Inputs the settings to the audio source
        tempSource.clip = audioSource.clip;
        tempSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

        tempSource.volume = audioSource.volume;
        tempSource.pitch = audioSource.pitch;

        tempSource.loop = audioSource.loop;
        tempSource.playOnAwake = audioSource.playOnAwake;
        tempSource.spatialBlend = audioSource.spatialBlend;
        tempSource.rolloffMode = AudioRolloffMode.Linear;
        tempSource.minDistance = audioSource.minDistance;
        tempSource.maxDistance = audioSource.maxDistance;

        tempSource.Play();

        //Destorys after the clip plays
        Destroy(tempAudio, tempSource.clip.length);

        return tempSource;
    }

    [System.Serializable]
    public class SoundVariables
    {
        //audio settings that can be changed in the array
        public string name;

        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;

        [Range(0.1f, 3f)]
        public float pitch;

        public bool loop;

        public bool playOnAwake;


        [Range(0f, 1f)]
        public float SpatialBlend;

        public float maxDistance;

        public float minDistance;

        [HideInInspector]
        public AudioSource source;


    }
}
