using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBallSpriteController : MonoBehaviour
{
    public float rotationBoost;

    private Rigidbody2D objectRigidbody;
    private float direction { get { return objectRigidbody.velocity.x > 0 ? -1 : 1; } }

    void Start()
    {
        objectRigidbody = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float rotAmount = 360 * Time.deltaTime;
        float curRot = transform.localRotation.eulerAngles.z;
        float rotationSpeed = 5 * direction * rotationBoost;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount * rotationSpeed));
    }
}
