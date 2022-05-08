using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlHandPosition : MonoBehaviour
{
    [SerializeField]
    private Transform _handTarget;
    public static float amountmousemoved;

    

    //public static float cumAmount; 

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
                amountmousemoved = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
            }
            if (hit.collider.CompareTag("Bubble"))
            {
                //Debug.Log("POP");
                //amountmousemoved;
            }

           // Debug.Log("amount mouse moved: " + amountmousemoved.ToString());

            
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
