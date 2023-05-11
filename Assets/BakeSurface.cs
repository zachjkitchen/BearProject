
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class BakeSurface : MonoBehaviour
{

    private NavMeshSurface _surface;


    // Start is called before the first frame update
    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        _surface = GetComponent<NavMeshSurface>();

        Debug.Log("HIT DEBUG");
        Debug.Log(_surface.navMeshData);

        Invoke("Bake", 1);

        
    }

    void Bake()
    {
        Debug.Log("HIT DEBUG");
        Debug.Log(_surface.navMeshData);
        _surface.BuildNavMesh();
    }

    
}
