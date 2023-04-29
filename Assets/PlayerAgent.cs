using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAgent : MonoBehaviour
{
    public Transform targetMarker;
    private NavMeshAgent player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Cast a ray where the agent will move towards to
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo))
            {
                //Update the target position based on where the raycast hits an object
                UpdateTarget(hitInfo.point);
            }
        }
    }

    private void UpdateTarget(Vector3 position)
    {
        //Move an agent
        player.SetDestination(position);
        targetMarker.position = position;
    }

}
