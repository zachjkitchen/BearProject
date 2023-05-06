using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectile;

    public float projectileForce = 400f;
    public int fireRateSeconds = 10;
    int fireRateCounter = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireRateCounter -= 1;

        // Debug.Log(fireRateCounter);
        if (fireRateCounter == 0)
        {
            GameObject firedProjectileObj = Instantiate(projectile, transform.position, transform.rotation);
            Rigidbody projectileRB = firedProjectileObj.GetComponent<Rigidbody>();
            projectileRB.AddForce(transform.forward * projectileForce);
            projectileRB.AddTorque(transform.right * Random.Range(-50f, 50f));
            projectileRB.AddTorque(transform.up * Random.Range(-50f, 50f));
            Destroy(firedProjectileObj, 2);
            fireRateCounter = fireRateSeconds;
        }
        //shoot block

        


    }
}
