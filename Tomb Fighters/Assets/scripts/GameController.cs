using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject ball;

    private float _delayBallSpammer;
    private PlayerController _playerController;
    private EnemyController _enemyController;

    private GameObject _playerUI;
    private Text _playerLifes;

    private GameObject _enemyUI;
    private Text _enemyLifes;


    // Use this for initialization
    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerUI = GameObject.Find("PlayerUI");
        _playerLifes = _playerUI.transform.Find("lifes").GetComponent<Text>();

        _enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        _enemyUI = GameObject.Find("EnemyUI");
        _enemyLifes = _enemyUI.transform.Find("lifes").GetComponent<Text>();

        //_playerLifes.text = _playerController.lifes.ToString();
        //_enemyLifes.text = _enemyController.lifes.ToString();

        _delayBallSpammer = 3;
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
                Instantiate(ball);
            }
        }
    }
}
