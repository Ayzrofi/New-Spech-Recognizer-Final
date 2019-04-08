using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

    public Text FinalHighScoreText;
    public Text AlphabetHighScoreText;
    public Text AnimalHighScoreText;
    public Text FruitHighScoreText;
    public Text NumberHighScoreText;
    public Text ObjectHighScoreText;


    int alphabetScore, animalScore, fruitScore, numberScore, objectScore, FinalHighScore;

    private void Start()
    {
        InitHighScore();
    }

    public void InitHighScore()
    {
        GetHighScore();

        AlphabetHighScoreText.text = alphabetScore.ToString();
        AnimalHighScoreText.text = animalScore.ToString();
        FruitHighScoreText.text = fruitScore.ToString();
        NumberHighScoreText.text = numberScore.ToString();
        ObjectHighScoreText.text = objectScore.ToString();

        FinalHighScoreText.text = FinalHighScore.ToString();
    }

    public void GetHighScore()
    {
        alphabetScore = PlayerPrefs.GetInt("Alphabet");
        animalScore = PlayerPrefs.GetInt("Animal");
        fruitScore = PlayerPrefs.GetInt("Fruit");
        numberScore = PlayerPrefs.GetInt("Number");
        objectScore = PlayerPrefs.GetInt("Object");

        Debug.Log(alphabetScore +" "+ animalScore + " " + fruitScore + " " + numberScore + " " + objectScore);

        FinalHighScore = alphabetScore + animalScore + fruitScore + numberScore + objectScore;
    }
}
