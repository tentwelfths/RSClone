using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AligntoMap))]
public class CharacterWalk : MonoBehaviour {
	private float distance;
    public float speed = 1.0f;



	public void SetLocalGoal(Vector3 destination){
		if (destination.y > 10000) {
			destination = transform.position;
		}
		SetDestination(new Vector2(destination.x, destination.z));
	}
    public void SetDestination(Vector3 destination)
    {
        //distance = 0.5f;
        //if (p != null) {
        //	p.FindPath (transform.position, destination);
        //} else {
        //	SetLocalGoal (destination);
        //}
        GetComponent<CharacterPathFinder>().Target = destination;
    }

	// Update is called once per frame
	/*void FixedUpdate () {
		if (p != null&&!p.started) {
			distance += speed / 60f;
			SetLocalGoal(p.getPoint(distance));
		}

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
	}*/
}
