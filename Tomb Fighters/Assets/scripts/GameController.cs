using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject ball;

    private float _delayBallSpammer;
    private PlayerController _playerController;
    private EnemyController _enemyController;

    // Use this for initialization
    void Start () {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("PowerBall") == null && GameObject.Find("PowerBall(Clone)") == null)
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
