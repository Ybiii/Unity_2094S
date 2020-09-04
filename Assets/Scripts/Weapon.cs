using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponProperties weaponProperties;
    public Transform shotPoint;
    public Transform targetLook;

    public GameObject cameraMain;
    public GameObject decal;
    public GameObject bullet;

    AudioSource audioSource;
    public AudioClip shootClip;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        shotPoint.LookAt(targetLook);//чтобы стрелять прямо в цель

        Vector3 origin = shotPoint.position;
        Vector3 dir = targetLook.position;

        RaycastHit hit;

        //decal.SetActive(false);
       // Debug.DrawLine(origin, dir, Color.black);
       // Debug.DrawLine(cameraMain.transform.position, dir, Color.black);

        //if (Physics.Linecast(origin, dir, out hit))
        //{
        //    //decal.SetActive(true);

        //    decal.transform.position = hit.point + hit.normal * 0.01f;
        //    decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
        //}
    }

    public void Shoot()
    {
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        audioSource.PlayOneShot(shootClip);
    }
}
