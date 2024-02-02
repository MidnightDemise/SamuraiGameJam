using EazyCamera.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private Transform Mesh;

    public Transform shootpoint;
    private float shootTimerMax = 1f;
    private float shootTimer;
    private bool isShooting;
    private bool canShoot = true;
    [SerializeField] private TrailRenderer bulletPrefab;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Transform scopeTransform;


    private float elapsedTime = 1f;
    public static bool isLerping = false;
    private float lerpDuration = 0.5f; // Adjust this value based on your needs
    private float lerpTimer = 0f;
    private Vector3 initialPosition; // Store the initial position before starting the lerp


    private Vector3 targetPoint;


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }


        if (Input.GetMouseButton(1))
        {
            if (!isLerping)
            {
                isLerping = true;
                lerpTimer = 0f;
                initialPosition = cameraObject.transform.position; // Store the initial position
            }

            LerpToScopePosition();
        }
        else
        {
            cameraObject.gameObject.GetComponent<Camera>().fieldOfView = 60;
            isLerping = false;
            crossHair.SetActive(true);
        }
    }

    private void LerpToScopePosition()
    {
        if (isLerping)
        {
            lerpTimer += Time.deltaTime;

            float t = lerpTimer / lerpDuration;
            cameraObject.transform.position = Vector3.Lerp(initialPosition, scopeTransform.position, t);

            if (t >= 1.0f)
            {
                cameraObject.transform.position = scopeTransform.position;
                cameraObject.transform.rotation = scopeTransform.rotation;
                cameraObject.gameObject.GetComponent<Camera>().fieldOfView = 20;

                crossHair.SetActive(false);

            }
        }
    }


    public void SetVisibility(bool visible)
    {
        Mesh.gameObject.SetActive(visible);
    }


    public void Shoot()
    {
        //if (!canShoot) return;

        
        // Create a ray from the camera through the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        // Get the point where the ray hits (or extends to)

        if(Physics.Raycast(ray,out RaycastHit hit , 40f))
        {
            targetPoint = hit.point - shootpoint.position;
        }
        else
        {
            targetPoint = ray.GetPoint(100f) - shootpoint.position; // Use a distance that suits your needs

        }

        if (isLerping)
        {
            GameObject temp = Instantiate(bullet, shootpoint.position, transform.rotation);
            temp.GetComponent<testBullet>().setInitialDirection(shootpoint.transform.forward);
        }
        else
        {
            
                GameObject temp = Instantiate(bullet, shootpoint.position, transform.rotation);
                temp.GetComponent<testBullet>().setInitialDirection(targetPoint.normalized);

        }





        //var tracer = Instantiate(bulletPrefab, ray.origin, Quaternion.identity);
        //tracer.AddPosition(shootpoint.transform.position);
       
        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    tracer.transform.position = hit.point;
        //}
        //else
        //{
        //    tracer.transform.position = targetPoint;
        //}
 

        //canShoot = false;


    }
}