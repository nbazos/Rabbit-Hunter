using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureRabbit : Action
{
    private bool rabbitCaptured = false;

    public CaptureRabbit()
    {
        AddActionEffect("killRabbit", true);
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
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position, Vector3.forward, out hit, 10.0f))
        //{
        //    if (hit.collider.tag == "Rabbit")
        //    {
        //        target = hit.collider.gameObject;
        //        this.GetComponent<Hunter>().rabbitSeen = true;
        //    }
        //}

        target = GameObject.FindGameObjectWithTag("Rabbit");


        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        target.transform.parent = transform;

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
    }
}
