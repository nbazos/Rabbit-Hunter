using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarrots : MonoBehaviour
{
    public GameObject carrotPrefab;

    Vector3 terrainPos;
    Vector3 terrainSize;

    const float TERRAIN_Y_OFFSET = 0.04f;

    // Maximum carrots allowed at any one time
    readonly int maxCarrots = 3;

    // Start is called before the first frame update
    void Start()
    {
        terrainSize = Terrain.activeTerrain.terrainData.size;
        terrainPos = Terrain.activeTerrain.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        int numCarrots = 0;

        // Iterate through all the carrots in the scene
        foreach (GameObject carrot in GameObject.FindGameObjectsWithTag("Carrot"))
        {
            // Only count the carrots which have not been stored by the rabbit
            if(carrot.transform.parent != GameObject.FindGameObjectWithTag("Rabbit Den").transform)
            {
                numCarrots++;
            }
        }
        
        // If there are less than 3 carrots in the scene, spawn more randomly so that there are three 
        if (numCarrots < 3)
        {
            for (int i = 0; i < (maxCarrots - numCarrots); i++)
            {
                float x = Random.Range(0, terrainSize.x);
                float z = Random.Range(0, terrainSize.z);
                float y = Terrain.activeTerrain.SampleHeight(terrainPos + new Vector3(x, 0, z)) + Terrain.activeTerrain.transform.position.y;

                Vector3 randomPos = new Vector3(x, y + TERRAIN_Y_OFFSET, z);

                Instantiate(carrotPrefab, terrainPos + randomPos, carrotPrefab.transform.rotation);
            }
        }
    }
}
