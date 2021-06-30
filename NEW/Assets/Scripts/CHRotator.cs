using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHRotator : MonoBehaviour
{
    public float rotateSpeed;
    //CROSSHAIR
    public GameObject CH;
    public bool CanGrab;
    private float firingTimeStamp;
    private float accumulatedRecoil;
    private float recoveringRecoilTimeStamp;
    private const float exponentialAlpha = 0.8f;
    private float rPrevious;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Time.time * rotateSpeed);

    }
    public void CHMotion()
    {
        // gameObject.transform.position = (0, 0, Time.time * rotateSpeed);
    }
}
