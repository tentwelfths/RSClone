using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour {

    public Transform Hotspot;
    public GameObject Player;

    void Start()
    {
        if (Hotspot == null)
            Debug.LogError("No Hotspot initialized for " + gameObject.name);
    }

    public void Talk()
    {
        Vector3 HotspotPos = new Vector3(0, 0, 0);
        if (Player.transform.position.x - transform.position.x > 0.1f)
            HotspotPos.x = 1.0f / transform.localScale.x;
        if (Player.transform.position.z - transform.position.z > 0.1f)
            HotspotPos.z = 1.0f / transform.localScale.z;
        if (Player.transform.position.x - transform.position.x < -0.1f)
            HotspotPos.x = -1.0f / transform.localScale.x;
        if (Player.transform.position.z - transform.position.z < -0.1f)
            HotspotPos.z = -1.0f / transform.localScale.z;

        Hotspot.localPosition = HotspotPos;
        Player.SendMessage("SetDestination", Hotspot.position);
        GamePlayLog.LogMessage(gameObject.name + " isn't interested in talking right now.");
    }
}
