using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _bubble;

    private Mesh _spawnArea;

    public CumMeter cm;

    public Vector2 _scaleRange;

    void Start()
    {
        _spawnArea = GetComponent<MeshFilter>().sharedMesh;
        
        //Vector3 worldPt = transform.TransformPoint(_spawnArea.vertices[54]);
       // Vector3 worldRot = transform.TransformDirection(_spawnArea.normals[54]);
        //Instantiate(_bubble, worldPt, Quaternion.Euler(worldRot * 90));

        
        for (int i = 0; i < _spawnArea.vertexCount; i++)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                Vector3 worldPt = transform.TransformPoint(_spawnArea.vertices[i]);
                Vector3 worldRot = transform.TransformDirection(_spawnArea.normals[i]);
                GameObject clone = Instantiate(_bubble, worldPt, Quaternion.identity);
                clone.GetComponent<bubbleScaler>()._cm = cm;
                clone.transform.forward = worldRot;
                clone.transform.localScale = Vector3.one * Random.Range(_scaleRange.x, _scaleRange.y);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
