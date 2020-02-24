using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarrots : MonoBehaviour
{
    public GameObject ground;
    public GameObject carrotPrefab;

    Vector3 groundPos;
    Vector3 range;

    int maxCarrots = 3;

    // Start is called before the first frame update
    void Start()
    {
        groundPos = ground.transform.position;

        range = transform.localToWorldMatrix.lossyScale * 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        int numCarrots = 0;

        foreach (GameObject carrot in GameObject.FindGameObjectsWithTag("Carrot"))
        {
            if(carrot.transform.parent != GameObject.FindGameObjectWithTag("Rabbit Den").transform)
            {
                numCarrots++;
            }
        }

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
