using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
[RequireComponent (typeof(AudioSource))]
public class MenuSpeak : MonoBehaviour
{
    [Header("Command List")]
    public string[] key;
    [Header("Level Speak Confidence")]
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    [Header("Text Result")]
    public Text Result;
    [Header("Transitions Animation")]
    public Animator anim;
    [Header("Sound ")]
    public AudioSource audio;
    public AudioClip PlayClip;

    protected PhraseRecognizer Recognizer;
    protected string word;

    private void Start()
    {
        if (key != null)
        {
            Recognizer = new KeywordRecognizer(key, confidence);
            Recognizer.OnPhraseRecognized += OnPhraseRecognized;
            Recognizer.Start();
        }
    }
    void OnPhraseRecognized(PhraseRecognizedEventArgs Sound)
    {
        word = Sound.text;
        // to Play The Game 
        if (word == "play")
        {
            StartCoroutine(playGame());
        }
        else
        // for exiting application
        if (word == "exit")
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
        // display your voice in game 
        Result.text = "You Say :<b> " + word + "</b> ";
    }// end of functions OnPhraseRecognized
    IEnumerator playGame()
    {
        anim.SetTrigger("end");
        audio.PlayOneShot(PlayClip);
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("SelectGame");
        //SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 3;
        //SceneController.TheInstanceOfSceneController.LoadNewScene();

        Destroy(this.gameObject);
    }
    // check if application quit
    private void OnApplicationQuit()
    {
        stopRecognizer();
    }
    // to Stop recognizer detection
    public void stopRecognizer()
    {
        if (Recognizer != null && Recognizer.IsRunning)
        {
            Recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            Recognizer.Stop();
        }
    }
}// end of Class









