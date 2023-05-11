using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public GameObject projectile;

    public float projectileForce = 400f;
    public int fireRateSeconds = 10;
    public int projectileMax = 10;
    int fireRateCounter = 50;
    int projectileCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireRateCounter -= 1;

        // Debug.Log(fireRateCounter);
        if (fireRateCounter == 0 && projectileCount <= projectileMax)
        {
            GameObject firedProjectileObj = Instantiate(projectile, transform.position + new Vector3(0f, 3f, 0f), transform.rotation);
            Rigidbody projectileRB = firedProjectileObj.GetComponent<Rigidbody>();
            projectileRB.AddForce(new Vector3(Random.Range(-1f, 1f), -1f, Random.Range(-1f, 1f)) * -projectileForce);
            // projectileRB.AddForce(new Vector3(0f, 0f, 1f) * -projectileForce);

            // Destroy(firedProjectileObj, 2);
            fireRateCounter = fireRateSeconds;
            projectileCount += 1;
        }
        //shoot block

        


    }
}
