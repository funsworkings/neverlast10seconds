using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerCamera : MonoBehaviour
{
    public Racer racer;
    public Pucca pucca;

    Vector3 camLookAt;

    void Start()
    {
        camLookAt = racer.transform.position;
    }

    void LateUpdate()
    {

        Vector3 targetCamPos = racer.transform.position - Vector3.ProjectOnPlane(racer.transform.forward, Vector3.up) * 10f +
                               Vector3.up * 5f;

        float lerpT = (1 - Mathf.Pow(1 - 0.99f, Time.deltaTime)) * Vector3.Distance(transform.position, targetCamPos);
        transform.position = Vector3.MoveTowards(transform.position, targetCamPos, lerpT);

        Vector3 targetCamLookAt;
        if (!pucca.lookAtMe)
            targetCamLookAt = racer.transform.position;
        else
            targetCamLookAt = (racer.transform.position + pucca.transform.position) / 2;

        lerpT = (1 - Mathf.Pow(1 - 0.99995f, Time.deltaTime)) * Vector3.Distance(camLookAt, targetCamLookAt);
        camLookAt = Vector3.MoveTowards(camLookAt, targetCamLookAt, lerpT);
        transform.rotation = Quaternion.LookRotation(camLookAt - transform.position, Vector3.up);
    }
}
