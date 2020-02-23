using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCarrot : Action
{
    bool carrotStored = false;

    // Start is called before the first frame update
    void Start()
    {
        AddActionPrecondition("hasCarrot", true);
        AddActionEffect("hasCarrot", false);
        AddActionEffect("storeCarrot", true);
        // AddActionEffect("findCarrot", false);
        //AddActionEffect("stayAlive", true);

        cost = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool ActionCompleted()
    {
        return carrotStored;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Rabbit Den");

        return target != null;
    }

    public override bool DoAction(GameObject agent)
    {
        GameObject child = transform.GetChild(0).gameObject;

        child.transform.parent = GameObject.FindGameObjectWithTag("Rabbit Den").transform;

        child.GetComponent<Carrot>().carrotActive = false;

        carrotStored = true;

        return true;
    }

    public override bool IsRangeBased()
    {
        return true;
    }

    public override void Reset()
    {
        carrotStored = false;
        target = null;
        setInRange(false);
    }
}
