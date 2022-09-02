using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{

    public float yTarget;
    public float speed = 100f;
    public ParticleSystem _particles;

    public static float cryingStrength;
    public Transform headAngle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            yTarget = 180f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            yTarget = 135f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            yTarget = 90f;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            yTarget = 45f;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            yTarget = 0f;
        }

        transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, new Vector3(0, yTarget, 0), Time.deltaTime * speed);
        
        headAngle.localRotation = Quaternion.Lerp(Quaternion.Euler(headAngle.localEulerAngles), Quaternion.Euler(new Vector3(0, 0, -18f*cryingStrength)), Time.deltaTime);
        ParticleSystem.EmissionModule emision = _particles.emission;
        emision.rateOverTime = 10 * (1 + cryingStrength);
        //_particles.emission = emision;
    }
}
