using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveCarrot : Action
{
    bool carrotFound = false;

    // Start is called before the first frame update
    void Start()
    {
        // AddActionEffect("findCarrot", true);
        AddActionEffect("hasCarrot", true);
        cost = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool ActionCompleted()
    {
        return carrotFound;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Carrot");

        float closest = 0;
        GameObject closestTarget = null;

        foreach (GameObject obj in targets)
        {
            if (obj.GetComponent<Carrot>().carrotActive)
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
        }

        target = closestTarget;

        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        target.transform.parent = transform;

        carrotFound = true;

        return true;
    }

    public override bool IsRangeBased()
    {
        return true;
    }

    public override void Reset()
    {
        carrotFound = false;
        target = null;
        setInRange(false);
    }
}
