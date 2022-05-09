using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAt : MonoBehaviour
{
    public Transform thingToLookAt;

    void Update()
    {
        transform.LookAt(thingToLookAt);
    }
}
