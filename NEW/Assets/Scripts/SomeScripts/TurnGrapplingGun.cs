using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGrapplingGun : MonoBehaviour
{
    public GameObject GrapplingGun;
    bool statement;

    private void Start()
    {
        //statement = false;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            GrapplingGun.SetActive(true);
            statement = !statement;
        }
        else if (statement)
        {
            GrapplingGun.SetActive(false);
        }
    }
}
