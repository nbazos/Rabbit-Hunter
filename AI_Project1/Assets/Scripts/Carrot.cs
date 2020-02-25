using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public bool carrotActive = true;

    private void Update()
    {
        GameObject hunter = GameObject.FindGameObjectWithTag("Hunter");

        // The rabbit should not go for a carrot that that is already in its den or is too close to a hunter
        if (hunter != null && this.transform.parent != GameObject.FindGameObjectWithTag("Rabbit Den").transform)
        {

            if (Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Hunter").transform.position) < 3.0f)
            {
                carrotActive = false;
            }
            else
            {
                carrotActive = true;
            }
        }
    }
}
