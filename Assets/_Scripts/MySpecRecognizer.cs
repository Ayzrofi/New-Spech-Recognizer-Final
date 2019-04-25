using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
[RequireComponent (typeof(AudioSource))]
public class MySpecRecognizer : MonoBehaviour {
    [Header("Transition Animation")]
    public Animator anim;
    public Animator PopUpAnim;
    public GameObject AllGameObj;
    [Header("Command List")]
    public string[] key;
    [Header("Level Speak Confidence")]
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    [Header("Text Result")]
    public Text Result;
    [Header("Player Component")]
    public int health;
    public Image[] HearthPlayer;
    public Text ScoreText,finalScoreWin,finalScoreLose;
    [Header("Other")]
    public bool LevelComplite = false;
    [Header("Sound Effect")]
    public AudioSource AudioSc;
    public AudioClip WinClip, LoseClip, TrueClip, falseClip;

    protected PhraseRecognizer Recognizer;
    protected string word;

    private void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("Animal"));
        ScoreText.text = "Score : "+ SceneController.MyScore.ToString();
        LevelComplite = false;
        if (key != null)
        {
            Recognizer = new KeywordRecognizer(key,confidence);
            Recognizer.OnPhraseRecognized += OnPhraseRecognized;
            Recognizer.Start();
        }
        // for setting health player UI Image
        for (int i = 0; i < HearthPlayer.Length; i++)
        {
            if (i < health)
            {
                HearthPlayer[i].enabled = true;
            }
            else
            {
                HearthPlayer[i].enabled = false;
            }
        }
    }

    //private void OnEnable()
    //{
    //    if (key != null)
    //    {
    //        Recognizer = new KeywordRecognizer(key, confidence);
    //        Recognizer.OnPhraseRecognized += OnPhraseRecognized;
    //        Recognizer.Start();
    //    }
    //}

    void OnPhraseRecognized(PhraseRecognizedEventArgs Sound)
    {
        word = Sound.text;
        // to Play The Game 
        if (word == "play")
        {
            //StartCoroutine(playGame());
        }
        else
        // for loading menu screen
        if (word == "menu")
        {
            StartCoroutine(ToMenu());
        }
        else
        // for exiting application
        if (word == "exit")
        {
            StartCoroutine(QuitGame());
        }
        else
        // to Load The Next Level
        if (word == "next" && LevelComplite)
        {
            StartCoroutine(NextLevel());
        }
        else
        // to Restart The game
        //if (word == "again")
        //{
        //    StartCoroutine(RestartGame());
        //}
        //else
        // for check the answer is true
        if (word == LevelString.AnswerThisLevel)
        {
            if (LevelString.AnswerThisLevel != null && !LevelComplite)
            {
                LevelComplite = true;
                AnswerTrue();
            }
                
        }
        // for check if the answer is false
        for (int i = 0; i < LevelString.wrongAnswer.Length; i++)
        {
            if (word == LevelString.wrongAnswer[i])
            {
                if (LevelString.wrongAnswer[i] != null)
                    AnswerFalse();
            }
        }
        // display your voice in game 
        Result.text = "You Say :<b> " + word + "</b> ";
    }// end of functions OnPhraseRecognized

    private void AnswerFalse()
    {
        health -= 1;
        AudioSc.PlayOneShot(falseClip);
        // for setting player Health UI
        for (int i = 0; i < HearthPlayer.Length; i++)
        {
            if(i < health)
            {
                HearthPlayer[i].enabled = true;
            }
            else
            {
                HearthPlayer[i].enabled = false;
            }
        }
       
        if ( health <= 0)
        {
            LevelComplite = true;
            GameOver();
        }
    }
    void GameOver()
    {
        AudioSc.PlayOneShot(LoseClip);
        Debug.Log("game Over");
        AllGameObj.SetActive(false);
        PopUpAnim.SetTrigger("lose");
        finalScoreLose.text = SceneController.MyScore.ToString();
    }
    private void AnswerTrue()
    {
        if (LevelComplite)
        {
            Debug.Log("correct");
            AudioSc.PlayOneShot(TrueClip);
            //scoreHolder.PlayerScore += 10;
            //if (scoreHolder.PlayerScore > PlayerPrefs.GetInt("HighScore"))
            //{
            //    PlayerPrefs.SetInt("HighScore",scoreHolder.PlayerScore);
            //}
            SceneController.MyScore += 10;
            PlayerPrefs.SetInt(SceneController.TheInstanceOfSceneController.JenisScene[SceneController.TheInstanceOfSceneController.WhatSceneToLoad].SceneNameType, SceneController.MyScore);

            ScoreText.text = "Score : " + SceneController.MyScore.ToString();
            finalScoreWin.text = SceneController.MyScore.ToString();
            PopUpAnim.SetTrigger("win");
            AllGameObj.SetActive(false);
        }
    }
    IEnumerator QuitGame()
    {
        AudioSc.PlayOneShot(WinClip);
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1.6f);
        Application.Quit();
    }
    IEnumerator RestartGame()
    {
        stopRecognizer();
        AudioSc.PlayOneShot(WinClip);
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1.6f);
        Application.LoadLevel(Application.loadedLevel);
        Destroy(this.gameObject);
    }
    IEnumerator NextLevel()
    {
        Debug.Log("next");
        stopRecognizer();
        AudioSc.PlayOneShot(WinClip);
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1.6f);
        //SceneManager.LoadScene(Application.loadedLevel + 1);
        SceneController.TheInstanceOfSceneController.LoadNewScene();
        Destroy(this.gameObject);
    }
    //IEnumerator playGame()
    //{
    //    stopRecognizer();
    //    anim.SetTrigger("end");
    //    yield return new WaitForSeconds(1.6f);
    //    SceneManager.LoadScene("Game_1");
    //    Destroy(this.gameObject);
    //}
    IEnumerator ToMenu()
    {
        stopRecognizer();
        AudioSc.PlayOneShot(WinClip);
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("MenuAwal");
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









