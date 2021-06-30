using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Time.time * rotateSpeed);

    }
}
