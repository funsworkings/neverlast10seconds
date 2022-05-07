using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandPosition : MonoBehaviour
{
    [SerializeField]
    private Transform _handTarget;



    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        {
            Debug.Log(hit.point);
            _handTarget.position = hit.point;
            _handTarget.rotation = Quaternion.Euler(90, 0, 0) * Quaternion.FromToRotation(Vector3.up, hit.normal);// * _handTarget.rotation;
        }
    }
}
