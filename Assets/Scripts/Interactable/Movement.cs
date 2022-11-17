using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(speed * rotation * Time.deltaTime);
    }
}
