using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManajer : MonoBehaviour {

    public Text FinalHighScoreText;
    [Header("Transitions Animation")]
    public Animator anim;
    [Header("Sound ")]
    public AudioSource audio;
    public AudioClip PlayClip;

    int alphabetScore, animalScore, fruitScore, numberScore, objectScore, FinalHighScore;

    public void Start()
    {
        GetHighScore();
        FinalHighScoreText.text = FinalHighScore.ToString();
    }

    public void StartGame()
    {
        StartCoroutine(playGame());
    }

    public void StartLearn()
    {
        StartCoroutine(Learning());
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit The Game");
    }

    IEnumerator playGame()
    {
        anim.SetTrigger("end");
        audio.PlayOneShot(PlayClip);
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("SelectGame");
        //SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 3;
        //SceneController.TheInstanceOfSceneController.LoadNewScene();

        //Destroy(this.gameObject);
    }

    IEnumerator Learning()
    {
        anim.SetTrigger("end");
        audio.PlayOneShot(PlayClip);
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("LearningMenu");
        //SceneController.TheInstanceOfSceneController.WhatSceneToLoad = 3;
        //SceneController.TheInstanceOfSceneController.LoadNewScene();

        //Destroy(this.gameObject);
    }

    public void GetHighScore()
    {
        alphabetScore = PlayerPrefs.GetInt("Alphabet");
        animalScore = PlayerPrefs.GetInt("Animal");
        fruitScore = PlayerPrefs.GetInt("Fruit");
        numberScore = PlayerPrefs.GetInt("Number");
        objectScore = PlayerPrefs.GetInt("Object");

        Debug.Log(alphabetScore + " " + animalScore + " " + fruitScore + " " + numberScore + " " + objectScore);

        FinalHighScore = alphabetScore + animalScore + fruitScore + numberScore + objectScore;
    }
}
