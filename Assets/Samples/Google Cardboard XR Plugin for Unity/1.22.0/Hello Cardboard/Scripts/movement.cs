using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public Transform cam;
    protected Vector3 left;
    protected Vector3 forward;
    public float scalemove;
    // Start is called before the first frame update
    void Start()
    {
        left = new Vector3(-1, 0, 0);
        forward = new Vector3(0, 0, 1);
        left = left * scalemove;
        forward = forward * scalemove;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            cam.Translate(left * Time.deltaTime);
            //Code for action on mouse moving left
            // print("Mouse moved left");
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            cam.Translate(-left * Time.deltaTime);
            //Code for action on mouse moving right
            // print("Mouse moved right");
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            cam.Translate(-forward * Time.deltaTime);
            //Code for action on mouse moving left
            // print("Mouse moved down");
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            cam.Translate(forward * Time.deltaTime);
            //Code for action on mouse moving right
            // print("Mouse moved up");
        }
    }
}