using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptable", menuName = "ScriptableObjects/CardScriptable")]
public class CardScriptableObject : ScriptableObject
{
    public int id;
    public Sprite sprite;
    [Space]
    public int baseDamage;
    public int baseHealing;
    public int baseShield;
    [Space]
    public Modifier modifier;
    public float modifierValue = 1f;

    public int GetDamage(int lvl)
    {
        return (int) (baseDamage * Math.Pow(1.1, lvl - 1));
    }

    public int GetHealing(int lvl)
    {
        return (int) (baseHealing * Math.Pow(1.1, lvl - 1));
    }

    public int GetShield(int lvl)
    {
        return (int) (baseShield * Math.Pow(1.1, lvl - 1));
    }
}

public enum Modifier {
    None,
    DamageMultiplier
}
