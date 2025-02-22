using System;
using UnityEngine;

public class InfoLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Awake()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(target);
        transform.Rotate(0, 180f, 0);
    }
}
