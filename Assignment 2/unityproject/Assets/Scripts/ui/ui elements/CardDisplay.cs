using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    private Card card;
    private int info;
    private ICardSelector cardSelector;

    public void SelectCard()
    {
        if (cardSelector == null) return;
        cardSelector.SelectCard(info);
    }

    public void InitiateCardDisplay(Card card)
    {
        // do smth
    }

    public void InitiateSelectableCard(ICardSelector c, int info)
    {
        cardSelector = c;
        this.info = info;
    }
}
