using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class PowerBallController : MonoBehaviour
{
    public float InitialSpeed;
    private float _speed;
    private bool _speedVariation;

    private PlayerController _playerController;
    private EnemyController _enemyController;

    private AudioSource _sound;

    //power up's
    private int _damageboost = 0;
    private bool _doppelganger { get { return _doppelgangerInitialSpeed != Vector2.zero; } }
    private Vector2 _doppelgangerInitialSpeed;
    private bool _barrier;

    public float pu_DamageExpiration;
    public float pu_SpeedBoostExpiration;
    public float pu_SpeedCurtailExpiration;

    private List<PowerUp> _currentPowerUps = new List<PowerUp>();
    [Serializable]
    private class PowerUp
    {
        public float CurrentTime { get; set; }
        public float ExpirationTime { get; set; }
        public bool Expired { get { return CurrentTime > ExpirationTime; } }
        public Action NormalizeMethod;
        public PowerUp(float exp, Action normalizedMethod)
        {
            ExpirationTime = exp;
            NormalizeMethod = normalizedMethod;
        }
    }

    void Awake()
    {
        SpeedNormalize();
        _sound = GetComponent<AudioSource>();

        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
    }

    void Start()
    {
        ChangeSpeed(!_doppelganger ? Vector2.up : _doppelgangerInitialSpeed);
    }

    void FixedUpdate()
    {
        //normalize power up's
        foreach (var powerup in _currentPowerUps.Where(_ => _.Expired))
            powerup.NormalizeMethod();

        _currentPowerUps = _currentPowerUps.Where(_ => !_.Expired).ToList();

        foreach (var powerup in _currentPowerUps)
            powerup.CurrentTime += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        _sound.Play();

        if (col.collider.tag == "Ball") { Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>()); return; }

        if (col.collider.tag == "Edge") return;

        if (col.collider.tag == "Goal")
        {
            GoalCollision(col);
            return;
        }

        if (col.collider.tag == "Player" || col.collider.tag == "Enemy")
        {
            RacketCollision(col);
            return;
        }
    }
    private void GoalCollision(Collision2D col)
    {
        if (col.collider.name == "up")
            _enemyController.Die(1 + _damageboost);
        if (col.collider.name == "down")
            _playerController.Die(1 + _damageboost);
        Destroy(gameObject);
    }
    private void RacketCollision(Collision2D col)
    {
        var racketFactor = col.collider.tag == "Player" ? 1 : -1;
        var hitFactor = (transform.position.x - col.transform.position.x) / col.collider.bounds.size.x;

        var direction = new Vector2(hitFactor, racketFactor).normalized;

        ChangeSpeed(direction);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PowerUp") SetPowerUp(col.gameObject);
    }

    private void ChangeSpeed()
    {
        ChangeSpeed(GetComponent<Rigidbody2D>().velocity.normalized);
    }
    private void ChangeSpeed(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * _speed;
    }

    #region Power Up's
    private void SetPowerUp(GameObject powerup)
    {
        if (powerup.name == "PowerUp_Dmg+") { SetDamageBoost(); return; }
        if (powerup.name == "PowerUp_Speed+") { SetSpeedBoost(); return; }
        if (powerup.name == "PowerUp_Speed-") { SetSpeedCurtail(); return; }
        if (powerup.name == "PowerUp_Doppelganger") { SetDoppelganger(); return; }
        if (powerup.name == "PowerUp_Barrier") { SetBarrier(); return; }
    }

    #region DAMAGE
    private void SetDamageBoost()
    {
        _damageboost = 1;
        _currentPowerUps.Add(new PowerUp(pu_DamageExpiration, DamageNormalize));
    }
    private void DamageNormalize()
    {
        _damageboost = 0;
    }
    #endregion    
    #region SPEED
    private void SetSpeedBoost()
    {
        _speed *= 1.5f;
        _currentPowerUps.Add(new PowerUp(pu_SpeedBoostExpiration, SpeedNormalize));

        ChangeSpeed();
    }
    private void SetSpeedCurtail()
    {
        _speed /= 1.5f;
        _currentPowerUps.Add(new PowerUp(pu_SpeedCurtailExpiration, SpeedNormalize));

        ChangeSpeed();
    }
    private void SpeedNormalize()
    {
        _speed = InitialSpeed;

        ChangeSpeed();
    }
    #endregion
    #region Doppelganger and Barrier
    private void SetDoppelganger()
    {
        var newBall = GameObject.Find("GameController").GetComponent<GameController>().CreateBall(transform.position);
        var currentVelocity = GetComponent<Rigidbody2D>().velocity.normalized;
        currentVelocity.y *= -1;
        newBall.GetComponent<PowerBallController>()._doppelgangerInitialSpeed = currentVelocity;


        //var direction = GetComponent<Rigidbody2D>().velocity.normalized;
        //direction.y *= -1;
        //newBall.GetComponent<Rigidbody2D>().velocity = direction;
    }
    private void SetBarrier()
    {
    }
    #endregion

    #endregion
}
