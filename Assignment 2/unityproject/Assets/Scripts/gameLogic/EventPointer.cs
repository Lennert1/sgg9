using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script is supposed to make the pointers on the map clickable and rotates the game object */
public class EventPointer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float rotSpeed = 50f;
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotatePointer();
        
    }


    /* Actual  function to make the pointer float and rotate effect */
    void RotatePointer()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude + 15f, transform.position.z);
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked");
    }
}
