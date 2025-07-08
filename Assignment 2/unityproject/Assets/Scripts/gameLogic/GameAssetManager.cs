using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetManager : MonoBehaviour
{
    private static GameAssetManager instance;
    public static GameAssetManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    #region assets

    [SerializeField] private List<CardScriptableObject> cards;

    #endregion

    #region methods
    void Awake()
    {
        if (Instance != null) Debug.LogError("Multiple Instances of GameAssetManager!");
        else Instance = this;

        for (int i = 0; i < cards.Count; i++) cards[i].id = i;
    }

    public CardScriptableObject ReadCard(int id)
    {
        if (id < cards.Count) return cards[id];
        else return cards[1];
    }

    #endregion
}
