using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeHunter : Action
{
    bool escapedHunter = false;
    

    // Start is called before the first frame update
    void Start()
    {
        // AddActionEffect("findCarrot", true);
        AddActionEffect("stayAlive", true);
        cost = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool ActionCompleted()
    {
        return escapedHunter;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Rabbit Sanctuary");

        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        this.gameObject.tag = "Hidden";

        escapedHunter = true;

        return true;
    }

    public override bool IsRangeBased()
    {
        return true;
    }

    public override void Reset()
    {
        escapedHunter = false;
        target = null;
        setInRange(false);
    }
}
