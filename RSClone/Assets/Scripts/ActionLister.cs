using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLister : MonoBehaviour
{

    private enum ActionState
    {
        state_firstaction,
        state_listactions
    };

    private static List<Action> Actions = new List<Action>();
    private static ActionState currState = ActionState.state_firstaction;
    public UnityEngine.UI.Text topAction;
    public UnityEngine.UI.Text[] ActionButtonText;
    public UnityEngine.UI.Image ActionMenu;

    private void Awake()
    {
    }

    private static void ResetActionList()
    {
        Actions.Clear();
    }

    public static void AddActions(Action[] _Actions)
    {
        // Don't mess with action list if you're picking one!
        if (currState == ActionState.state_listactions)
            return;

        for (int i = 0; i < _Actions.Length; i++)
        {
            AddAction(_Actions[i]);
        }

        //Sort Current Actions
    }

    private static void AddAction(Action _toAdd)
    {
        // Don't add redundant Actions
        for (int i = 0; i < Actions.Count; i++)
            if (Actions[i].text == _toAdd.text)
            {
                return;
            }

        Actions.Add(_toAdd);
    }

    public static void RemoveActions(GameObject toRemove)
    {
        // Don't mess with action list if you're picking one!
        if (currState == ActionState.state_listactions)
            return;
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
        if (currState == ActionState.state_firstaction)
        {
            Vector3 topActionPos = Input.mousePosition;
            topAction.rectTransform.position = topActionPos + new Vector3(100, -30, 0);
            if (Actions.Count > 0)
            {
                if (Actions.Count > 1)
                    topAction.text = Actions[0].text + "...";
                else
                    topAction.text = Actions[0].text;
                //Left-click: First Action
                if (Input.GetMouseButtonDown(0))
                {
                    ExecuteAction(0);
                }
                //Right-click: All Actions
                if (Input.GetMouseButtonDown(1))
                {
                    ListActions();
                    currState = ActionState.state_listactions;
                    ActionMenu.rectTransform.position = topActionPos + new Vector3(0,-ActionMenu.rectTransform.rect.height, 0);
                    // Move Action List display to mouse position

                }
            }
            else
                topAction.text = "";
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
            }
                
        }
    }

    private void ListActions()
    {
        for(int i = 0; i < Actions.Count && i < ActionButtonText.Length; i++)
        {
            ActionButtonText[i].text = Actions[i].text;
        }
        if(Actions.Count < 5)
        {
            for (int i = Actions.Count; i < ActionButtonText.Length; i++)
                ActionButtonText[i].text = "";
        }
    }
    public void TestFunction()
    {
        Debug.Log("test");
    }
    public void ExecuteAction(int _ActionIndex)
    {
        if (Actions.Count <= _ActionIndex)
        {
            return;
        }
        Actions[_ActionIndex].Execute();
        ToggleActionState();

        //So that you don't get duplicates of the same action
        //ResetActionList();
    }

    public void ToggleActionState()
    {
        currState = ActionState.state_firstaction;
        ActionMenu.rectTransform.position = new Vector3(0, -ActionMenu.rectTransform.rect.height, 0);
    }
}