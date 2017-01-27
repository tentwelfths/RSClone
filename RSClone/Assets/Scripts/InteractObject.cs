using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

    public Action[] Options;

    public void Awake()
    {
        // Idiot-proofing.
        for (int i = 0; i < Options.Length; i++)
            if (Options[i].obj == null)
            {
                Debug.LogWarning("Object " + gameObject.name + " has null objects in its actions list. Temporarily setting them to self.");
                Options[i].obj = gameObject;
            }
                
    }

    public void ClearNullActions()
    {
        for (int i = 0; i < Options.Length; i++)
            if (Options[i].obj == null)
            {
                Options[i].obj = gameObject;
            }
    }

    public void SendActions()
    {
        if (Options[0] == null)
            Debug.LogError("No Options on game object " + gameObject.name);

        ActionLister.ins.AddActions(Options);
    }

    public bool isAdjacentTo(GameObject other)
    {

        return false;
    }
}

[System.Serializable]
public class Action
{
    public string funct;
    public GameObject obj;
    public string text;
    public int priority;

    public void Execute()
    {
        //Debug.Log("Executing " + funct + " " + obj.name);
        if(obj)
            obj.SendMessage(funct);
    }
}
