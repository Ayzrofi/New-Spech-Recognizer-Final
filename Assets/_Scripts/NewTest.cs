using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class NewTest : MonoBehaviour {
    [Header("Command List")]
    public List<string> key = new List<string>();
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
    //private KeywordRecognizer Recognizer;
    protected string word;
    private void Awake()
    {
        key.Add("ball");
    }
    private void Start()
    {
        if (key != null)
        {
            Recognizer = new KeywordRecognizer(key.ToArray(), confidence);

            Recognizer.OnPhraseRecognized += OnPhraseRecognized;
            Recognizer.Start();
        }
    }
    private void Update()
    {
        //if (key != null)
        //    Recognizer = new KeywordRecognizer(key.ToArray(), confidence);
    }
    void OnPhraseRecognized(PhraseRecognizedEventArgs Sound)
    {
        //StartCoroutine(UlangRecognitions(Sound.text));

        word = Sound.text;

        //// to Play The Game 
        //if (word == "play")
        //{
        //    StartCoroutine(playGame());
        //}
        //else
        //// for exiting application
        //if (word == "exit")
        //{
        //    Debug.Log("Quit Game");
        //    Application.Quit();
        //}
        // display your voice in game 
        Result.text = "You Say :<b> " + word + "</b> ";
    }// end of functions OnPhraseRecognized

    IEnumerator UlangRecognitions(string txt)
    {

        Recognizer.OnPhraseRecognized -= OnPhraseRecognized;
        Recognizer.Stop();
        yield return new WaitForSeconds(.5f);
        key.Add(txt);
        Recognizer = new KeywordRecognizer(key.ToArray(), confidence);
        Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        Recognizer.Start();
    }

    IEnumerator playGame()
    {
        anim.SetTrigger("end");
        audio.PlayOneShot(PlayClip);
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("Game_1");
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
