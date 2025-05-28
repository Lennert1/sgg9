using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.Rotate(new Vector3(0, 1, 0));
    }

    public void changeColorGreen()
    {
        mat.color = Color.green;
    }
    public void changeColorRed()
    {
        mat.color = Color.red;
    }
     public void changeColorBlue()
    {
        mat.color = Color.blue;
    }
}
