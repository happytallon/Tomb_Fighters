using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PaddleBaseController
{
    public string Name;
    public List<Sprite> BracketImages;
    public List<AudioClip> BracketSFX;


    private string _axis = "Horizontal";
    private string _name;
    private Sprite _sprite;
    private AudioSource _bracketSfx;

    public void Start()
    {
        _name = Name;

        _sprite = BracketImages.Find(_ => _.name == _name.ToLower() + "_bracket");
        GetComponent<SpriteRenderer>().sprite = _sprite;

        var sfx = BracketSFX.Find(_ => _.name == _name.ToLower() + "_bracket");
        _bracketSfx = GetComponent<AudioSource>();
        _bracketSfx.clip = sfx;
    }

    void FixedUpdate()
    {
        var x = Input.GetAxis(_axis);
        MoveRacket(x);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Ball") _bracketSfx.Play();
    }
}
