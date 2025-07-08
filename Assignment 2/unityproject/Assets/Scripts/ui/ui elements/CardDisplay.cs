using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Card card;
    private int info;
    private List<ICardSelector> cardSelector;

    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private Gradient lvlColorGradient;

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
        CardScriptableObject c = GameAssetManager.Instance.ReadCard(card.type);
        GetComponent<Image>().sprite = c.sprite;

        if (c.id < 1)
        {
            infoText.text = "";
            countText.text = "";
            lvlText.text = "";
            return;
        }

        string text = "";
        if (c.modifier != Modifier.None)
        {
            switch (c.modifier)
            {
                case Modifier.DamageMultiplier: text += $"x {c.modifierValue * 100}% Dmg\n"; break;
                default: break;
            }
        }
        if (c.healing != 0)
        {
            text += $"   + {c.healing} HP\n";
            infoText.color = new Color(0.8f, 0, 0);
        }
        if (c.damage != 0) text += $"+ {c.damage} Dmg\n";
        if (c.shield != 0) text += $"+ {c.shield} Shield\n";
        infoText.text = text;

        countText.text = card.count != 1 ? $"x{card.count}" : "";
        countText.color = card.count > 0 ? new Color(1f, 0.9f, 0.5f) : new Color(1, 0, 0);

        lvlText.text = $"LVL {card.lvl}";
        lvlText.color = lvlColorGradient.Evaluate((float)card.lvl / 24);
    }

    public void InitiateSelectableCard(List<ICardSelector> c, int info)
    {
        cardSelector = c;
        this.info = info;
    }
}
