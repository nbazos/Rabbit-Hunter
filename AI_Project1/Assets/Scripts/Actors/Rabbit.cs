using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    [HideInInspector] public bool inDanger = false;
    [HideInInspector] public bool processingInterruption = false;

    private void Start()
    {
        // Rabbit is slightly slower than hunter so that multiple generations can happen in the simulation
        speed = 1.75f;
    }

    public void Update()
    {
        StickToTerrain();

        // Accounting for interruptions in the "Move To" state depending on the Hunter's actions
        if (!processingInterruption)
        {
            GameObject hunter = GameObject.FindGameObjectWithTag("Hunter");

            if (hunter != null)
            {
                if (Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Hunter").transform.position) < 1.0f)
                {
                    inDanger = true;
                }
                else
                {
                    inDanger = false;
                }
            }
        }
    }

    // Set the goal of this specific actor
    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            {"stayAlive", true }
        };

        return goal;
    }
}
