using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CLEANING THIS FILE IS TOP PRIORITY FOR FINAL RELEASE.

public class ActionLister : MonoBehaviour
{

    public static ActionLister ins;

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
    private LayerMask mapLayer;
    public UnityEngine.UI.GraphicRaycaster gRay;

    public string useItem;

    public void Use(string Item)
    {
        useItem = Item;
    }

    private void Awake()
    {
        if (ins != null)
            Debug.LogError("Only one ActionLister per scene");

        ins = this;

        ToggleActionState();
        useItem = "";
        mapLayer = LayerMask.GetMask("Map");
    }

    public void AddActions(Action[] _Actions)
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

    public void AddAction(Action _toAdd)
    {
        // Don't add redundant Actions
        for (int i = 0; i < Actions.Count; i++)
            if (Actions[i].text == _toAdd.text)
            {
                return;
            }

        Actions.Add(_toAdd);
    }

    private void ClearActions()
    {
        Actions.Clear();
    }

    public void RemoveActions(GameObject toRemove)
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

    private void Update()
    {

        if (currState == ActionState.state_firstaction)
        {
            FirstActionState();
        }
        
        if (currState == ActionState.state_listactions)
        {
            ListActionsState();
        }

        if (currState == ActionState.state_firstaction && (Input.mousePosition.x / Screen.width < 0.75f || Input.mousePosition.y / Screen.height > 0.67f))
            ClearActions();
    }

    private void SortActions()
    {
        Action[] ActionList = Actions.ToArray();
        // Sort the actions by priority
        if (ActionList.Length <= 1)
            return;

        ClearActions();
        for(int i = 1; i < ActionList.Length; i++)
        {
            if(ActionList[i].priority > ActionList[i-1].priority)
            {
                Action temp = ActionList[i - 1];
                ActionList[i - 1] = ActionList[i];
                ActionList[i] = temp;
                i = 0;
            }
        }

        Actions.AddRange(ActionList);
    }

    private bool UpdateMouseOverActions()
    {
        RaycastHit Ray;
        Vector3 mousePos = Input.mousePosition;
        //For now this is just the main area
        if (Input.mousePosition.x / Screen.width < 0.75f || Input.mousePosition.y / Screen.height > 0.67f)
        {
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Camera.main.transform.forward, out Ray))
            {

                if (useItem != "" && Ray.collider.gameObject.layer != mapLayer)
                {
                    Action use = new Action();
                    use.funct = "UseItem";
                    use.obj = Ray.collider.gameObject;
                    use.priority = 1;
                    use.text = "Use " + Inventory.inv.lookupItem(useItem).name + " -> " + use.obj.name;
                    AddAction(use);
                }

                Ray.collider.SendMessage("SendActions", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
           // If I need some moar behaviorinos they can go here.
        }

        SortActions();

        return (Actions.Count > 0);
    }

    private void FirstActionState()
    {
            Vector3 topActionPos = Input.mousePosition;
            topAction.rectTransform.position = topActionPos + new Vector3(100, -30, 0);
            
            
            if (UpdateMouseOverActions())
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
                
                // Right-click: List Actions State
                if (Input.GetMouseButtonDown(1))
                {
                    
                    currState = ActionState.state_listactions;
                    ActionMenu.rectTransform.position = topActionPos + new Vector3(0, -ActionMenu.rectTransform.rect.height, 0);
                    // Move Action List display to mouse position

                }
            }
            else
                topAction.text = "";

        }

    private void ListActionsState()
    {
        ListActions();
    }

    private void ListActions()
    {
        int i = 0;
        for(; i < Actions.Count && i < ActionButtonText.Length; i++)
            ActionButtonText[i].text = Actions[i].text;
        for (; i < ActionButtonText.Length; i++)
            ActionButtonText[i].text = "";
        
    }

    public void ExecuteAction(int _ActionIndex)
    {

        if (Actions.Count <= _ActionIndex)
        {
            return;
        }
        Actions[_ActionIndex].Execute();

        ToggleActionState();

        // Clear out Use Item if it was being used
        if (Actions[_ActionIndex].funct != "Use")
            useItem = "";

        //So that you don't get duplicates of the same action
        ClearActions();
    }

    public void ToggleActionState()
    {
        currState = ActionState.state_firstaction;
        ActionMenu.rectTransform.position = new Vector3(0, -ActionMenu.rectTransform.rect.height, 0);
    }
}