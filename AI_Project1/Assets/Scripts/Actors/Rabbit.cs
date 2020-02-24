using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    public bool inDanger = false;
    public bool processingNewPlan = false;

    private void Start()
    {
        speed = 2.0f;
    }

    public void Update()
    {
        if (!processingNewPlan)
        {
            GameObject hunter = GameObject.FindGameObjectWithTag("Hunter");

            if (hunter != null)
            {

                if (Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Hunter").transform.position) < 1.5f)
                {
                    inDanger = true;
                }
                else
                {
                    inDanger = false;
                }
            }
        }
    }

    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            {"stayAlive", true }
        };

        return goal;
    }
}
