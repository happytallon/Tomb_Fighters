using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {        
        transform.position = new Vector2(transform.position.x + Input.GetAxis("Horizontal") * speed, transform.position.y);
    }
}
