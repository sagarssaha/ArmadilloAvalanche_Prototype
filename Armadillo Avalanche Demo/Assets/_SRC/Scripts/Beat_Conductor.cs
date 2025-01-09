using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Beat_Conductor : MonoBehaviour
{
    //Conductor instance
    public static Beat_Conductor Instance;
    public AudioClip audioClip;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;


    //Loop System
    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    public bool inputAllowed;

    public UnityEvent OnBeat;
    public Image canvasBeatStatus;
    public TextMeshProUGUI accuracy_text_ui;

    public bool canStartSong;

    public void Start()
    {
        
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;


        //Place all notes before starting the music
        //if(notesHolder) notesHolder.PlaceNotes();
        //Start the music
        musicSource.clip = audioClip;
        musicSource.Play();

        canStartSong = true;
        //musicSource.Play();
    }

    public void Reset()
    {
        canStartSong = false;
        secPerBeat = secPerBeat = songPosition = songPositionInBeats = dspSongTime = completedLoops = 0;
        loopPositionInBeats = loopPositionInAnalog = 0;
        if(musicSource)
        {
            musicSource.Stop();
            musicSource.clip = null;
        } 
    }

    public int lastBeat;
    void Update()
    {
        //if(PauseSystem.IsPaused)
        //{
        //    if(musicSource.isPlaying) musicSource.Pause();
        //    return;
        //}
        //else if(musicSource && !musicSource.isPlaying) 

        if(!canStartSong) return;

        //Debug.Log(musicSource.time + " / " + musicSource.clip.length);
        if (musicSource && musicSource.clip && songPosition >= musicSource.clip.length)
        {
            // Reset starting variables
            songPosition = 0;
            completedLoops = 0;
            secPerBeat = 60f / songBpm;
            dspSongTime = (float)AudioSettings.dspTime;

           // LevelScrollerManager.Instance.rewindLevel = true;
            Debug.Log("Song restarted");
        }

        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = (songPosition / secPerBeat);

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;

        canvasBeatStatus.fillAmount = loopPositionInAnalog;
        inputAllowed = /*loopPositionInAnalog >= 0 &&*/ loopPositionInAnalog < secPerBeat;

        if (Mathf.FloorToInt(loopPositionInBeats) != lastBeat /*&& Mathf.FloorToInt(loopPositionInBeats) > -2 */)
        {
            lastBeat = Mathf.FloorToInt(loopPositionInBeats);
            if (Mathf.FloorToInt(loopPositionInBeats) >= 0)
            {
                print("Event on every Beat");
                //if (actionEventManager.eventActive)
                //{
                //    //actionEventManager.SpawnEnemy();
                //}

                OnBeat?.Invoke();
            }

            if (lastBeat == 0)
            {
                //print("Event on 1st Beat");
                // if (actionEventManager.eventActive)
                // {
                //     actionEventManager.SpawnEnemy();
                // }

            }
            //if (lastBeat == 1)
            //{
            //    print("Event on 2nd Beat");
            //}
            if (lastBeat == 2)
            {
                print("Event on 3rd Beat");
                //if (actionEventManager.eventActive)
                //{
                //    //actionEventManager.SpawnEnemy();
                //}
            }
            //if (lastBeat == 3)
            //{
            //    print("Event on 4th Beat");
            //    if (actionEventManager.eventActive)
            //    {
            //        actionEventManager.SpawnWave(3);
            //    }
            //}

        }
        //{
        //    print("Event per 1 Beat");
        //}
    }

    public void CheckInputAccuracy()
    {
        float accuracy = loopPositionInBeats - Mathf.FloorToInt(loopPositionInBeats);
        if(accuracy <= 0.3f || accuracy > 0.9f)
        {
            accuracy_text_ui.text = "Perfect!";
            accuracy_text_ui.color = Color.green;
        }
        else if(accuracy > 0.3f && accuracy < 0.6f)
        {
            accuracy_text_ui.text = "Good";
            accuracy_text_ui.color = Color.yellow;
        }
        else if(accuracy >= 0.6f && accuracy < 0.9f)
        {
            accuracy_text_ui.text = "Missed";
            accuracy_text_ui.color = Color.red;
        }
        print("Score:" + accuracy);
    }
}
