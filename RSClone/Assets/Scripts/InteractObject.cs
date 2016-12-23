using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour {

    public Action[] Options;

    private void OnMouseEnter()
    {
        if (Options[0] != null)
        {
            ActionLister.AddActions(Options);

        }
        else
        {
            Debug.LogError("No Options on game object " + gameObject.name);
        }
    }

    private void OnMouseExit()
    {
        ActionLister.RemoveActions(gameObject);
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
        obj.SendMessage(funct);
    }
}
