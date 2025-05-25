using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    protected GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.GetGameManager();
    }
}
