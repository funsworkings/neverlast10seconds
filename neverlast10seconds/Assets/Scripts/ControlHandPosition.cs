using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandPosition : MonoBehaviour
{
    [SerializeField]
    private Transform _handTarget;


    private Vector3 hitCache;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        {
            Debug.Log(hit.point);
            Vector3 handDirection = hitCache - hit.point;
            if (hit.point != hitCache)
            {
                hitCache = hit.point;
            }
            _handTarget.position = Vector3.Lerp(_handTarget.position, hit.point, Time.deltaTime * 15f);
            _handTarget.rotation = Quaternion.Lerp(_handTarget.rotation, Quaternion.Euler(90, 0, 0) * Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(handDirection), Time.deltaTime * 15f);// * _handTarget.rotation;

            

            
            
            
            
            Shader.SetGlobalVector("_HandPosition", hit.point);
            Shader.SetGlobalVector("_HandDirection", handDirection);
        }
    }
}
