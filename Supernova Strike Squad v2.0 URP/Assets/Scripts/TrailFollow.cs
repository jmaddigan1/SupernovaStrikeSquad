using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollow : MonoBehaviour
{
    [SerializeField] private Transform target = null;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(target.position, transform.position, Time.deltaTime * 5);
    }
}
