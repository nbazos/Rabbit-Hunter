using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    // Stack structure (LIFO) to hold states
    private Stack<I_FSMState> states = new Stack<I_FSMState>();

    public delegate void I_FSMState(FSM fSM, GameObject obj);

    // Update is called once per frame
    public void Update(GameObject obj)
    {
        if (states.Peek() != null)
        {
            // use Invoke() to call delegate 
            states.Peek().Invoke(this, obj);
        }
    }

    // Push a state on the stack
    public void Push(I_FSMState state)
    {
        states.Push(state);
    }

    // Pop a state off the top of the stack
    public void Pop()
    {
        states.Pop();
    }

}
