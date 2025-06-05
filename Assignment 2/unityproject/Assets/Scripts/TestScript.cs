using System;
using System.Collections;
using System.Collections.Generic;
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

        User t = new User(1234, "Pony", new List<Card>() {new Card(3,1,1), new Card(17,1,4), new Card(4,3,1)});
        usr = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(t));
    }
    void Update()
    {
        obj.transform.Rotate(new Vector3(0, 1, 0));
    }

    public void changeColorGreen()
    {
        mat.color = Color.green;
        RestServerCaller.Instance.GenericSendCall("api/change_color/", greenColor,null);
    }
    public void changeColorRed()
    {
        mat.color = Color.red;
        RestServerCaller.Instance.GenericSendCall("api/change_color/", redColor,null);
    }
     public void changeColorBlue()
    {
        mat.color = Color.blue;
        RestServerCaller.Instance.GenericSendCall("api/change_color/", blueColor,null);
    }
}
