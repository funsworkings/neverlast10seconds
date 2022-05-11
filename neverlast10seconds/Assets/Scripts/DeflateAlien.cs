using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DeflateAlien : MonoBehaviour
{
    [SerializeField]
    private Cloth _alienSkin;

    [SerializeField]
    private Rig _ikRig;


    [SerializeField]
    private Transform _cameraPos;

    public Vector3 newCameraPos;

    [SerializeField]
    private GameObject[] destroyList;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Deflate();
        }
    }

    public void Deflate()
    {
        _cameraPos.position = newCameraPos;
        _ikRig.weight = 0;
        _alienSkin.enabled = true;

        //destroy bubbles
        for(int i =0; i < destroyList.Length; i++)
        {
            Destroy(destroyList[i]);
        }
    }
}
