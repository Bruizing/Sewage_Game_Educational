using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform GunFirePoint;
    public AudioSource ShotSFX;//from firepoint
    public GameObject bullet;
    public GameObject GameOverBool;
    private bool canShoot = true;
    private float lastClickTime = 0f;
    private float doubleClickDelay = 0f;//time between shots
    public bool hasShot = false;//Bool for Death

    public bool isEnabled = true;
    private float shotDelay = 0;
    private bool doubleShotReady = false;
    private bool shotPrimed = false;

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameObject != null)
        {
            if (GameOverBool.activeSelf == false && isEnabled)//allows shooting when gameover hasn't happned
            {
                shotDelay += Time.deltaTime; // checks how long since we shot last

                if (shotDelay > 0.5f) // if its been .5s since we shot, allow us to shoot again
                {
                    canShoot = true;
                    hasShot = false;
                    shotDelay = 0;
                }

                if (hasShot) // checks if we're within the double shot time frame
                {
                    doubleClickDelay += Time.deltaTime;
                }
                if (shotPrimed && doubleClickDelay > 0.15) // if we've clicked twice within the timeframe of .15, shoot the second shot
                {
                    ShotSFX.Play();
                    Instantiate(bullet, GunFirePoint.position, Quaternion.identity);
                    shotPrimed = false;
                    doubleClickDelay = -1000;

                }
                if (doubleClickDelay < 0.15 && doubleClickDelay >= 0 && Input.GetMouseButtonDown(0)) // prime the shot if we click within the timeframe
                {
                    shotPrimed = true;
                    // move second shot here
                }

                if (Input.GetMouseButtonDown(0) && canShoot) // standard shooting code: if able to shoot, and we click, try to shoot
                {
                    shotDelay = 0;
                    hasShot = true;

                    float timeSinceLasClick = Time.time - lastClickTime;

                   if (canShoot)
                   {
                        canShoot = false;

                        ShotSFX.Play();
                        Instantiate(bullet, GunFirePoint.position, Quaternion.identity);//Shooting Logic
                        doubleClickDelay = 0;
                   }
                   lastClickTime = Time.time;// Record time of this click
                }
            }
        }
    }
}
