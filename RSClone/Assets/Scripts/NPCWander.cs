using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWander : MonoBehaviour {

    public float Range = 2.0f;
    public float Frequency = 1.0f;
    private float Countdown;
    private Vector2 InitialPos;

	// Use this for initialization
	void Start () {
        Countdown = Frequency;
        InitialPos = new Vector2(transform.position.x, transform.position.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Countdown -= Time.deltaTime;

        if (Countdown < 0.0f)
        {
            Vector2 newDest = InitialPos;
            newDest.x += Random.Range(-Range, Range);
            newDest.y += Random.Range(-Range, Range);
            gameObject.SendMessage("SetDestination", newDest);
            Countdown = Frequency;
        }
	}
}
