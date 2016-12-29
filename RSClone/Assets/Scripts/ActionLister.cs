using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLister : MonoBehaviour
{

    private static List<Action> Actions = new List<Action>();
    public UnityEngine.UI.Text topAction;

    private void Awake()
    {
    }

    public static void AddActions(Action[] _Actions)
    {
        for (int i = 0; i < _Actions.Length; i++)
        {
            Actions.Add(_Actions[i]);
        }

        //Sort Current Actions
    }

    public static void RemoveActions(GameObject toRemove)
    {
        int i = 0;
        while (i < Actions.Count)
        {
            if (Actions[i].obj = toRemove)
            {
                Actions.RemoveAt(i);
                i = 0;
            }
            }
    }

    void Update()
    {
        Vector3 topActionPos = Input.mousePosition;
        topAction.rectTransform.position = topActionPos + new Vector3(100,-30,0);
        if (Actions.Count > 0)
        {
            if (Actions.Count > 1)
                topAction.text = Actions[0].text + "...";
            else
                topAction.text = Actions[0].text;
            //Left-click: First Action
            if (Input.GetMouseButtonDown(0))
            {
                Actions[0].Execute();
            }
            //Right-click: All Actions
            if (Input.GetMouseButtonDown(1))
            {

            }
        }
        else
            topAction.text = "";
    }

}