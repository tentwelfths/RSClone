using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour {

    //Item which spawns
    public string item;

    //Respawn time in minutes
    public float respawnTime = 0.5f;
    private float countdowntimer = 0.0f;
    private bool collected = false;
    private GameObject newItem;


    // Use this for initialization
    void Start () {
        respawnItem();
	}
	
	// Update is called once per frame
	void Update () {
        if (collected)
        {
            countdowntimer -= Time.deltaTime;
            if (countdowntimer <= 0.0f)
                respawnItem();
        }
	}

    public void Collected()
    {
        collected = true;
        countdowntimer = respawnTime * 60.0f;
    }

    void respawnItem()
    {
        newItem = Instantiate(Inventory.inv.lookupItem(item).model);
        newItem.transform.SetParent(gameObject.transform);
        newItem.transform.localPosition = newItem.transform.position;
        collected = false;
    }

}
