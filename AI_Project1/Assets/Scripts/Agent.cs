using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private FSM fSM;

    private FSM.FSMState idle;
    private FSM.FSMState moveTo;
    private FSM.FSMState doAction;

    private List<Action> possibleActions;
    private Queue<Action> plannedActions;

    private Planner actionPlanner;

    // Start is called before the first frame update
    void Start()
    {
        fSM = new FSM();
        possibleActions = new List<Action>();
        plannedActions = new Queue<Action>();
        actionPlanner = new Planner();

    }

    // Update is called once per frame
    void Update()
    {
        fSM.Update(gameObject);
    }

}
