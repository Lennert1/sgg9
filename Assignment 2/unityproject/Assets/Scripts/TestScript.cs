using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.Rotate(new Vector3(0, 1, 0));
    }

    void changeColorGreen()
    {
        obj.GetComponent<Material>().color = Color.green;
    }
    void changeColorRed()
    {
        obj.GetComponent<Material>().color = Color.red;
    }
    void changeColorBlue()
    {
        obj.GetComponent<Material>().color = Color.blue;
    }
}
