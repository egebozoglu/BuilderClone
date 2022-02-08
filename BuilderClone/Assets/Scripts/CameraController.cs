using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    float actualX;
    float actualZ;

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (!GameManager.instance.finished)
        {
            actualX = player.transform.position.x;
            actualZ = player.transform.position.z - 5f;
        }
        else
        {
            actualX = player.transform.position.x;
            actualZ = player.transform.position.z - 12f;
        }
        transform.position = new Vector3(actualX, transform.position.y, actualZ);
    }
}
