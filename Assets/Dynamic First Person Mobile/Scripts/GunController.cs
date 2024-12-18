using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraTransform;
    public int rayHittingRange = 20;
    public AudioClip gunShotSound;
    private Ray forwardRay;

    public ParticleSystem muzzleFlash;

    public GameObject hitEffect;

    

    // Update is called once per frame
    void Update()
    {
        forwardRay = new Ray(cameraTransform.position, cameraTransform.forward);
        Debug.DrawRay(forwardRay.origin, forwardRay.direction * rayHittingRange, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(forwardRay, out hit, rayHittingRange))
        {
            Debug.Log("Hit: " + hit.transform.name);
            // Debug.Log("Hit point: " + hit.point);
        }
        Debug.Log("Hit: " + hit.transform.name);
        muzzleFlash.Play();
        GameObject hitPoint = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(hitPoint, 2f);
        AudioSource.PlayClipAtPoint(gunShotSound, transform.position);
    }
}
