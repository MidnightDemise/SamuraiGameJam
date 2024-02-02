using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBullet : MonoBehaviour
{

    private Vector3 direction;
    public GameObject shield;
   public  void setInitialDirection(Vector3 dir)
    {
        direction = dir;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * 15f * Time.deltaTime;
        Destroy(gameObject, 3f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RangedEnemy"))
        {
            other.GetComponent<RangedEnemy>().TakeDamage(50f);
            Destroy(gameObject);
        }
        else if(other.CompareTag("MeleeEnemy"))
        {
            other.GetComponent<EnemyScript>().TakeDamage(50f);
            Destroy(gameObject);

        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(1f);

            Destroy(gameObject);

        }
    }
   

}
