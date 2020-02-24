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
            foreach (GameObject hunter in GameObject.FindGameObjectsWithTag("Hunter"))
            {
                if (Vector3.Distance(this.gameObject.transform.position, hunter.transform.position) < 1.0f)
                {
                    this.GetComponent<RetrieveCarrot>().cost = 3.0f;

                    this.GetComponent<StoreCarrot>().cost = 3.0f;

                    inDanger = true;
                }

            }
        }
    }

    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            // { "findCarrot", true },
            {"stayAlive", true }
            // {"stayAlive", true }
        };

        return goal;
    }
}
