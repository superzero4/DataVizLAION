using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSphere : MonoBehaviour
{

    public float baseRotationSpeed = 0.05f;
    public float rotationSpeed = 0.5f;
    private bool goesLeft, goesRight;
    
    // Start is called before the first frame update
    void Start()
    {
        goesLeft = false;
        goesRight = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) goesLeft = true;
        if (Input.GetKeyDown(KeyCode.RightArrow)) goesRight = true;
        
        if (Input.GetKeyUp(KeyCode.LeftArrow)) goesLeft = false;
        if (Input.GetKeyUp(KeyCode.RightArrow)) goesRight = false;
        
        transform.Rotate(0,baseRotationSpeed + (goesLeft?-rotationSpeed:0) + (goesRight?rotationSpeed:0),0);

    }
}
