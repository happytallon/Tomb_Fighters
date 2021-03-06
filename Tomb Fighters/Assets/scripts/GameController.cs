﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Elements
    public Transform Ball;
    public Transform Player;
    public Transform Enemy;
    public Canvas Canvas;

    //Ball
    private float _delayBallSpammer;

    //PowerUps
    public float PowerUpDelay;
    public float _currentPowerUpDelay;

    public List<GameObject> PowerUps_Objects;
    public List<float> PowerUps_Probability;
    public List<AudioClip> PowerUps_AudioClip;
    private Dictionary<GameObject, float> _powerUps = new Dictionary<GameObject, float>();

    public bool PowerUpActive;
    public List<GameObject> PlayerBarriers = new List<GameObject>();
    public List<GameObject> EnemyBarriers = new List<GameObject>();

    //Audio
    private AudioSource _music;
    private AudioSource _powerUp_sfx;

    //Controllers
    private PlayerController _playerController;
    private EnemyController _enemyController;

    //Canvas
    private Transform _playerUI;
    private Transform _enemyUI;
    private Text _playerLifes;
    private Text _enemyLifes;

    //Informations
    public bool IsPlayerTurn;
    public bool UpdateBarrier = true;

    void Start()
    {
        //ball
        _delayBallSpammer = 3;

        //controllers
        _playerController = Player.GetComponent<PlayerController>();
        _enemyController = Enemy.GetComponent<EnemyController>();
        //_gameController = GameObject.Find("GameController").GetComponent<GameController>();

        //canvas
        _playerUI = Canvas.transform.Find("PlayerUI");
        _enemyUI = Canvas.transform.Find("EnemyUI");

        _playerLifes = _playerUI.transform.Find("lifes").GetComponent<Text>();
        _enemyLifes = _enemyUI.transform.Find("lifes").GetComponent<Text>();

        for (int i = 0; i < PowerUps_Objects.Count; i++)
            _powerUps.Add(PowerUps_Objects[i], PowerUps_Probability[i]);

        //audio
        var audioSources = GetComponents<AudioSource>();
        _music = audioSources[0];
        _powerUp_sfx = audioSources[1];

        UpdateCanvas();
    }

    void Update()
    {
        if (GameObject.Find("PowerBall(Clone)") == null)
        {
            _delayBallSpammer += Time.deltaTime;

            if (_delayBallSpammer > 3)
            {
                _delayBallSpammer = 0;
                CreateBall(Vector3.zero);
            }

            UpdateCanvas();
        }
        if (!PowerUpActive)
        {
            if (_currentPowerUpDelay > PowerUpDelay)
            {
                GameObject powerUpToSpawn = null;
                Vector2 positionToSpawn;

                //GettingObject
                var n = Random.Range(0, 100);
                var cumulative = 0.0;
                foreach (var powerup in _powerUps)
                {
                    cumulative += powerup.Value;
                    if (n < cumulative)
                    {
                        powerUpToSpawn = powerup.Key;
                        break;
                    }
                }

                //GettingPosition
                var x = Random.Range(-2.5f, 2.5f);
                var y = Random.Range(-2f, 2f);
                positionToSpawn = new Vector2(x, y);

                //finish
                Instantiate(powerUpToSpawn, positionToSpawn, Quaternion.identity);
                PowerUpActive = true;
                _currentPowerUpDelay = 0;
            }
            else
            {
                _currentPowerUpDelay += Time.deltaTime;
            }
        }

        if (UpdateBarrier)
        {
            if (IsPlayerTurn)
            {
                foreach (var barrier in PlayerBarriers)
                    barrier.GetComponent<BarrierController>().ActivateDeactivateBarrier(false);
                foreach (var barrier in EnemyBarriers)
                    barrier.GetComponent<BarrierController>().ActivateDeactivateBarrier(true);
            }
            if (!IsPlayerTurn)
            {
                foreach (var barrier in PlayerBarriers)
                    barrier.GetComponent<BarrierController>().ActivateDeactivateBarrier(true);
                foreach (var barrier in EnemyBarriers)
                    barrier.GetComponent<BarrierController>().ActivateDeactivateBarrier(false);
            }
            UpdateBarrier = false;
        }
    }
    public GameObject CreateBall(Vector3 location)
    {
        var newBall = Instantiate(Ball, location, Quaternion.identity); ;
        return newBall.gameObject;
    }
    public void UpdateCanvas()
    {
        _playerLifes.text = _playerController.Lifes().ToString();
        _enemyLifes.text = _enemyController.Lifes().ToString();
    }

    public void PlayPowerUpSfx(string powerUp)
    {
        _powerUp_sfx.clip = PowerUps_AudioClip.Find(_ => _.name == "sfx_" + powerUp);
        _powerUp_sfx.Play();
    }
}
