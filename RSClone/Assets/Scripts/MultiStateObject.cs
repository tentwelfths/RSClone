using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiStateObject : MonoBehaviour {

    private int currState;
    private GameObject currStateObject;
    public ObjectState[] States;
    public string initialState;

    private void Awake()
    {
        currStateObject = Instantiate(States[0].Model);
        SetState(initialState);
    }


    // For the love of God don't use SetState(int) unless you have to!
	private void SetState(int _toState)
    {
        Destroy(currStateObject);

        currState = _toState;
        currStateObject = Instantiate(States[_toState].Model);
        currStateObject.transform.SetParent(transform);
        currStateObject.transform.localPosition = currStateObject.transform.position;
    }

    public void SetState(string _toState)
    {
        for(int i = 0; i < States.Length; i++)
        {
            if (States[i].Name == _toState)
            {
                SetState(i);
                return;
            }
        }

        Debug.LogError("State \"" + _toState + "\" does not exist!");
    }

    public string GetState()
    {
        return States[currState].Name;
    }

    [System.Serializable]
    public struct ObjectState
    {
        public string Name;
        public GameObject Model;
    }
}
