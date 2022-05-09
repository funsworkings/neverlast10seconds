using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleScaler : MonoBehaviour
{
    [Range(0, 1)]
    public float bubbleSize = 0.0f;

    SkinnedMeshRenderer mesh;
    CapsuleCollider cCollider;
    private float blendOffset;
    private float blendSpeed;
    private Vector3 scaleCache;
    float timer;
    public CumMeter _cm;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
        cCollider = GetComponent<CapsuleCollider>();
        mesh.SetBlendShapeWeight(0, 100);
        scaleCache = transform.localScale;
        blendOffset = Random.Range(0f, 1f);
        blendSpeed = Random.Range(.5f, 2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bubbleSize < 0f)
        {
            bubbleSize = 0;
        }
        else if(bubbleSize <= 0.33f)
        {
            //interpolates the bubbleSize value to change the shapeweights
            mesh.SetBlendShapeWeight(1, Mathf.Lerp(0, 100, bubbleSize * 3));
            mesh.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, bubbleSize * 3));
        } else if(bubbleSize <= 1f)
        {
            mesh.SetBlendShapeWeight(1, Mathf.Lerp(100, 25, bubbleSize));
        } else if(bubbleSize > 1)
        {
            bubbleSize = 1;

            
        }

        //bubbleSize += Mathf.Sin((Time.time + blendOffset) * blendSpeed);

        cCollider.center = Vector3.Lerp(new Vector3(0, 0, -0.007300895f), new Vector3(0, 0, -0.0003007389f), bubbleSize);
        cCollider.radius = Mathf.Lerp(0.01011366f, 0.01146688f, bubbleSize);
        cCollider.height = Mathf.Lerp(0.02022731f, 0.03422762f, bubbleSize);

        mesh.SetBlendShapeWeight(2, Mathf.Lerp(0, 100, bubbleSize));

        timer += Time.deltaTime * blendSpeed * _cm.currentCumValue;

        bubbleSize = _cm.currentCumValue + (((Mathf.Sin(blendOffset + timer) + 1f)/2f) * .2f);
        

        //transform.localScale = scaleCache * Mathf.Sin(Time.time + transform.position.x);
    }


}
