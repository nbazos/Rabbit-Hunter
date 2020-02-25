using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarrots : MonoBehaviour
{
    public GameObject ground;
    public GameObject carrotPrefab;

    Vector3 groundPos;
    Vector3 range;

    // Maximum carrots allowed at any one time
    readonly int maxCarrots = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Position of the plane in the world
        groundPos = ground.transform.position;

        // Accurate enough to represent bounds of the plane
        range = transform.localToWorldMatrix.lossyScale * 4.0f;
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
                Vector3 randomPos = new Vector3(Random.Range(-range.x, range.x), 0.1f, Random.Range(-range.z, range.z));

                Instantiate(carrotPrefab, groundPos + randomPos, carrotPrefab.transform.rotation);
            }
        }
    }
}
