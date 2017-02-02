using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining_MinedOre : MonoBehaviour {
    public float RespawnTime = 0.5f;
    private float RespawnCountdown;

    private MultiStateObject states;

    // Use this for initialization
    void Start()
    {
        states = transform.parent.GetComponent<MultiStateObject>();
        RespawnCountdown = RespawnTime * 60.0f;

    }

    // Update is called once per frame
    void Update()
    {
        RespawnCountdown -= Time.deltaTime;
        if (RespawnCountdown < 0.0f)
            states.SetState("Unmined");
    }
}
