using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class love : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = Vector3.one * (Mathf.Sin(Time.time * 3f) + 3.5f) * 0.4f;

    }
}
