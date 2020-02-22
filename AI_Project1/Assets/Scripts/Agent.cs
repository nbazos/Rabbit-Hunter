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

    private I_InfoBridge infoBridge;

    private Planner actionPlanner;

    // Start is called before the first frame update
    void Start()
    {
        fSM = new FSM();
        possibleActions = new List<Action>();
        plannedActions = new Queue<Action>();
        actionPlanner = new Planner();
        RetrieveInfoBridge();
        IdleState();
        MoveToState();
        DoActionState();
        fSM.Push(idle);
        LoadPossibleActions();
    }

    // Update is called once per frame
    void Update()
    {
        fSM.Update(this.gameObject);
    }

    public void AddPossibleAction(Action action)
    {
        possibleActions.Add(action);
    }

    public void RemovePossibleAction(Action action)
    {
        possibleActions.Remove(action);
    }

    private bool ActionPlanActive()
    {
        if(plannedActions.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void IdleState()
    {
        idle = (fSM, gameObject) =>
        {
            Dictionary<string, object> stateofWorld = infoBridge.RetrieveWorldState();
            Dictionary<string, object> goal = infoBridge.SetGoal();

            Queue<Action> plan = actionPlanner.Plan(gameObject, possibleActions, stateofWorld, goal);

            if(plan != null)
            {
                plannedActions = plan;
                // infoBridge.PlanSuccess(goal, plan);

                fSM.Pop();
                fSM.Push(doAction);
            }
            else
            {
                // infoBridge.PlanInvalid(goal);
                fSM.Pop();
                fSM.Push(idle);
            }
        };
    }

    private void MoveToState()
    {
        moveTo = (fSM, gameObject) =>
        {
            Action action = plannedActions.Peek();

            if(action.target == null && action.IsRangeBased())
            {
                fSM.Pop();
                fSM.Pop();
                fSM.Push(idle);
            }

            if(infoBridge.IsAgentAtTarget(action))
            {
                fSM.Pop();
            }
        };
    }

    private void DoActionState()
    {
        doAction = (fSM, gameObject) =>
        {
            if(!ActionPlanActive())
            {
                fSM.Pop();
                fSM.Push(idle);
                // infoBridge.AllActionsPerformed();
                return;
            }

            Action action = plannedActions.Peek();

            if(action.ActionCompleted())
            {
                plannedActions.Dequeue();
            }

            if(ActionPlanActive())
            {
                action = plannedActions.Peek();

                bool inRange = action.IsRangeBased() ? action.WithinRange() : true; 

                if(inRange)
                {
                    bool isActionSuccessful = action.DoAction(gameObject);

                    if(!isActionSuccessful)
                    {
                        fSM.Pop();
                        fSM.Push(idle);
                        IdleState();
                        // infoBridge.PlanTermination(action);
                    }
                }
                else
                {
                    fSM.Push(moveTo);
                }
            }
            else
            {
                fSM.Pop();
                fSM.Push(idle);
                // infoBridge.AllActionsPerformed();
            }
        };
    }

    private void RetrieveInfoBridge()
    {
        infoBridge = gameObject.GetComponent<I_InfoBridge>();
    }

    private void LoadPossibleActions()
    {
        foreach(Action a in gameObject.GetComponents<Action>())
        {
            possibleActions.Add(a);
        }
    }
}
