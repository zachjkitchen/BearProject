
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _surface = GetComponent<NavMeshSurface>();
        
        Invoke("Bake", 1);

        
    }

    void Bake()
    {
        //_surface.BuildNavMesh();
    }

    
}
