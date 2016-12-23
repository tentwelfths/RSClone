using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingRange : MonoBehaviour {

    public Transform Hotspot;
    
    // Replace this prior to online implementation!!
    public GameObject Player;

	// Use this for initialization
	void Start ()
    {
        if (Hotspot == null)
            Debug.LogError("No Hotspot initialized for " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Cook()
    {
        Player.SendMessage("SetDestination", Hotspot.position);
        GamePlayLog.LogMessage("You'll be able to cook stuff here.. eventually.");
    }
}
