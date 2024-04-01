using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverCanvas;

    public static UIManager instance;

    private int currentScore;

    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (instance == null)
        {
            instance = this; // Set the instance to this object if it's null
            DontDestroyOnLoad(gameObject); // Optional: Keep GameManager alive between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        gameOverCanvas.SetActive(false);
        currentScore = 0;
        scoreText.text = "Score: 0";
    }

    public void SetScore(int numCollected)
    {
        currentScore += numCollected;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void SetGameOver()
    {
        gameOverCanvas.SetActive(true);
    }
}
