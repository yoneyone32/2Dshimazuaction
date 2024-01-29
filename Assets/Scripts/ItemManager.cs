using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private GameManager gameManager;
    [Header("アイテム獲得時に得るスコア")]public int score;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void GetItem()
    {
        Destroy(this.gameObject);
        gameManager.AddScore(score);
    }
}
