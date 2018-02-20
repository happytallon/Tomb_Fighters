using System.Collections;
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
    private Dictionary<GameObject, float> _powerUps = new Dictionary<GameObject, float>();
    public bool PowerUpActive;


    //Controllers
    private PlayerController _playerController;
    private EnemyController _enemyController;
    private GameController _gameController;

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
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();

        //canvas
        _playerUI = Canvas.transform.Find("PlayerUI");
        _enemyUI = Canvas.transform.Find("EnemyUI");

        _playerLifes = _playerUI.transform.Find("lifes").GetComponent<Text>();
        _enemyLifes = _enemyUI.transform.Find("lifes").GetComponent<Text>();

        for (int i = 0; i < PowerUps_Objects.Count; i++)
            _powerUps.Add(PowerUps_Objects[i], PowerUps_Probability[i]);

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
    }
    public GameObject CreateBall(Vector3 location)
    {
        var newBall = Instantiate(Ball, location, Quaternion.identity); ;
        return newBall.gameObject;
    }
    private void UpdateCanvas()
    {
        _playerLifes.text = _playerController.Lifes().ToString();
        _enemyLifes.text = _enemyController.Lifes().ToString();
    }
}
