using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float sensivity = 100f;
    public Transform Playerr;
    public GameObject Player;
    float xRotation = 0f;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensivity * Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        Playerr.Rotate(Vector3.up * mouseX);

        //Player.transform.Rotate(0,Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensivity, 0);
        //transform.Rotate(-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensivity, 0, 0);
    }
    private void Update()
    {
        //FixedUpdate();
    }
    // Update is called once per frame
    //void Update()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
    //    float mouseY = Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

    //    xRotation -= mouseY;
    //    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    //    Playerr.Rotate(Vector3.up*mouseX);

    //    //Player.transform.Rotate(0,Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensivity, 0);
    //    //transform.Rotate(-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensivity, 0, 0);
    //}
}
