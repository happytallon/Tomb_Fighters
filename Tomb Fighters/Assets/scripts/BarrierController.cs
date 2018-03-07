﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    private Rigidbody2D _ballRigidBody;
    private GameController _gameController;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _ballRigidBody = GetBallRigidbody();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ballRigidBody == null)
        {
            _ballRigidBody = GetBallRigidbody();
            return;
        }

        if (_gameController.UpdateBarrier)
        {
            
        }
    }

    public void ActivateDeactivateBarrier(bool activated)
    {
        var color = _spriteRenderer.color;
        color.a = activated ? 1f : 0.5f;
        _spriteRenderer.color = color;

        var ballCollider = _ballRigidBody.gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(ballCollider, GetComponent<Collider2D>(), !activated);
    }

    private Rigidbody2D GetBallRigidbody()
    {
        var obj = GameObject.Find("PowerBall(Clone)");
        return obj != null ? obj.GetComponent<Rigidbody2D>() : null;
    }
}
