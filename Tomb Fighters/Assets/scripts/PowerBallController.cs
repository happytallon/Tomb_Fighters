using UnityEngine;

public class PowerBallController : MonoBehaviour
{
    public float xspeed = 2f;
    public float yspeed = 2f;

    private float _ydirection = 1f;
    private float _xdirection = 1f;

    private float _maxPaddleAngleReflect = 0.75f;
    private float _xmod = 0f;
    private System.Random _enemyReflect;

    private GameObject _player;
    private SpriteRenderer _playerSprite;
    private PlayerController _playerController;

    private GameObject _enemy;
    private SpriteRenderer _enemySprite;
    private EnemyController _enemyController;

    private AudioSource _sound;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerSprite = _player.GetComponent<SpriteRenderer>();
        _playerController = _player.GetComponent<PlayerController>();

        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        _enemySprite = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SpriteRenderer>();
        _enemyController = _enemy.GetComponent<EnemyController>();
        _enemyReflect = new System.Random();

        _sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + xspeed * _xmod, transform.position.y + yspeed * _ydirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _sound.Play();

        if (collision.collider.tag == "Edge") { _xmod *= -1; return; }
        if (collision.collider.tag == "Goal")
        {
            if (collision.collider.name == "up")
                _enemyController.lifes -= 1;
            if (collision.collider.name == "down")
                _playerController.lifes -= 1;
            Destroy(gameObject);
            return;
        }
        if (collision.collider.tag == "Player" || collision.collider.tag == "Enemy")
        {
            _ydirection *= -1;

            var collisionPosition = transform.position.x - collision.collider.transform.position.x;
            var angle = (_maxPaddleAngleReflect * collisionPosition) / (_playerSprite.size.x / 2);

            var paddleSpeedMod = (1 - _maxPaddleAngleReflect) * (collision.collider.tag == "Player" ? Input.GetAxis("Horizontal") : _enemyReflect.Next(0, 100) / 100);

            _xmod = angle + paddleSpeedMod;
        }
    }
}
