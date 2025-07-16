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

    public void InitiateCardDisplay(Card card, bool hideCount = false, bool hideLVL = false, bool displayAsEnemy = false)
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
                case Modifier.DamageMultiplier: text += $"x " + (displayAsEnemy ? "???" : (c.modifierValue * 100)) + "% Dmg\n"; break;
                default: break;
            }
        }
        if (c.GetHealing(card.lvl) != 0)
        {
            text += "     + " + (displayAsEnemy ? "???" : c.GetHealing(card.lvl)) + " HP\n";
            infoText.color = new Color(0.8f, 0, 0);
        }
        if (c.GetDamage(card.lvl) != 0) text += "+ " + (displayAsEnemy ? "???" : c.GetDamage(card.lvl)) + " Dmg\n";
        if (c.GetShield(card.lvl) != 0) text += "+ " + (displayAsEnemy ? "???" : c.GetShield(card.lvl)) + " Shield\n";
        infoText.text = text;

        countText.text = (card.count != 1 && !hideCount) ? $"x{card.count}" : "";
        countText.color = card.count > 0 ? new Color(1f, 0.9f, 0.5f) : new Color(1, 0, 0);

        lvlText.text = hideLVL ? "" : $"LVL {card.lvl}";
        lvlText.color = lvlColorGradient.Evaluate((float)card.lvl / 24);
    }

    public void InitiateSelectableCard(List<ICardSelector> c, int info)
    {
        cardSelector = c;
        this.info = info;
    }
}
