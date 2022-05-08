using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    Rigidbody r;
    public float maxSpeed = 10;
    public float maxAccel = 10;
    public float maxTurnSpeed = 10;
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

    Quaternion groundAlignment = Quaternion.identity;
    void FixedUpdate()
    {
        if (onGround) groundNormal.Normalize();
        else groundNormal = Vector3.up;

        AlignToGround();

        DoVel();

        onGround = false;
        groundNormal = Vector3.zero;
    }

    void AlignToGround()
    {
        r.rotation = Quaternion.Lerp(r.rotation,
                        Quaternion.FromToRotation(r.rotation * Vector3.up, groundNormal) * r.rotation,
                        onGround ? 0.2f : 0.01f);
        //dont love this lerp
    }

    void DoVel()
    {
        vel = r.velocity;
        angVel = r.angularVelocity;

        Vector3 xAxis = ProjectOnGroundPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnGroundPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(vel, xAxis);
        float currentZ = Vector3.Dot(vel, zAxis);

        Vector3 targVel = Vector3.ProjectOnPlane(transform.forward, Vector3.up) * playerInput.y * maxSpeed;

        float newX = Mathf.MoveTowards(currentX, targVel.x, onGround ? maxAccel : maxAccel * .1f);
        float newZ = Mathf.MoveTowards(currentZ, targVel.z, onGround ? maxAccel : maxAccel * .1f);

        vel += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

        if (!onGround) vel.y -= gravity;
        r.velocity = vel;

        float maxTurnAllowed = Mathf.Abs(playerInput.y);
        float turnDir = Mathf.Sign(playerInput.y);
        angVel.y = Mathf.MoveTowards(angVel.y,
                                     playerInput.x * maxTurnSpeed * maxTurnAllowed * turnDir,
                                     onGround ? maxAccel : maxAccel * .1f);
        r.angularVelocity = angVel;
    }


    public bool onGround;
    Vector3 groundNormal;
    void OnCollisionEnter(Collision collision) { EvaluateCollision(collision); }
    void OnCollisionStay(Collision collision) { EvaluateCollision(collision); }
    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= 0.5f)//this should change depending on speed!
            {
                onGround = true;
                groundNormal += normal;
            }
        }
    }

    Vector3 ProjectOnGroundPlane(Vector3 vector)
    {
        return vector - groundNormal * Vector3.Dot(vector, groundNormal);
    }
}


//   <3