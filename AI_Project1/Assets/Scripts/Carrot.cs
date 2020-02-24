using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public bool carrotActive = true;

    private void Update()
    {
        GameObject hunter = GameObject.FindGameObjectWithTag("Hunter");

        if (hunter != null && this.transform.parent != GameObject.FindGameObjectWithTag("Rabbit Den").transform)
        {

            if (Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Hunter").transform.position) < 2.0f)
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
