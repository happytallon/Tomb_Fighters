using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private GameController _gameController;

    void Awake()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        _gameController.PowerUpActive = false;
        Destroy(gameObject);
    }
}
