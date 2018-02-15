using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PaddleBaseController
{
    private Transform _ball;

    void Update()
    {
        if (_ball == null)
        {
            var temp = GameObject.FindGameObjectWithTag("Ball");
            _ball = temp == null ? null : temp.GetComponent<Transform>();
        }
    }
    void FixedUpdate()
    {
        if (_ball == null) return;

        var velocity = _ball.position.x > transform.position.x ? 1 : -1;
        MoveRacket(velocity);
    }
}
