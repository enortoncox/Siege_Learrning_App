using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//The SoundEvent contains all variables and methods to play an audioclip
[System.Serializable]
public class SoundEvent
{
    #region Variables
    //The name of the SoundEvent (The name is used to call this SoundEvent)
    public string name;

    [HideInInspector]
    //The AudioSource attached to the SoundEvent
    public AudioSource audioSource;

    //The AudioClip that this SoundEvent will use
    public AudioClip audioClip;

    //--FLOATS

    //The volume of the audioSource. It is limited to a range of 0f and 1F
    [Range(0f, 1f)]
    public float volume = 1f;

    //The ammount of delay that the sound will start with. It is limited to a range of 0f and 5F
    [Range(0f, 5f)]
    public float delayTime = 0f;

    //--BOOLS-- 

    //A bool to determine if a sound should have a delay
    public bool delay = false;

    //A bool to determine if a sound should loop
    public bool loop = false;

    //A bool to determine if a sound should be muted
    public bool mute = false;

    //A bool to determine if a sound should playOnAwake
    public bool playOnAwake = false;

    //A bool to determine if a sound has been called to play
    [HideInInspector]
    public bool playCalled = false;
    #endregion

    #region Methods
    //Called from the PlaySound method when a sound is required to play
    public void Play()
    {
        //Check if the SoundEvent has a clip attached
        if (audioClip != null)
        {
            //Set the PlayCalled bool to true once a sound has been asked to play
            playCalled = true;

            //Set the volume value of the audioSource equal to volume
            audioSource.volume = volume;

            //Set the loop value equal to the loop bool
            audioSource.loop = loop;

            //Set the mute value equal to the mute bool
            audioSource.mute = mute;

            //Set the audioSource's clip equal to audioClip
            audioSource.clip = audioClip;

            //If no delay is needed then play the clip
            if (delay == false)
            {               
                //Play the sound
                audioSource.Play();
            }

            //If a delay is needed then play the clip delayed
            if (delay == true)
            {
                //Play the sound with a delay
                audioSource.PlayDelayed(delayTime);
            }
        }
        //If the SoundEvent doesn't have a clip then send a LogWarning
        else 
        {
            Debug.LogWarning("SoundManager_Script: No audioClips attached to the " + name + " SoundEvent.");
        }
    }

    //Called from the StopSound method when a sound is required to stop
    public void Stop()
    {
        //Check is the audioSource is currently playing a sound
        if(audioSource.isPlaying)
        {
            //Stop the sound from playing
            audioSource.Stop();
        }
    }
    #endregion
}

//A class to manage all the different SoundEvents
public class SoundManager_Script : MonoBehaviour
{
    #region Variables
    //A static instance of the SoundManager_Script
    public static SoundManager_Script instance = null;

    //An array of all the soundEvents required
    [SerializeField]
    SoundEvent[] soundEvents;
    #endregion

    #region Methods
    //Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        //If the current instance is not null
        if(instance != null)
        {
            //If the current instance is not equal to this gameObject
            if(instance != this)
            {
                //Destroy this gameObject as an instance of the SoundManager_Script already exists
                Destroy(gameObject);
            }
        }
        //Else if the current instance is null
        else 
        {
            //Set the current instance equal to this gameObject
            instance = this;
        }

        //Initialise the SoundEvents
        InitialiseSoundEvents();
    }

    //Initialise the SoundEvents
    private void InitialiseSoundEvents()
    {
        //Search through all soundEvents, if they don't have an audioSource then create one
        for (int i = 0; i < soundEvents.Length; i++)
        {
            //Check to see if the SoundEvent has an audioSource
            if (soundEvents[i].audioSource == null)
            {
                //Create a gameObject with the name of the SoundEvent
                GameObject gameObject = new GameObject("SoundEvent" + "_" + soundEvents[i].name);

                //Set the new gameObject's parent equal to the Sound Manager gameObject
                gameObject.transform.SetParent(this.transform);

                //Add an AudioSource to the new gameObject and set it to the audioSource of the soundEvent
                soundEvents[i].audioSource = gameObject.AddComponent<AudioSource>();
            }

            //Check if the playOnAwake bool is set to true
            if (soundEvents[i].playOnAwake)
            {
                //Play the sound
                soundEvents[i].Play();
            }
        }
    }

    //Plays the soundEvent that is named
    public void PlaySound(string _name) 
    {
        //Searches through the soundEvents until one matching the name is found
        for(int i = 0; i < soundEvents.Length; i++)
        {
            //Check if the soundEvent has the same name
            if(soundEvents[i].name == _name)
            {
                //Play the sound
                soundEvents[i].Play();

                //Return out of the for loop
                return;
            }
        }
        //The named sound was not found
        Debug.LogWarning("SoundManager_Script: Sound not found in SoundEvents," + _name);
    }

    //Stops the soundEvent that is named
    public void StopSound(string _name)
    {
        //Searches through the soundEvents until one matching the name is found
        for (int i = 0; i < soundEvents.Length; i++)
        {
            //Check if the soundEvent has the same name
            if (soundEvents[i].name == _name)
            {
                //Stop the sound from playing
                soundEvents[i].Stop();

                //Return out of the for loop
                return;
            }
        }
        //The named sound was not found
        Debug.LogWarning("SoundManager_Script: Sound not found in SoundEvents," + _name);
    }
    #endregion
}
