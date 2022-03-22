using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPad : MonoBehaviour
{
    public float timer;
    public Vector3 tempY;
    public Vector3 moveHeight;
    public Vector3 topHeight;
    bool atTop;
    bool atBottom;
    // Start is called before the first frame update
    void Start()
    {
        timer = 3;
        moveHeight = new Vector3(0,10, 0);
        tempY = this.transform.position;
        topHeight = tempY + moveHeight;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (this.transform.position.y >= tempY.y && atTop == false)
        { 
            if (timer <= 0)
            {
                this.transform.Translate(0,10f* 0.5f * Time.deltaTime,0, Space.World);

                if (this.transform.position.y > topHeight.y)
                {
                    this.transform.position = topHeight;
                    atTop = true;
                    atBottom = false;
                    timer = 3;
                }
            }
        }
        else if (this.transform.position.y <= topHeight.y && atBottom == false)
        {
            if (timer <= 0)
            {
                this.transform.Translate(0, -10f* 0.5f * Time.deltaTime, 0, Space.World);

                if (this.transform.position.y < tempY.y)
                {
                    this.transform.position = tempY;
                    atTop = false;
                    atBottom = true;
                    timer = 3;
                }
            }

        }
    }
}
