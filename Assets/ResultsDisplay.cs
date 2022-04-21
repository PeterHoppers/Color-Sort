using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsDisplay : MonoBehaviour
{
    public GameObject winnerResults;
    public GameObject loseResults;
    public GameObject submitButton;
    // Start is called before the first frame update
    void Start()
    {
        winnerResults.SetActive(false);
        loseResults.SetActive(false);
        submitButton.SetActive(true);
    }

    public void DisplayWinResults()
    {
        winnerResults.SetActive(true);
        submitButton.SetActive(false);
    }

    public void DisplayLoseResults()
    {
        loseResults.SetActive(true);
        submitButton.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
