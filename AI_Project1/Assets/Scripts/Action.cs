using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    private List<KeyValuePair<string, object>> actionPreconditions;
    private List<KeyValuePair<string, object>> actionEffects;

    public float importanceValue = 1.0f;

    public GameObject target;

    public Action()
    {
        actionPreconditions = new List<KeyValuePair<string, object>>();
        actionEffects = new List<KeyValuePair<string, object>>();
    }

    public abstract bool ActionCompleted();

    public abstract bool CheckProceduralPrecondition(GameObject agent);

    public abstract bool DoAction();

    public void AddActionPrecondition(string key, object value)
    {
        actionPreconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemoveActionPrecondition(string key)
    {
        int indexToRemove = 0;

        for (int i = 0; i < actionPreconditions.Count; i++)
        {
            if(actionPreconditions[i].Key.Equals(key))
            {
                indexToRemove = i;
            }
        }

        actionPreconditions.RemoveAt(indexToRemove);
    }

    public void AddActionEffect(string key, object value)
    {
        actionEffects.Add(new KeyValuePair<string, object>());
    }

    public void RemoveActionEffect(string key)
    {
        int indexToRemove = 0;

        for (int i = 0; i < actionEffects.Count; i++)
        {
            if (actionEffects[i].Key.Equals(key))
            {
                indexToRemove = i;
            }
        }

        actionPreconditions.RemoveAt(indexToRemove);
    }
}
