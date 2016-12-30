using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AligntoMap))]
public class CharacterWalk : MonoBehaviour {

    public float speed = 1.0f;
    private Vector2 Destination;

	// Use this for initialization
	void Start () {
        Destination.x = transform.position.x;
        Destination.y = transform.position.z;
    }
	
    public void SetDestination(Vector3 destination)
    {
        SetDestination(new Vector2(destination.x, destination.z));
    }

    public void SetDestination(Vector2 destination)
    {
        Destination.x = Mathf.RoundToInt(destination.x - 0.5f) + 0.5f;
        Destination.y = Mathf.RoundToInt(destination.y - 0.5f) + 0.5f;
    }

	// Update is called once per frame
	void FixedUpdate () {
        float deltX = Destination.x - transform.position.x;
        float deltY = Destination.y - transform.position.z;
        if ((deltX * deltX > 0.01f) || (deltY * deltY > 0.01f))
        {
            transform.position += new Vector3(deltX, 0, deltY).normalized * Time.deltaTime * speed;
            gameObject.SendMessage("alignToMap");
        }
        else
        {
            transform.position = new Vector3(Destination.x, transform.position.y, Destination.y);
        }
	}
}
