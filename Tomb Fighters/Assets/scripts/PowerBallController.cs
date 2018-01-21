using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallController : MonoBehaviour
{
    public float xspeed = 2f;
    public float yspeed = 2f;

    private float _xdirection = 1f;
    private float _ydirection = 1f;
    private float _xmod = 0f;

    private SpriteRenderer _playerSprite;
    private SpriteRenderer _enemySprite;

    // Use this for initialization
    void Start()
    {
        _playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        _enemySprite = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + (_xdirection * xspeed) * _xmod, transform.position.y + yspeed * _ydirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Edge") _xdirection *= -1;
        if (collision.collider.tag == "Player")
        {
            _ydirection *= -1;

            var gap = _playerSprite.size.x / 6;
            if (transform.position.x < collision.collider.transform.position.x - gap || transform.position.x > collision.collider.transform.position.x + gap)
            {
                _xmod += 0.25f;
            }
            _xmod += 0.75f * Math.Abs(Input.GetAxis("Horizontal"));
        }

        if (collision.collider.tag == "Enemy")
        {
            _ydirection *= -1;

            var gap = _enemySprite.size.x / 6;
            if (transform.position.x < collision.collider.transform.position.x - gap || transform.position.x > collision.collider.transform.position.x + gap)
            {
                _xmod += 0.5f;
            }
        }
    }
}
