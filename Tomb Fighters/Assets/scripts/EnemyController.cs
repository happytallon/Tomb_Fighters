using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;

    public Transform ball;

    private float direction;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ball == null)
        {
            var temp = GameObject.FindGameObjectWithTag("Ball");
            ball = temp == null ? null : temp.GetComponent<Transform>();
        }
    }

    void FixedUpdate()
    {
        if (ball != null)
        {
            if (ball.position.x > transform.position.x)
                direction = 1;
            if (ball.position.x < transform.position.x)
                direction = -1;
        }

        transform.position = new Vector2(transform.position.x + direction * speed, transform.position.y);
    }
}
