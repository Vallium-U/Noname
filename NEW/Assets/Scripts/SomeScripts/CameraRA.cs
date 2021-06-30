using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRA : MonoBehaviour
{

    public Transform player;

    void FixedUpdate()
    {
        transform.position = player.transform.position;
    }
    
}