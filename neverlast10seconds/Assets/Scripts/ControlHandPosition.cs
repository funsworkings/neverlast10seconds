using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandPosition : MonoBehaviour
{
    [SerializeField]
    private Transform _handTarget;

    [SerializeField]
    public static float amountmousemoved;

    private Vector3 hitCache;
    // Update is called once per frame
    void Update()
    {
        //has to move mouse a certain threshold

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        {
            if (hit.collider.CompareTag("Skin"))
            {
                amountmousemoved += Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
            }
            if (hit.collider.CompareTag("Bubble"))
            {
                Debug.Log("POP");
                amountmousemoved = 0;
            }

            _handTarget.position = Vector3.Lerp(_handTarget.position, hit.point, Time.deltaTime * 15f);
            //_handTarget.rotation = Quaternion.Lerp(_handTarget.rotation, Quaternion.Euler(90, 0, 0) * Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(handDirection), Time.deltaTime * 15f);// * _handTarget.rotation;
            _handTarget.rotation = Quaternion.FromToRotation(_handTarget.rotation * Vector3.down, hit.normal) * _handTarget.rotation;
           // r.rotation = Quaternion.FromToRotation(r.rotation * Vector3.up, groundNormal) * r.rotation


            Shader.SetGlobalVector("_HandPosition", hit.point);

        }
    }
}