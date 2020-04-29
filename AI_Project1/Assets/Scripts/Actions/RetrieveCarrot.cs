using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveCarrot : Action
{
    bool carrotFound = false;

    // Initialize starting cost and preconditions/effects of this action
    public void Start()
    {
        AddActionEffect("hasCarrot", true);
        cost = 1.0f;
    }

    // Return boolean for this action to represent if it has been completed or not
    public override bool ActionCompleted()
    {
        return carrotFound;
    }

    // Complete the necessary steps so this action can be done
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // Find the closest carrot that is active and set that to the action target

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Carrot");

        float closest = 0;
        GameObject closestTarget = null;

        foreach (GameObject obj in targets)
        {
            if (obj.transform.GetComponent<Carrot>().carrotActive)
            {
                float dist = Vector3.Distance(obj.transform.position, gameObject.transform.position);

                if(closest == 0)
                {
                    closestTarget = obj;
                    closest = dist;
                }

                else if(dist < closest)
                {
                    closestTarget = obj;
                    closest = dist;
                }
            }
            else
            {
                continue;
            }
        }

        target = closestTarget;

        return target != null;
    }

    // Do the action itself (if range is required for this action then you are already at the necessary location)
    public override bool DoAction(GameObject agent)
    {
        // Attach the carrot to the rabbit
        target.transform.parent = transform;

        // Turn on sound collider for a second when the carrot is picked up
        StartCoroutine(TurnOnSoundCollider());

        // Set this action's boolean
        carrotFound = true;

        return true;
    }

    IEnumerator TurnOnSoundCollider()
    {
        gameObject.transform.GetChild(1).GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<LineRenderer>().enabled = true;

        yield return new WaitForSeconds(1f);

        gameObject.transform.GetChild(1).GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    // Is this action dependent on range?
    public override bool IsRangeBased()
    {
        return true;
    }

    // Reset this action's variables
    public override void Reset()
    {
        carrotFound = false;
        target = null;
        SetInRange(false);
    }
}
