using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptable", menuName = "ScriptableObjects/CardScriptable")]
public class CardScriptableObject : ScriptableObject
{
    public int id;
    public Sprite sprite;
    [Space]
    public int damage;
    public int healing;
    public int shield;
    [Space]
    public Modifier modifier;
    public float modifierValue = 1f;

    public int GetDamage(int lvl)
    {
        return damage * lvl;
    }

    public int GetHealing(int lvl)
    {
        return 1;
    }

    public int GetShield(int lvl)
    {
        return 1;
    }
}

public enum Modifier {
    None,
    DamageMultiplier
}
