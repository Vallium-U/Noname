using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OffScript : MonoBehaviour
{
    public GameObject GrapplingGun;

    public TextMeshPro TextMesh;
    // Start is called before the first frame update
    public float sec = 2.5f;

    void Awake() //or any event
    {
        Invoke("ShowTextTap", sec);//invoke after 5 seconds
    }
    void ShowTextTap()
    {
        TextMesh.gameObject.SetActive(true);
        //then remove it
        Invoke("DisableTextTap", sec);
    }
    void DisableTextTap()
    {
        TextMesh.gameObject.SetActive(false);
    }
    
    /*private void Start()
    {
        TextMesh = gameObject.GetComponent<TextMeshPro>();
    }

    void Awake(){
        if (TextMesh.gameObject.activeInHierarchy)
            TextMesh.alpha = 1;
 
        StartCoroutine(LateCall());
    }

    private void Update()
    {
        if (GrapplingGun.activeSelf==true)
        {
           // Turn();
           TextMesh.alpha = 1;
        }
        else  
            TextMesh.enabled = true;
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            TextMesh.alpha = 1;
        }
    }

    IEnumerator LateCall()
    {
 
        yield return new WaitForSecondsRealtime(sec);
 
        TextMesh.alpha = 0;
        yield return new WaitForSecondsRealtime(2*sec);
        //Do Function here...
    }*/
}
