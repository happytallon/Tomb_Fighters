using UnityEngine;

public class PowerBallController : MonoBehaviour
{
    public float speed = 1;

    private PlayerController _playerController;
    private EnemyController _enemyController;

    private AudioSource _sound;

    void Awake()
    {
        _sound = GetComponent<AudioSource>();

        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
    }

    void Start()
    {        
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D col)
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
        }
    }
    private void GoalCollision(Collision2D col)
    {
        if (col.collider.name == "up")
            _enemyController.Die();
        if (col.collider.name == "down")
            _playerController.Die();
        Destroy(gameObject);        
    }
    private void RacketCollision(Collision2D col)
    {
        var racketFactor = col.collider.tag == "Player" ? 1 : -1;
        var hitFactor = (transform.position.x - col.transform.position.x) / col.collider.bounds.size.x;

        var direction = new Vector2(hitFactor, racketFactor).normalized;

        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
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
