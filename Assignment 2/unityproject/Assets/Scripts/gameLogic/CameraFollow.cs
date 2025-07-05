using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTargetTransform;

    Vector3 lastPlayerPosition;

    private void Start()
    {
        Camera.main.transform.position = new Vector3(1.525879e-05f, 114.6f, -67.3f);
        Camera.main.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        Camera.main.orthographic = false;

        if (playerTargetTransform == null) {
            Debug.Log("No player!");
            return;
        
        };

        lastPlayerPosition = playerTargetTransform.position;
    }
    void LateUpdate()
    {
        if (playerTargetTransform == null) return;

        Vector3 playerMovement = playerTargetTransform.position - lastPlayerPosition;

        Camera.main.transform.position += playerMovement;
        Camera.main.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        lastPlayerPosition = playerTargetTransform.position;
    }
}
