using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    public bool processingNewPlan = false;
    public bool rabbitSeen = false;

    public void Start()
    {
        speed = 1.0f;
    }

    public void Update()
    {
        if (!processingNewPlan)
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

    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            { "huntRabbit", true }
        };

        return goal;
    }
}
