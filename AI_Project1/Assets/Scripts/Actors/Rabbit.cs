﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    public bool inDanger = false;
    public bool processingInterruption = false;

    private void Start()
    {
        // Rabbit is faster than hunter
        speed = 2.0f;
    }

    public void Update()
    {
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
