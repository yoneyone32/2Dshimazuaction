using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject gameClearText;
    public Text scoreText;

    //SE
    public AudioClip gameClearSE;
    public AudioClip gameOverSE;
    AudioSource audioSource;

    const int MAX_SCORE = 9999;
    int score = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
    }
    public void AddScore(int value)
    {
        score += value;
        
        if(score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        scoreText.text = "SCORE:" + score.ToString();
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        audioSource.PlayOneShot(gameOverSE);
        Invoke("RestartScene", 1.5f);
    }
    public void GameClear()
    {
        gameClearText.SetActive(true);
        audioSource.PlayOneShot(gameClearSE);
        Invoke("RestartScene", 1.5f);
    }
    void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
