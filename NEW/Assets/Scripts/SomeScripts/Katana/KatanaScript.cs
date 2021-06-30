using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaScript : MonoBehaviour
{
    // Variables
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)){
            anim.SetBool("Attack", true);
        }else if (Input.GetKeyUp(KeyCode.B))
        {
            anim.SetBool("Attack", false);
        }
    }
}
