using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBlendShapeControl : MonoBehaviour
{
    SkinnedMeshRenderer mesh;
    float timer;
    public CumMeter _cm;
    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
        mesh.SetBlendShapeWeight(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        mesh.SetBlendShapeWeight(0, 100*_cm.currentCumValue);
    }
}
