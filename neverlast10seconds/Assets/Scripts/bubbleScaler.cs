using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleScaler : MonoBehaviour
{
    public float bubbleSize = 0.0f;
    SkinnedMeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
        mesh.SetBlendShapeWeight(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if(bubbleSize < 0f)
        {
            bubbleSize = 0;
        }else if(bubbleSize <= 0.33f)
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

        mesh.SetBlendShapeWeight(2, Mathf.Lerp(0, 100, bubbleSize));

    }


}
