using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : MonoBehaviour
{
    public float life = 10f;
    public ParticleSystem smoke, fire;
    public GameObject nugget;
    float timer = 3f;

    public Vector3 targetPos;

    public Transform[] chickPositions;
    private void Start()
    {
        transform.position = chickPositions[Random.Range(0, chickPositions.Length)].position;
        targetPos = chickPositions[Random.Range(0, chickPositions.Length)].position;
        transform.LookAt(targetPos);
        transform.localEulerAngles += new Vector3(0, 180f, 0);

        
    }


    void Death()
    {
        ChickenController.cryingStrength += 1f;
        Instantiate(nugget, transform.position, Random.rotation);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        life -= Time.deltaTime;
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            //move chick
            targetPos = chickPositions[Random.Range(0, chickPositions.Length)].position;
            timer = life/2f;
            transform.LookAt(targetPos);
            transform.localEulerAngles += new Vector3(0, 180f, 0);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);

        if(life <= 0)
        {
            Death();
        }
        
        if(life < 5f)
        {
            //smoke
            if(!smoke.isPlaying)
                smoke.Play();

        }
        else
        {
            if(smoke.isPlaying)
                smoke.Stop();
        }

        if(life < 2f)
        {
            //fire
            if(!fire.isPlaying)
                fire.Stop();
        }
        else { 
            if(fire.isPlaying)
                fire.Pause(); 
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tear"))
        {
            Debug.Log("Saved chick");
            life = 10f;
        }
    }
}
