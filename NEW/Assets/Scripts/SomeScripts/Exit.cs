using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [Range(1, 300)]
    public int fpsRange;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fpsRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("TutorialLVL");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("Sandbox1");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene("Enemy scene");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("City");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("Sandbox2");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene("Sandbox3");
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("Sandbox4");
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SceneManager.LoadScene("Sandbox5");
        }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     SceneManager.LoadScene(1);
        //     //Application.LoadLevel(1);
        // }


    }
}
