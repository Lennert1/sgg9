using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    void Awake()
    {
        if (gameManager != null) Debug.LogError("Multiple Instances of GameManager!");
        else gameManager = this;
    } 

    public static GameManager GetGameManager() {
        return gameManager;
    }
}
