using UnityEngine;
using UnityEngine.AI;

public class OpossumScript : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
        agent.SetDestination(player.transform.position);

    }
}
