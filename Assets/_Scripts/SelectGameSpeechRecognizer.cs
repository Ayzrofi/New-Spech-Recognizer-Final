using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SelectGameSpeechRecognizer : MonoBehaviour {
    public GameObject ExitConfirmPanel;
    public bool WantToExit;

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

        if (word == "alphabet")
        {
            SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 0;
            StartCoroutine(playGame());
        }
        else
       if (word == "animal")
        {
            SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 1;
            StartCoroutine(playGame());
        }
        else
         if (word == "fruit")
        {
            SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 2;
            StartCoroutine(playGame());
        }
        else
         if (word == "number")
        {
            SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 3;
            StartCoroutine(playGame());
        }
        else
        if (word == "object")
        {
            SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 4;
            StartCoroutine(playGame());
        }
        else
        // for exiting application
        //if (word == "exit")
        //{
        //    Debug.Log("Quit Game");
        //    Application.Quit();
        //}
        //else
        // for exiting application
        if (word == "exit" && !WantToExit)
        {
            WantToExit = true;
            ExitConfirmPanel.gameObject.SetActive(true);

        }
        else
        // yes confirm to exit / to menu
        if (word == "yes" && WantToExit)
        {
            Application.Quit();
            Debug.Log("Exit Game");
        }
        else
        if (word == "no" && WantToExit)
        {
            WantToExit = false;
            ExitConfirmPanel.SetActive(false);
        }
        // display your voice in game 
        Result.text = "You Choose The " + word + " Category ";
    }// end of functions OnPhraseRecognized
    IEnumerator playGame()
    {
        anim.SetTrigger("end");
        audio.PlayOneShot(PlayClip);
        yield return new WaitForSeconds(1.6f);
        //SceneManager.LoadScene("Game_1");
        //SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 3;
        SceneController.TheInstanceOfSceneController.LoadNewScene();

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