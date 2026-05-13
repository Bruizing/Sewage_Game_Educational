using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickering : MonoBehaviour
{
    public GameObject FL;
    public float FlickSpeed = 0.1f;

    void Start()
    {
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    IEnumerator BlinkText()
    {
        // blink it forever. You can set a terminating condition depending upon your requirement
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            FL.SetActive(true);
            yield return new WaitForSeconds(FlickSpeed);
            FL.SetActive(false);
        }
    }
}