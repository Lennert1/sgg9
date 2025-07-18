using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FriendsUI : UI
{
    [SerializeField] ScrollView scrollView;
    [SerializeField] Transform friendsPanel;
    [SerializeField] TextMeshProUGUI noFriendsText;

    public override void SetActive(bool b)
    {
        base.SetActive(b);
        if (b)
        {
            User usr = GameManager.Instance.usrData;
            if (usr != null)
            {
                if (usr.friendsUID.Count <= 0)
                {
                    noFriendsText.text = "You have no friends yet";
                }
                else
                {
                    Debug.Log(usr.friendsUID.Count);
                    foreach (int friendID in usr.friendsUID)
                    {
                        noFriendsText.text = "";
                        User friend = GameManager.Instance.LoadUserData(friendID);
                        GameObject newItem = new GameObject("FriendItem");
                        newItem.transform.SetParent(friendsPanel.transform);

                        TextMeshProUGUI text = newItem.AddComponent<TextMeshProUGUI>();
                        text.text = "LVL " + friend.lvl + " " + friend.name;  
                        //text.text = friendID.ToString();
                        text.fontSize = 30;

                        // RectTransform anpassen
                        RectTransform rectTransform = newItem.GetComponent<RectTransform>();
                        rectTransform.localScale = Vector3.one;
                        rectTransform.sizeDelta = new Vector2(100, 30);
                    }
                }

            }
        }

    }

    
}
