using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "ScriptableObjects/EnemyScriptable")]
public class EnemyScriptableObject : ScriptableObject
{
    public int id;
    public Sprite sprite;
}
