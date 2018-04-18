using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{	
	public Rigidbody2D rigidbody;
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		var x = Input.GetAxis("Horizontal");
		rigidbody.velocity = new Vector2(x, 0) * 5;
	}

	void OnMouseDown()
	{
		
	}
}
