using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    public Text generationText;
    static int generationCount = 0;
    static int prevCameraIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Placement of necessary game objects
        SceneSetup();

        // Set the active cameras on start-up
        cameras = GameObject.FindGameObjectsWithTag("Camera");

        for (int i = 0; i < cameras.Length; i++)
        {
            if(i == prevCameraIndex)
            {
                cameras[i].SetActive(true);
            }
            else
            {
                cameras[i].SetActive(false);
            }
        }

        generationText.text = "Generation #: " + generationCount;
    }

    private void SceneSetup()
    {
        Vector3 terrainPos = Terrain.activeTerrain.transform.position;

        // Hunter shed
        Vector3 hunterShedPos = terrainPos + new Vector3(1, 0, 1);
        hunterShedPos.y = Terrain.activeTerrain.SampleHeight(new Vector3(hunterShedPos.x, 0, hunterShedPos.z));
        Instantiate(hunterShedPrefab, hunterShedPos, hunterShedPrefab.transform.rotation);

        // Rabbit den
        Vector3 rabbitDenPos = terrainPos + new Vector3(Terrain.activeTerrain.terrainData.size.x - 1, 0, Terrain.activeTerrain.terrainData.size.z - 1);
        rabbitDenPos.y = Terrain.activeTerrain.SampleHeight(new Vector3(rabbitDenPos.x, 0, rabbitDenPos.z));
        Instantiate(rabbitDenPrefab, rabbitDenPos, rabbitDenPrefab.transform.rotation);

        // Rabbit sanctuary
        Vector3 rabbitSanctuaryPos = terrainPos + new Vector3(Terrain.activeTerrain.terrainData.size.x - 1, 0, 1); ;
        rabbitSanctuaryPos.y = Terrain.activeTerrain.SampleHeight(new Vector3(rabbitSanctuaryPos.x, 0, rabbitSanctuaryPos.z));
        Instantiate(rabbitSanctuaryPrefab, rabbitSanctuaryPos, rabbitSanctuaryPrefab.transform.rotation);

        // Rabbit
        Instantiate(rabbitPrefab, rabbitDenPos, rabbitPrefab.transform.rotation);

        // Hunter
        Instantiate(hunterPrefab, hunterShedPos, hunterPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCameraSwitch();

        if(GameObject.FindGameObjectWithTag("Hunter") != null)
        {
            // Check if the rabbit was captured and re-load the scene as a new generation
            if (GameObject.FindGameObjectWithTag("Hunter").GetComponent<CaptureRabbit>().ActionCompleted())
            {
                prevCameraIndex = currentCameraIndex; // Don't change camera on scene reload
                generationCount++;
                SceneManager.LoadScene("Main");
            }
        }
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
