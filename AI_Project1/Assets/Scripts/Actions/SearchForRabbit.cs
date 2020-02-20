using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForRabbit : Action
{
    private bool rabbitFound = false;

    public SearchForRabbit()
    {
        AddActionPrecondition("hasRabbit", false);
        AddActionEffect("rabbitSeen", true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override bool ActionCompleted()
    {
        return rabbitFound;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        throw new System.NotImplementedException();
    }

    public override bool DoAction(GameObject agent)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 10.0f))
        {
            if (hit.collider.tag == "Rabbit")
            {
                target = hit.collider.gameObject;
                rabbitFound = true;
            }
        }

        return true;
    }

    public override bool IsRangeBased()
    {
        return false;
    }

    public override void Reset()
    {
        rabbitFound = false;
    }
}
