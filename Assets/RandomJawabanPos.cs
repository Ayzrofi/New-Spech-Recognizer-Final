using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomJawabanPos : MonoBehaviour {

    public Text[] ClueText;
    public List<string> ClueJawaban;

    private void Start()
    {
        InitializeClue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            InitializeClue();
    }

    public void InitializeClue()
    {
        ClueJawaban.Add(LevelString.AnswerThisLevel);
        ClueJawaban.Add(LevelString.wrongAnswer[0]);
        ClueJawaban.Add(LevelString.wrongAnswer[1]);

        StartCoroutine(TimerSetPosClue());
    }

    IEnumerator TimerSetPosClue()
    {
        yield return new WaitForSeconds(.1f);
        SetPosClue();
    }

    public void SetPosClue()
    {
        for (int i = 0; i < ClueText.Length; i++)
        {
            int rand = Random.Range(0, ClueJawaban.Count);

            ClueText[i].text = ClueJawaban[rand];
            ClueJawaban.RemoveAt(rand);
        }
    }
}
