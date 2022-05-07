using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    Rigidbody r;
    public float maxSpeed = 10;
    public float maxAccel = 10;
    public float maxTurn = 10;
    public float gravity = 10;

    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    Vector3 vel, angVel;
    Vector2 playerInput;

    void Update()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
    }

    void FixedUpdate()
    {
        vel = r.velocity;
        Vector3 fwd = transform.TransformDirection(transform.forward);
        vel = Vector3.MoveTowards(vel, transform.forward * playerInput.y * maxSpeed, maxAccel);
        if (!onGround) vel.y -= gravity;
        r.velocity = vel;

        angVel = r.angularVelocity;
        angVel.y = Mathf.MoveTowards(angVel.y, playerInput.x * maxTurn, maxAccel);
        r.angularVelocity = angVel;

        onGround = false;
    }
    public bool onGround;
    void OnCollisionEnter(Collision collision) { EvaluateCollision(collision); }
    void OnCollisionStay(Collision collision) { EvaluateCollision(collision); }
    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}


//   <3