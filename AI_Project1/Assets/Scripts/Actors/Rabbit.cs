using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    float speed = 3.0f;
    bool carrotInMouth = false;
    bool isAlive = true;

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

        if (gameObject.transform.position == followingAction.target.transform.position)
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

        worldData.Add(new KeyValuePair<string, object>("hasCarrot", (carrotInMouth)));
        worldData.Add(new KeyValuePair<string, object>("isAlive", (isAlive)));

        return worldData;
    }

    public override List<KeyValuePair<string, object>> SetGoal()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("findCarrot", true),
            // new KeyValuePair<string, object>("storeCarrot", true),
            // new KeyValuePair<string, object>("escapeDanger", true)
        };

        return goal;
    }
}
