using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAtShed : Action
{
    private bool restingAtShed = false;

    public RestAtShed()
    {
        AddActionEffect("huntRabbit", true);
        cost = 3.0f;
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
        return restingAtShed;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Hunter Shed");

        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        this.gameObject.tag = "Resting";
        this.gameObject.GetComponent<Hunter>().processingNewPlan = false;

        restingAtShed = true;

        return true;
    }

    public override bool IsRangeBased()
    {
        return true;
    }

    public override void Reset()
    {
        restingAtShed = false;
        target = null;
        setInRange(false);
    }
}
