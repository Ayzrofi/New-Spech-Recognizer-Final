using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
[RequireComponent (typeof(AudioSource))]
public class MySpecRecognizer : MonoBehaviour {
    [Header("Timer Component")]
    public float timerCounter;
    public Slider TimerSlider;
    public Text TimerTextDisplay;

    [Header("Options Menu")]
    public GameObject OptionsMenuPanel;
    public GameObject MenuConfirmPanel;
    public GameObject ExitConfirmPanel;
    public bool InMenu, WantToExit, WantToMenu;

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

    private void Awake()
    {
        OptionsMenuPanel.gameObject.SetActive(false);
        MenuConfirmPanel.gameObject.SetActive(false);
        ExitConfirmPanel.gameObject.SetActive(false);
        InMenu = false;
        WantToExit = false;
        WantToMenu = false;

        TimerSlider.maxValue = timerCounter;
    }

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

    private void Update()
    {
        if (!InMenu && !LevelComplite)
        {
            if (timerCounter > 0)
            {
                timerCounter -= Time.deltaTime;
                TimerSlider.value = timerCounter;
                TimerTextDisplay.text = timerCounter.ToString("00");
            }

            if (timerCounter <= 0)
            {
                LevelComplite = true;
                InMenu = true;
                GameOver();
                Debug.Log("Lose");
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
        // open options menu
        if (word == "options" && !InMenu && !LevelComplite)
        {
            InMenu = true;
            OptionsMenuPanel.gameObject.SetActive(true);
            //StartCoroutine(playGame());
        }
        else
        // resume the game
        if (word == "resume" && InMenu && !LevelComplite)
        {
            InMenu = false;
            OptionsMenuPanel.gameObject.SetActive(false);
            //StartCoroutine(playGame());
        }
        else
        //  back to main menu 
        if (word == "menu" && InMenu && !WantToExit)
        {
            WantToMenu = true;
            MenuConfirmPanel.gameObject.SetActive(true);
           
        }
        else
        // for exiting application
        if (word == "exit" && InMenu && !WantToMenu)
        {
            WantToExit = true;
            ExitConfirmPanel.gameObject.SetActive(true);
            
        }
        else
        // yes confirm to exit / to menu
        if (word == "yes" && InMenu)
        {
            if (WantToMenu && !WantToExit)
            {
                Debug.Log("to Menu");
                StartCoroutine(ToMenu());
            }
      
            if (WantToExit && !WantToMenu)
            {
                Debug.Log("exit Game");
                StartCoroutine(QuitGame());
            }
            //WantToMenu = true;
            //MenuConfirmPanel.gameObject.SetActive(true);
            //StartCoroutine(ToMenu());
        }
        else
        // no confirm to exit / to menu
        if (word == "no" && InMenu)
        {
            if (WantToMenu && !WantToExit)
            {
                WantToMenu = false;
                WantToExit = false;
                MenuConfirmPanel.gameObject.SetActive(false);
            }

            if(WantToExit && !WantToMenu)
            {
                WantToExit = false;
                WantToMenu = false;
                ExitConfirmPanel.gameObject.SetActive(false);
            }
            
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
        if (word == LevelString.AnswerThisLevel && !InMenu)
        {
            if (LevelString.AnswerThisLevel != null && !LevelComplite)
            {
                LevelComplite = true;
                InMenu = true;
                AnswerTrue();
            }
                
        }
        // for check if the answer is false
        for (int i = 0; i < LevelString.wrongAnswer.Length; i++)
        {
            if (word == LevelString.wrongAnswer[i] && !InMenu)
            {
                if (LevelString.wrongAnswer[i] != null)
                    AnswerFalse();
            }
        }
        // display your voice in game 
        Result.text = "Your answer is :<b> " + word + "</b> ";
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
            InMenu = true;
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









