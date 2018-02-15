using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PaddleBaseController
{
    private string _axis = "Horizontal";
    void FixedUpdate()
    {
        var x = Input.GetAxis(_axis);
        MoveRacket(x);
    }
}
