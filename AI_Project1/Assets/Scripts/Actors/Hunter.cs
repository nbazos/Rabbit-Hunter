using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    float speed = 1.0f;
    int rabbitsCarried = 0;
    int availableAmmo = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool IsAgentAtTarget(Action followingAction)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, followingAction.transform.position, speed * Time.deltaTime);

        if(gameObject.transform.position == followingAction.target.transform.position)
        {
            followingAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override List<KeyValuePair<string, object>> RetrieveWorldState()
    {
        List<KeyValuePair<string, object>> worldData = new List<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasRabbits", (rabbitsCarried > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasAmmo", (availableAmmo > 0)));

        return worldData;
    }

    public override List<KeyValuePair<string, object>> SetGoal()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("killRabbits", true),
            new KeyValuePair<string, object>("storeRabbits", true)
        };

        return goal;
    }
}
