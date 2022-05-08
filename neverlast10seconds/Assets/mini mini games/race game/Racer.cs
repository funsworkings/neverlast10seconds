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
        DoTapSpeed();
    }

    Quaternion groundAlignment = Quaternion.identity;
    void FixedUpdate()
    {
        if (OnGround)
        {
            if (groundContactCount > 1)
                groundNormal.Normalize();
        }
        else groundNormal = transform.up;

        AlignToGround();

        DoVel();

        groundContactCount = 0;
        groundNormal = Vector3.zero;
    }

    void AlignToGround()
    {
        r.rotation = Quaternion.Lerp(r.rotation,
                        Quaternion.FromToRotation(r.rotation * Vector3.up, groundNormal) * r.rotation,
                        OnGround ? .3f : 0.05f);
        //dont love this lerp
        //also gotta do something about the jitter
    }
    float speed;
    void DoVel()
    {
        vel = r.velocity;
        angVel = r.angularVelocity;

        Vector3 xAxis = ProjectOnGroundPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnGroundPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(vel, xAxis);
        float currentZ = Vector3.Dot(vel, zAxis);


        float angVelChange = Mathf.Abs(angVel.y) - Mathf.Abs(prevAngVel.y);
        if (angVelChange > 0.1f)
            speed *= Remaps.EaseInQuad(angVelChange, 0, 8, 1, 0);

        float targSpeed = maxSpeed * fwdTapSpeed;// Mathf.Max(playerInput.y, -0.15f);
        speed = Mathf.MoveTowards(speed, targSpeed, OnGround ? maxAccel : maxAccel * .1f);
        Vector3 targVel = Vector3.ProjectOnPlane(transform.forward, Vector3.up) * speed;
        vel += xAxis * (targVel.x - currentX) + zAxis * (targVel.z - currentZ);

        // Vector3 targVel = Vector3.ProjectOnPlane(transform.forward, groundNormal) * playerInput.y * maxSpeed;

        // float newX = Mathf.MoveTowards(currentX, targVel.x, OnGround ? maxAccel : maxAccel * .1f);
        // float newZ = Mathf.MoveTowards(currentZ, targVel.z, OnGround ? maxAccel : maxAccel * .1f);

        // vel += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);



        if (!OnGround) vel.y -= gravity;
        r.velocity = vel;

        float maxTurnAllowed = 1;// Mathf.Abs(fwdTapSpeed);
        float turnDir = Mathf.Sign(fwdTapSpeed);
        angVel.y = Mathf.MoveTowards(angVel.y,
                                    (rightTapSpeed - leftTapSpeed) * maxTurnSpeed * maxTurnAllowed * turnDir,
                                     OnGround ? maxAccel : maxAccel * .1f);
        r.angularVelocity = prevAngVel = angVel;

    }
    float fwdTapSpeed, leftTapSpeed, rightTapSpeed;
    public float tapSpeedIncrement, tapSpeedSlowDown;
    void DoTapSpeed()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) fwdTapSpeed += tapSpeedIncrement;
        fwdTapSpeed -= Time.deltaTime * tapSpeedSlowDown;
        fwdTapSpeed = Mathf.Max(fwdTapSpeed, 0);


        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) leftTapSpeed += tapSpeedIncrement;
        leftTapSpeed -= Time.deltaTime * tapSpeedSlowDown;
        leftTapSpeed = Mathf.Max(leftTapSpeed, 0);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) rightTapSpeed += tapSpeedIncrement;
        rightTapSpeed -= Time.deltaTime * tapSpeedSlowDown;
        rightTapSpeed = Mathf.Max(rightTapSpeed, 0);

        // print("fwd: " + fwdTapSpeed + ", r: " + rightTapSpeed + ", l: " + leftTapSpeed);
    }

    Vector3 prevAngVel;


    int groundContactCount;
    bool OnGround => groundContactCount > 0;
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
                groundContactCount += 1;
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