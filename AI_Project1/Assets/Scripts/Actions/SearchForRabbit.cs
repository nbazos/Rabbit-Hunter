using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForRabbit : Action
{
    bool rabbitFound = false;
    [HideInInspector] public GameObject wayPoint;

    // Initialize starting cost and preconditions/effects of this action
    public void Start()
    {
        AddActionEffect("rabbitFound", true);
        cost = 1.0f;

        wayPoint = new GameObject("Waypoint");
    }

    // Return boolean for this action to represent if it has been completed or not
    public override bool ActionCompleted()
    {
        return rabbitFound;
    }

    // Complete the necessary steps so this action can be done
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // Set wandering waypoint
        if (wayPoint != null)
        {
            wayPoint.transform.position = gameObject.GetComponent<Hunter>().CreateWanderPoint();

            target = wayPoint;
        }

        return target != null;
    }

    // Do the action itself (if range is required for this action then you are already at the necessary location)
    public override bool DoAction(GameObject agent)
    {
        // If the rabbit has not been detected keep wandering
        if (gameObject.GetComponent<Hunter>().rabbitDetected != null)
        {
            rabbitFound = true;
            return true;
        }
        else
        {
            rabbitFound = false;
            return false;
        }
    }

    // Is this action dependent on range?
    public override bool IsRangeBased()
    {
        return true;
    }

    // Reset this action's variables
    public override void Reset()
    {
        rabbitFound = false;
        target = null;
        SetInRange(false);
    }
}
