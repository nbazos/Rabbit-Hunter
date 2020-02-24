using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureRabbit : Action
{
    private bool rabbitCaptured = false;

    public CaptureRabbit()
    {
        AddActionEffect("huntRabbit", true);
        cost = 1.0f;
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
        return rabbitCaptured;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Rabbit");

        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        target.GetComponent<Rabbit>().speed = 0.0f;
        target.transform.parent = transform;

        this.gameObject.GetComponent<Hunter>().speed = 0.0f;

        rabbitCaptured = true;

        return true;
    }

    public override bool IsRangeBased()
    {
        return true;
    }

    public override void Reset()
    {
        rabbitCaptured = false;
        target = null;
        setInRange(false);
    }
}
