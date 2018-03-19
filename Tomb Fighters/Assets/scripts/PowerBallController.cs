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
    private GameController _gameController;
    private PowerBallSpriteController _spriteController;

    private AudioSource _sound;

    //power up's
    private int _damageboost = 0;
    //private bool _doppelganger { get { return _doppelgangerInitialSpeed != Vector2.zero; } }
    //private Vector2 _doppelgangerInitialSpeed;
    public GameObject _barrier;
    public GameObject _barrier_enemy;

    public float pu_DamageExpiration;
    public float pu_SpeedBoostExpiration;
    public float pu_SpeedCurtailExpiration;

    public PowerUpAudioController _powerUpAudioController;

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
        _spriteController = transform.Find("_sprite").GetComponent<PowerBallSpriteController>();
        SpeedNormalize();
        _sound = GetComponent<AudioSource>();
        
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        ChangeSpeed(Vector2.up);
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
        _gameController.UpdateBarrier = true;

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
            _gameController.IsPlayerTurn = col.collider.tag == "Player";
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
        _gameController.UpdateCanvas();
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
        if (powerup.name == "PowerUp_Dmg+(Clone)") { SetDamageBoost(); return; }
        if (powerup.name == "PowerUp_Speed+(Clone)") { SetSpeedBoost(); return; }
        if (powerup.name == "PowerUp_Speed-(Clone)") { SetSpeedCurtail(); return; }
        if (powerup.name == "PowerUp_LifeUp(Clone)") { SetLifeUp(); return; }
        if (powerup.name == "PowerUp_Barrier(Clone)") { SetBarrier(); return; }
    }

    #region DAMAGE
    private void SetDamageBoost()
    {
        _damageboost = 1;
        _currentPowerUps.Add(new PowerUp(pu_DamageExpiration, DamageNormalize));
        _gameController.PlayPowerUpSfx("damage+");
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
        _spriteController.rotationBoost = 2;
        _gameController.PlayPowerUpSfx("speed+");

        ChangeSpeed();
    }
    private void SetSpeedCurtail()
    {
        _speed /= 1.5f;
        _currentPowerUps.Add(new PowerUp(pu_SpeedCurtailExpiration, SpeedNormalize));
        _spriteController.rotationBoost = 0.5f;
        _gameController.PlayPowerUpSfx("speed-");

        ChangeSpeed();
    }
    private void SpeedNormalize()
    {
        _speed = InitialSpeed;
        _spriteController.rotationBoost = 1f;

        ChangeSpeed();
    }
    #endregion
    #region LifeUp and Barrier
    private void SetLifeUp()
    {
        if (_gameController.IsPlayerTurn)
            if (_playerController.Lifes() < 5) _playerController.LifeUp(1);
        else
            if (_enemyController.Lifes() < 5) _enemyController.LifeUp(1);

        _gameController.PlayPowerUpSfx("lifeUp");

        _gameController.UpdateCanvas();
    }
    private void SetBarrier()
    {
        var direction = GetComponent<Rigidbody2D>().velocity.normalized;

        CreateBarrier(direction.y > 0 ? 0 : 1);
    }
    private void CreateBarrier(int player)
    {
        if (player == 0 && _playerController.BarrierActive || player == 1 && _enemyController.BarrierActive) return;

        //setup
        var barrierPositionsSetup = player == 0 ?
            new List<BarrierPosition>() {
                new BarrierPosition(-2.25f,-2.75f,-1.25f,-1.75f),
                new BarrierPosition(-0.25f,-2.5f,0.25f,-1.5f),
                new BarrierPosition(1.25f,-2.75f,2.25f,-1.75f),
            } :
            new List<BarrierPosition>() {
                new BarrierPosition(-2.25f,2.75f,-1.25f,1.75f),
                new BarrierPosition(-0.25f,2.5f,0.25f,1.5f),
                new BarrierPosition(1.25f,2.75f,2.25f,1.75f),
            };

        var barrierPositions = new List<Vector2>();

        barrierPositionsSetup.ForEach(_ =>
        {
            var position_x = UnityEngine.Random.Range(_.MinPosition.x, _.MaxPosition.x);
            var position_y = UnityEngine.Random.Range(_.MinPosition.y, _.MaxPosition.y);
            barrierPositions.Add(new Vector2(position_x, position_y));
        });

        barrierPositions.ForEach(_ =>
        {
            var barrier = Instantiate(player == 0 ? _barrier : _barrier_enemy, _, Quaternion.identity);
            if (player == 0) _gameController.PlayerBarriers.Add(barrier); else _gameController.EnemyBarriers.Add(barrier);
        });

        if (player == 0)
            _playerController.BarrierActive = true;
        else
            _enemyController.BarrierActive = true;

        _gameController.PlayPowerUpSfx("barrier");
    }
    private class BarrierPosition
    {
        public Vector2 MinPosition { get; set; }
        public Vector2 MaxPosition { get; set; }
        public BarrierPosition(float min_x, float min_y, float max_x, float max_y)
        {
            MinPosition = new Vector2(min_x, min_y);
            MaxPosition = new Vector2(max_x, max_y);
        }
    }
    #endregion



    #endregion
}
