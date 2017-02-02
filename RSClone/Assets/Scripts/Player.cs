using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player character;

	// Use this for initialization
	void Awake ()
    {
        if(character != null)
        {
            Debug.LogError("Can't have multiple instances of playerSkills!");
        }
        character = this;
    }
}
