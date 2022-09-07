using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _bubble;

    private Mesh _spawnArea;

    public CumMeter cm;

    public Vector2 _scaleRange;

    private List<GameObject> active_bubbles = new List<GameObject>();
    private List<GameObject> inactive_bubbles = new List<GameObject>();

    public int bubbleCount = 100;

    void Start()
    {
        _spawnArea = GetComponent<MeshFilter>().sharedMesh;

        var free_meshPairs = new List<MeshPair>();

        bubbleCount = Mathf.Min(bubbleCount, _spawnArea.vertexCount);
        
        List<int> verts = new List<int>();
        for (int i = 0; i < bubbleCount; i++)
        {
            int v = Random.Range(0, _spawnArea.vertexCount);
            while (verts.Contains(v))
            {
                v = Random.Range(0, _spawnArea.vertexCount); // Wait for unused vert
            }
            verts.Add(v);

            free_meshPairs.Add(new MeshPair()
            {
                pos = _spawnArea.vertices[v],
                normal = _spawnArea.normals[v]
            });
        }

        for (int i = 0; i < free_meshPairs.Count; i++)
        {
            DestroyBubble(InitBubble(free_meshPairs[i]));
        }

        StartCoroutine(Loop());
    }

    private int threshold = 0;

    IEnumerator Loop()
    {
        while (true)
        {
            // Eval cum state

            int t = Mathf.RoundToInt(cm.cumInterval * bubbleCount); Debug.LogWarning($"prev: {threshold} curr: {t}");
            int t_diff = t - threshold;
            threshold = t;
            
            if (t_diff != 0)
            {
                int st_diff = Mathf.Abs(t_diff);
                for (int i = 0; i < st_diff; i++)
                {
                    if (t_diff < 0) // Destroy
                    {
                        DestroyBubble();
                    }
                    else // Spawn
                    {
                        SpawnBubble();
                    }
                }
            }
            
            for(int i = 0; i < 5; i++) yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    [System.Serializable]
    public struct MeshPair
    {
        public int index;
        public Vector3 pos;
        public Vector3 normal;
    }

    GameObject InitBubble(MeshPair loc)
    {
        Vector3 worldPt = transform.TransformPoint(loc.pos);
        Vector3 worldRot = transform.TransformDirection(loc.normal);
        
        GameObject clone = Instantiate(_bubble, worldPt, Quaternion.identity);
        bubbleScaler scaler = clone.GetComponent<bubbleScaler>();

        scaler.meshIndex = loc.index;
        scaler._cm = cm;
        
        clone.transform.forward = worldRot;
        clone.transform.localScale = Vector3.one * Random.Range(_scaleRange.x, _scaleRange.y);
        clone.transform.SetParent(transform);

        return clone;
    }

    void SpawnBubble(GameObject bubble = null)
    {
        if (bubble == null)
        {
            var inactive_count = inactive_bubbles.Count;
            if (inactive_count > 0)
            {
                // Recycle existing bubble
                var _bubble = inactive_bubbles[Random.Range(0, inactive_count)];
                SpawnBubble(_bubble);
                return;
            }
        }
        else
        {
            bubble.SetActive(true);

            if (inactive_bubbles.Contains(bubble))
            {
                inactive_bubbles.Remove(bubble);
            }

            if (!active_bubbles.Contains(bubble))
            {
                active_bubbles.Add(bubble);
            }

            return;
        }

        Debug.LogWarning("Failed to spawn bubble");
    }

    void DestroyBubble(GameObject bubble = null)
    {
        if (bubble == null)
        {
            if (active_bubbles.Count > 0)
            {
                var b = active_bubbles[Random.Range(0, active_bubbles.Count)];
                DestroyBubble(b);
                
                return;
            }
        }
        else
        {
            bubble.SetActive(false);
            
            if (active_bubbles.Contains(bubble))
            {
                active_bubbles.Remove(bubble);
            }
            
            if (!inactive_bubbles.Contains(bubble))
            {
                inactive_bubbles.Add(bubble);
            }

            return;
        }

        Debug.LogWarning("Failed to destroy bubble: " + bubble.GetInstanceID().ToString());
    }
}
