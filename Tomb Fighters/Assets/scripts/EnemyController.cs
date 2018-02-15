using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PaddleBaseController
{
    public float Delay = 0.05f;

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
        if (_ball == null)
        {
            MoveRacket(0);
            return;
        }

        var newPosition = new Vector2(_ball.position.x, transform.position.y);
        MoveRacketByPosition(newPosition, Delay);
    }
}
