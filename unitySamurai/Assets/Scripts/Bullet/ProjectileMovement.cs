using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private Vector3 direction;

    //used to set the directon of the bullet called in the rifle script to instantiate the bullet and get the component of bulletmovement and set the direction on the middle of the screen (crosshari)
    public void setInitialDirection(Vector3 dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        //incremeneting the bullet position each time so it travels yk
        transform.position += direction * 1.1f * Time.deltaTime;
    }
}
