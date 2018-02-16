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
            var balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (var ball in balls)
            {
                var direction = ball.GetComponent<Rigidbody2D>().velocity.normalized;
                if (direction.y > 0)
                {
                    _ball = ball.GetComponent<Transform>();
                    break;
                }
            }            
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == _ball.gameObject)
            _ball = null;
    }
}
