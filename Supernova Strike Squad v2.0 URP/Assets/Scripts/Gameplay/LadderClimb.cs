using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public Transform chController;
    public bool inside = false;
    public float speedUpDown = 2.2f;
    public PlayerController playerControl;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<PlayerController>();
        inside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ladder")
        {
            inside = !inside;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            inside = !inside;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(inside == true && Input.GetKey("w"))
        {
            chController.transform.position += Vector3.up / speedUpDown;
        }
        if (inside == true && Input.GetKey("s"))
        {
            chController.transform.position += Vector3.down / speedUpDown;
        }
    }
}
