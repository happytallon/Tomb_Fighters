using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBaseController : MonoBehaviour
{
    public int InitialLifes = 5;
    public float Speed;

    protected int _lifes;
    public int Lifes() { return _lifes; }

    void Awake()
    {
        _lifes = InitialLifes;
    }
    protected void MoveRacket(float _velocity)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(_velocity, 0) * Speed;
    }
    protected void MoveRacketByPosition(Vector2 pos, float lerp)
    {
        transform.position = Vector2.Lerp(transform.position, pos, lerp);
    }
    public void Die(int lifes)
    {
        _lifes -= lifes;
    }
    
}
