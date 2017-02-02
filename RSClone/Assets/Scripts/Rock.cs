using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {


    public int _model;
    public int _facing;
    public int _quadrant1;
    public int _quadrant2;
    public int _quadrant3;
    public int _quadrant4;

    private GameObject Model;
    private Vector2 Facing;
    private Texture2D[] Quadrants;


	// Use this for initialization
	void Start ()
    {
        GenerateRock();
	}

    private void RandomizeRock()
    {

    }

    private void GenerateRock()
    {
        //Instantiate the rock's mesh 4 times
        //Set the facing of each mesh
        //Set each mesh's texture
        //Don't forget to flip textures, 2, 3 and 4 accordingly.
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
