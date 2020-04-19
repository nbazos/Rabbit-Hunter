using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    GameObject [] cameras;
    int currentCameraIndex = 0;

    public GameObject hunterShedPrefab;
    public GameObject rabbitDenPrefab;
    public GameObject rabbitSanctuaryPrefab;
    public GameObject rabbitPrefab;
    public GameObject hunterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SceneSetup();

        cameras = GameObject.FindGameObjectsWithTag("Camera");

        for (int i = 0; i < cameras.Length; i++)
        {
            if(i == 0)
            {
                cameras[i].SetActive(true);
            }
            else
            {
                cameras[i].SetActive(false);
            }
        }
    }

    private void SceneSetup()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraSwitch();
    }

    private void CheckCameraSwitch()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            cameras[currentCameraIndex].SetActive(false);

            currentCameraIndex++;

            if (currentCameraIndex >= cameras.Length)
            {
                currentCameraIndex = 0;
            }

            cameras[currentCameraIndex].SetActive(true);
        }
    }
}
