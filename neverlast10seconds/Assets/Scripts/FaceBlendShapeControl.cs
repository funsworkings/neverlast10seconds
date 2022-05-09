using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBlendShapeControl : MonoBehaviour
{
    SkinnedMeshRenderer mesh;
    float timer;
    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
        mesh.SetBlendShapeWeight(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        mesh.SetBlendShapeWeight(0, 50 + (20f*Mathf.Sin(timer * (.1f * ControlHandPosition.amountmousemoved))));
    }
}
