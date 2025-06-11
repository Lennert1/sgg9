using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gamelogic.ServerClasses;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    public Material mat;

    User usr;
    
    Dictionary<String, Object> greenColor = new Dictionary<String, Object>();
    Dictionary<String, Object> redColor = new Dictionary<String, Object>();
    Dictionary<String, Object> blueColor = new Dictionary<String, Object>();

    void Start()
    {
        greenColor.Add("color", 0);
        redColor.Add("color", 1);
        blueColor.Add("color", 2);

        User t = new User(1234, "Pony", new List<Card>() { new Card(3, 1, 1), new Card(17, 1, 4), new Card(4, 3, 1) }, new List<Character>());
        usr = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(t));

        RestServerCaller.Instance.GenericSendCall("api/login/", t);
    }
    void Update()
    {
        obj.transform.Rotate(new Vector3(0, 1, 0));
    }

    public void changeColorGreen()
    {
        
        RestServerCaller.Instance.GenericSendCall("api/change_color/", greenColor,null);
    }
    public void changeColorRed()
    {
        
        RestServerCaller.Instance.GenericSendCall("api/change_color/", redColor,null);
    }
     public void changeColorBlue()
    {
        
        RestServerCaller.Instance.GenericSendCall("api/change_color/", blueColor,null);
    }

    public void updateColor()
    {
        RestServerCaller.Instance.GenericRequestCall("api/update_color/",changecolor);
    }

    public void changecolor(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        Debug.Log(response);
        JsonConvert.DeserializeObject<Dictionary<String, Object>>(response.message)
            .TryGetValue("color", out var value);
        Debug.Log(value);
        switch (value)
        {
            case "0": mat.color = Color.green; break;
            case "1": mat.color = Color.red; break;
            case "2": mat.color = Color.blue; break;
            default: Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAh!"); break;
        }
        
    }
}
