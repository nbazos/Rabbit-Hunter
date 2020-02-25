using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    public bool processingInterruption = false;
    public bool rabbitSeen = false;

    public void Start()
    {
        speed = 1.0f;
    }

    public void Update()
    {
        // Accounting for interruptions in the "Move To" state depending if the rabbit is not hidden
        if (!processingInterruption)
        {
            if(GameObject.FindGameObjectWithTag("Rabbit") != null)
            {
                rabbitSeen = true;
            }
            else
            {
                rabbitSeen = false;
            }
        }
    }

    // Set the goal of this specific actor
    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            { "huntRabbit", true }
        };

        return goal;
    }
}
