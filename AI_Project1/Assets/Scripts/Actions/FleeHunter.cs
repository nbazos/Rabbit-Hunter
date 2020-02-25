using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeHunter : Action
{
    bool escapedHunter = false;

    // Initialize starting cost and preconditions/effects of this action
    public void Start()
    {
        AddActionEffect("stayAlive", true);
        cost = 3.0f;
    }

    // Return boolean for this action to represent if it has been completed or not
    public override bool ActionCompleted()
    {
        return escapedHunter;
    }

    // Complete the necessary steps so this action can be done
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Rabbit Sanctuary");

        return target != null;
    }

    // Do the action itself (if range is required for this action then you are already at the necessary location)
    public override bool DoAction(GameObject agent)
    {
        // set the hunter's tag to hidden so that the rabbit will search for carrots again
        this.gameObject.tag = "Hidden";

        // set this action's boolean
        escapedHunter = true;

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
        escapedHunter = false;
        target = null;
        SetInRange(false);
    }
}
