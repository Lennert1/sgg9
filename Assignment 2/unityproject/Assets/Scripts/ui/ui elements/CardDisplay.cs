using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    private Card card;
    private int info;
    private List<ICardSelector> cardSelector;

    public void SelectCard()
    {
        if (cardSelector == null) return;
        foreach (ICardSelector c in cardSelector)
        {
            c.SelectCard(info);
        }
    }

    public void InitiateCardDisplay(Card card)
    {
        // display correct image etc
    }

    public void InitiateSelectableCard(List<ICardSelector> c, int info)
    {
        cardSelector = c;
        this.info = info;
    }
}
