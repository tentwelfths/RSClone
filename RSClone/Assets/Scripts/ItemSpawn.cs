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
    private Collider col;


    // Use this for initialization
    void Start () {
        col = gameObject.GetComponent<Collider>();
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

    void respawnItem()
    {
        newItem = Instantiate(Inventory.lookupItem(item).model);
        newItem.transform.SetParent(gameObject.transform);
        Vector3 finalpos = newItem.transform.localPosition;
        finalpos.x = 0.0f;
        finalpos.z = 0.0f;
        newItem.transform.localPosition = finalpos;
        collected = false;
        col.enabled = true;
    }

    void Take()
           {
        if (Inventory.addItem(item))
        {
            collected = true;
            countdowntimer = respawnTime * 60.0f;
            Destroy(newItem);
            col.enabled = false;
        }

        }

}
