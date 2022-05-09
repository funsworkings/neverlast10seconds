using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerCamera : MonoBehaviour
{
    public Racer racer;

    void LateUpdate()
    {
        Vector3 targetCamPos = racer.transform.position - Vector3.ProjectOnPlane(racer.transform.forward, Vector3.up) * 10f +
                               Vector3.up * 5f;

        float lerpT = (1 - Mathf.Pow(1 - 0.99f, Time.deltaTime)) * Vector3.Distance(transform.position, targetCamPos);
        transform.position = Vector3.MoveTowards(transform.position, targetCamPos, lerpT);

        Vector3 targetCamLookAt = racer.transform.position;
        transform.rotation = Quaternion.LookRotation(targetCamLookAt - transform.position, Vector3.up);
    }
}
