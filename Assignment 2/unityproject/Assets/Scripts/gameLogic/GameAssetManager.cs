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
    [SerializeField] private CardScriptableObject defaultCard;

    [SerializeField] private List<EnemyScriptableObject> enemies;
    public List<EnemyScriptableObject> Enemies { get { return enemies; } }

    #endregion

    #region methods
    void Awake()
    {
        if (Instance != null) Debug.LogError("Multiple Instances of GameAssetManager!");
        else Instance = this;

        for (int i = 0; i < cards.Count; i++) cards[i].id = i;
        defaultCard.id = -1;

        for (int i = 0; i < enemies.Count; i++) enemies[i].id = i;
    }

    public CardScriptableObject ReadCard(int id)
    {
        if (id < cards.Count && id >= 0) return cards[id];
        else return defaultCard;
    }

    public List<Card> CreateInventoryOfAllCards()
    {
        List<Card> l = new();
        for (int i = 1; i < cards.Count; i++)
        {
            l.Add(new Card(i, 1, 1));
        }
        return l;
    }

    public EnemyScriptableObject ReadEnemy(int id)
    {
        if (id < enemies.Count && id >= 0) return enemies[id];
        else return null;
    }

    #endregion
}
