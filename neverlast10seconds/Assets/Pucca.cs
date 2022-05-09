using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pucca : MonoBehaviour
{
    public Racer garu;
    public float chaseSpeed;
    public bool lookAtMe;
    public GameObject loveTitle;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (dToGaru < 5)
        {
            tAtGaru += Time.deltaTime;
            loveTitle.SetActive(true);
        }
        else
        {
            tAtGaru = 0;
            loveTitle.SetActive(false);
        }

        if (tAtGaru > 2)
        {
            cumOS.Overworld.BrowserController.Instance.DisableMinigameWindow(3);
        }
    }

    [ContextMenu("close!")]
    public void DebugClose()
    {
        cumOS.Overworld.BrowserController.Instance.DisableMinigameWindow(3);
    }

    Vector3 targPos;
    float tAtGaru;
    float dToGaru;
    void FixedUpdate()
    {
        dToGaru = Vector3.Distance(transform.position, garu.transform.position);

        float d = Vector3.Dot(garu.transform.forward, (transform.position - garu.transform.position).normalized);
        Vector3 newTargPos;
        if (d < 0.9f && dToGaru > 7)
        {
            newTargPos = garu.transform.position + garu.transform.forward * 25;
            lookAtMe = false;
        }
        else
        {
            newTargPos = garu.transform.position;
            lookAtMe = true;
        }
        targPos = Vector3.Lerp(targPos, newTargPos, 0.4f);
        transform.position = Vector3.MoveTowards(transform.position, targPos, chaseSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.LookRotation(targPos - transform.position, Vector3.up),
                                             0.1f);

        Vector3 scale = Vector3.one * 2;
        scale += Vector3.one * (Mathf.Sin(Time.time * Remaps.EaseInQuad(dToGaru, 200, 3, 0, 4)) + 1);
        transform.localScale = scale;

    }
}
