using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    public float speed = 1.0f;
    bool hasRabbits = false;
    public bool rabbitSeen = false;
    bool hasAmmo = false;
    private Vector3 wanderingPoint;

    // no numbers?

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
        //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, followingAction.transform.position, speed * Time.deltaTime);

        //if(gameObject.transform.position == followingAction.target.transform.position)
        //{
        //    followingAction.setInRange(true);
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        if (!rabbitSeen)
        {

            transform.position += transform.TransformDirection(Vector3.forward) * this.GetComponent<Hunter>().speed * Time.deltaTime;
            if ((transform.position - wanderingPoint).magnitude < 3)
            {
                wanderingPoint = Random.insideUnitSphere * 47;
                wanderingPoint.y = 1;
                transform.LookAt(wanderingPoint);
            }
            return false;
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, followingAction.target.transform.position, speed * Time.deltaTime);

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
    }

    public override List<KeyValuePair<string, object>> RetrieveWorldState()
    {
        List<KeyValuePair<string, object>> worldData = new List<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasRabbits", hasRabbits));

        return worldData;
    }

    public override List<KeyValuePair<string, object>> SetGoal()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("killRabbits", true),
            //new KeyValuePair<string, object>("storeRabbits", true)
        };

        return goal;
    }
}
