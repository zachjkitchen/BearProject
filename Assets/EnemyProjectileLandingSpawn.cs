using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileLandingSpawn : MonoBehaviour
{

    public GameObject spawnObj;
    public CapsuleCollider col;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
        
        
        
    //}

    private void OnCollisionEnter(Collision collision)
    {
        int collisionLayer = collision.gameObject.layer;
        // Debug.Log(collisionLayer);
        if (collisionLayer == 3 || collisionLayer == 6 || collisionLayer == 7)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = transform.rotation;
            Vector3 position = contact.point;
            Instantiate(spawnObj, position, rotation);
            Destroy(gameObject);
        }
        

    }

}
