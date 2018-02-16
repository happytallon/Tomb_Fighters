using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerBallController : MonoBehaviour
{
    public float InitialSpeed;
    private float _speed;
    private bool _speedVariation;

    private PlayerController _playerController;
    private EnemyController _enemyController;

    private AudioSource _sound;

    //power up's
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
    private int _damageboost = 0;
    private bool _doppelganger;
    private bool _barrier;

    public float pu_DamageExpiration;
    public float pu_SpeedBoostExpiration;
    public float pu_SpeedCurtailExpiration;

    private IEnumerable<PowerUp> _currentPowerUps;

    void Awake()
    {
        _sound = GetComponent<AudioSource>();

        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * InitialSpeed;
    }

    void FixedUpdate()
    {
        //normalize power up's
        foreach (var powerup in _currentPowerUps.Where(_ => _.Expired))
            powerup.NormalizeMethod();

        _currentPowerUps.ToList().RemoveAll(_ => _.Expired);

        foreach (var powerup in _currentPowerUps)
            powerup.CurrentTime += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        _sound.Play();

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

        GetComponent<Rigidbody2D>().velocity = direction * _speed;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PowerUp")
        {
            SetPowerUp(col.gameObject);
        }
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

    //DAMAGE
    private void SetDamageBoost()
    {
        _damageboost = 1;
        _currentPowerUps.Concat(new[] { new PowerUp(pu_DamageExpiration, DamageNormalize) });
    }
    private void DamageNormalize()
    {
        _damageboost = 0;
    }
    //SPEED
    private void SetSpeedBoost()
    {
        _speed *= 2;
        _currentPowerUps.Concat(new[] { new PowerUp(pu_SpeedBoostExpiration, SpeedNormalize) });
    }
    private void SetSpeedCurtail()
    {
        _speed /= 2;
        _currentPowerUps.Concat(new[] { new PowerUp(pu_SpeedCurtailExpiration, SpeedNormalize) });
    }
    private void SpeedNormalize()
    {
        _speed = InitialSpeed;
    }

    private void SetDoppelganger()
    {
        var newBall = Instantiate(gameObject);
        var currentVelocity = GetComponent<Rigidbody2D>().velocity;
        newBall.GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x * -1, currentVelocity.y * -1);
    }
    private void SetBarrier()
    {
    }
    #endregion


    /*
    public float xspeed = 2f;
    public float yspeed = 2f;

    private float _ydirection = 1f;
    private float _xdirection = 1f;

    private float _maxPaddleAngleReflect = 0.75f;
    private float _xmod = 0f;
    private System.Random _enemyReflect;

    private GameObject _player;
    private SpriteRenderer _playerSprite;


    private GameObject _enemy;
    private SpriteRenderer _enemySprite;




    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerSprite = _player.GetComponent<SpriteRenderer>();


        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        _enemySprite = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SpriteRenderer>();
        _enemyController = _enemy.GetComponent<EnemyController>();
        _enemyReflect = new System.Random();


    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + xspeed * _xmod, transform.position.y + yspeed * _ydirection);
    }


    */
}
