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

    //Controllers
    private PlayerController _playerController;
    private EnemyController _enemyController;

    //Canvas
    private Transform _playerUI;
    private Transform _enemyUI;
    private Text _playerLifes;
    private Text _enemyLifes;
    
    // Use this for initialization
    void Start()
    {
        //ball
        _delayBallSpammer = 3;

        //controllers
        _playerController = Player.GetComponent<PlayerController>();
        _enemyController = Enemy.GetComponent<EnemyController>();

        //canvas
        _playerUI = Canvas.transform.Find("PlayerUI");
        _enemyUI = Canvas.transform.Find("EnemyUI");

        _playerLifes = _playerUI.transform.Find("lifes").GetComponent<Text>();
        _enemyLifes = _enemyUI.transform.Find("lifes").GetComponent<Text>();

        UpdateCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("PowerBall(Clone)") == null)
        {
            _delayBallSpammer += Time.deltaTime;

            if (_delayBallSpammer > 3)
            {
                _delayBallSpammer = 0;
                Instantiate(Ball);
            }

            UpdateCanvas();
        }
    }
    private void UpdateCanvas()
    {
        _playerLifes.text = _playerController.Lifes().ToString();
        _enemyLifes.text = _enemyController.Lifes().ToString();
    }
}
