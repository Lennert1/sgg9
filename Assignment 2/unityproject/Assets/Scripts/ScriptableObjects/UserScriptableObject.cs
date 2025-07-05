using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserScriptable", menuName = "ScriptableObjects/UserScriptable")]
public class UserScriptableObject : ScriptableObject
{
    public string username;
    public string level;
    public Sprite profilePic;
}
