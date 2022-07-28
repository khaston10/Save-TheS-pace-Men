using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform freeLookTarget;
    public Rigidbody2D rgb2;
    private float zoomSpeed = .03f;
    private float scrollSpeed = 10f;
    private bool zoomInNeeded = false;
    private bool zoomOutNeeded = true;
    private float zoomMax = 5;
    private float zoomMin = 3f;
    private float moveHorizontal;
    private float moveVertical;
    public GameObject[] walls; //Up, Right, Bottom, Left
    private float leftEdge;
    private float rightEdge;
    private float topEdge;
    private float bottomEdge;

    public bool gameIsStarted = false;

    private void Start()
    {
        GetComponent<Camera>().orthographicSize = 2;
        freeLookTarget.transform.position = target.transform.position;

        //Set level end points.
        leftEdge = walls[3].transform.position.x;
        rightEdge = walls[1].transform.position.x;
        topEdge = walls[0].transform.position.y;
        bottomEdge = walls[2].transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsStarted)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -2f);
        }

        else
        {
            MoveFreeLookTarget();

            transform.position = new Vector3(freeLookTarget.position.x, freeLookTarget.position.y, -2f);
        }

    }

    void FixedUpdate()
    {
        ZoomCamera();
    }

    void ZoomCamera()
    {
        // Check to see what zoom out is needed.
        if (Mathf.Abs(rgb2.velocity.y) > 0f)
        {
            zoomInNeeded = false;
            zoomOutNeeded = true;
        }

        else
        {
            zoomOutNeeded = false;
            zoomInNeeded = true;
        }


        // Move the camera.
        if (zoomInNeeded)
        {
            if (GetComponent<Camera>().orthographicSize > zoomMin)
            {
                GetComponent<Camera>().orthographicSize -= zoomSpeed;
            }

            else zoomInNeeded = false;

        }

        else if (zoomOutNeeded)
        {
            if (GetComponent<Camera>().orthographicSize < zoomMax)
            {
                GetComponent<Camera>().orthographicSize += zoomSpeed;
            }

            else zoomOutNeeded = false;

        }

    }

    void MoveFreeLookTarget()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (freeLookTarget.transform.position.x > rightEdge && moveHorizontal > 0) moveHorizontal = 0;
        else if (freeLookTarget.transform.position.x < leftEdge && moveHorizontal < 0) moveHorizontal = 0;

        if (freeLookTarget.transform.position.y > topEdge && moveVertical > 0) moveVertical = 0;
        else if (freeLookTarget.transform.position.y < bottomEdge && moveVertical < 0) moveVertical = 0;

        freeLookTarget.transform.position = new Vector2(freeLookTarget.transform.position.x + moveHorizontal * scrollSpeed * Time.deltaTime, freeLookTarget.transform.position.y + moveVertical * scrollSpeed * Time.deltaTime);
      
    }

}