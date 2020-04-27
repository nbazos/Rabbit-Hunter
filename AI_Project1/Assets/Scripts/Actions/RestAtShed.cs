using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAtShed : Action
{
    private bool restingAtShed = false;
    
    // Initialize starting cost and preconditions/effects of this action
    public void Start()
    {
        AddActionEffect("huntRabbit", true);
        cost = 3.0f;
    }

    // Return boolean for this action to represent if it has been completed or not
    public override bool ActionCompleted()
    {
        return restingAtShed;
    }

    // Complete the necessary steps so this action can be done
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Hunter Shed");

        return target != null;
    }

    // Do the action itself (if range is required for this action then you are already at the necessary location)
    public override bool DoAction(GameObject agent)
    {
        // Change the tag of the hunter so the rabbit will leave hiding, the hunter will no longer be proessing an interruptiong
        this.gameObject.tag = "Resting";
        this.gameObject.GetComponent<Hunter>().processingInterruption = false;
        this.gameObject.GetComponent<Hunter>().rabbitDetected = null;

        // set this action's boolean
        restingAtShed = true;

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
        restingAtShed = false;
        target = null;
        SetInRange(false);
    }
}
