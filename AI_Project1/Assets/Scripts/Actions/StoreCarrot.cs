using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCarrot : Action
{
    bool carrotStored = false;

    // Initialize starting cost and preconditions/effects of this actions
    public void Start()
    {
        AddActionPrecondition("hasCarrot", true);
        AddActionEffect("hasCarrot", false);
        AddActionEffect("stayAlive", true);

        cost = 1.0f;
    }

    // Return boolean for this action to represent if it has been completed or not
    public override bool ActionCompleted()
    {
        return carrotStored;
    }

    // Complete the necessary steps so this action can be done
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // Rabbit should drop off carrots at its den
        target = GameObject.FindGameObjectWithTag("Rabbit Den");

        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        // Move the carrot from the rabbit to its den
        GameObject child = transform.GetChild(2).gameObject; // Index of 2 necessary to bypass the asset object child itself and the sound collider

        // Attach carrot to the rabbit den
        child.transform.parent = GameObject.FindGameObjectWithTag("Rabbit Den").transform;

        // Carrot can no longer be retrieved
        child.GetComponent<Carrot>().carrotActive = false;

        // set this action's boolean
        carrotStored = true;

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
        carrotStored = false;
        target = null;
        SetInRange(false);
    }
}
