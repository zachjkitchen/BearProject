using UnityEngine;
using UnityEngine.AI;

public class OpossumScript : MonoBehaviour
{

    // public NavMeshAgent agent;
    public NavMeshAgent agent;
    public GameObject opossumProjectile;
    public float opossumJumpForce = 2f;
    public float opossumForwardForce = 2f;

    int donotdestroytimer = 100;

       
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        Vector3 playerPosition = player[0].transform.position;
        agent.SetDestination(playerPosition);

        // Debug.Log(agent.velocity.magnitude);


        if (donotdestroytimer > 0)
        {
            donotdestroytimer -= 1;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathPartial && agent.velocity.magnitude < 0.1f && donotdestroytimer == 0)
        {
            OpossumJump();
        }


        

    }

    void OpossumJump()
    {
        
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, 0);
        Vector3 position = transform.position + new Vector3(0f, 1f, 0f);
        GameObject projectile = Instantiate(opossumProjectile, position, rotation);
        Rigidbody opossumRB = projectile.GetComponent<Rigidbody>();

        opossumRB.AddRelativeForce(new Vector3(0f, 1 * opossumJumpForce, 1 * opossumForwardForce));

        Destroy(gameObject);
    }
}
