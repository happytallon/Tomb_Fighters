using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PaddleBaseController
{
    void FixedUpdate()
    {        
        transform.position = new Vector2(transform.position.x + Input.GetAxis("Horizontal") * speed, transform.position.y);
    }    
}
