using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelString : MonoBehaviour {
    [Header("Correct Answer")]
    public string answer;
    public static string AnswerThisLevel;
    [Header("Wrong Answer")]
    public string[] Wrong;
    public static string[] wrongAnswer ;

    private void Awake()
    {
        AnswerThisLevel = answer;
        wrongAnswer = Wrong;
        Debug.Log(AnswerThisLevel);
       
    }
}
