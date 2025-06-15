using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    private Card card;

    public void SelectCard()
    {
        BattleManager.Instance.PlayerChooseCard(123, card);
    }
}
