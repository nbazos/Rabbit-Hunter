using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureRabbit : Action
{
    bool rabbitCaptured = false;

    // Initialize starting cost and preconditions/effects of this action
    public void Start()
    {
        AddActionPrecondition("rabbitFound", true);
        AddActionEffect("rabbitFound", false);
        AddActionEffect("huntRabbit", true);
        
        cost = 1.0f;
    }

    // Return boolean for this action to represent if it has been completed or not
    public override bool ActionCompleted()
    {
        return rabbitCaptured;
    }

    // Complete the necessary steps so this action can be done
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Rabbit");

        return target != null;
    }

    // Do the action itself (if range is required for this action then you are already at the necessary location)
    public override bool DoAction(GameObject agent)
    {
        // stop the rabbit from moving and attach it to the hunter 
        target.transform.GetComponent<Rabbit>().speed = 0.0f;
        target.transform.parent = transform;

        // stop the hunter from moving because the simulation is over since the rabbit got caught
        this.gameObject.GetComponent<Hunter>().speed = 0.0f;

        // set this action's boolean
        rabbitCaptured = true;

        return true;
    }

    // Is this action dependent on range?
    public override bool IsRangeBased()
    {
        return true;
    }

    // Reset this action's variables
    public override void Reset()
    {
        rabbitCaptured = false;
        target = null;
        SetInRange(false);
    }
}
