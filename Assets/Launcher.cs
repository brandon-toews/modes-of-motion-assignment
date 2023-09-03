using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

//Launch Script
public class Launcher : MonoBehaviour
{

    //Floats to store launch velocities/angles & gravity
    public static float launchVelocity = 10f, launchAngle = 30f;
    private const float Gravity = -9.8f;

    //Vector3s to store velocity and acceleration on X & Y axis', and start postion of object
    //when script starts
    public Vector3 v3InitialVelocity;
    public Vector3 v3CurrentVelocity;
    private Vector3 v3Acceleration;
    private Vector3 startPos;
    private Quaternion startRot;

    //Floats for storing airtime and displacement
    private float airTime = 0f, xDisplacement = 0f;

    //Booleans to start launch or to just calculate and draw path when not launching
    public bool simulate = false, launch = false, detectCollision = false;

    //Array that populates with calculated path so path can be drawn
    private List<Vector3> pathPoints;
    private int simulationSteps = 30;

    // Start is called before the first frame update
    void Start() {
        //Sets start position of object and creates pathpoints array for CalculatePath function
        startPos = transform.position;
        startRot = transform.rotation;
        pathPoints = new List<Vector3>();
        //calls Function to calculate projectile movement before launch
        CalculateProjectile();
        CalculatePath();
    }

    //Function calculates projectile movement before launch
    private void CalculateProjectile() {
        //Velocity as vector quantity
        v3InitialVelocity.x = launchVelocity * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        v3InitialVelocity.y = launchVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        //gravity as vector
        v3Acceleration = new Vector3(0f, Gravity, 0f);

        //calculate total time in air
        float finalYVel = 0f;
        airTime = 2f * (finalYVel - v3InitialVelocity.y) / v3Acceleration.y;

        //calculate total distance travelled in x
        xDisplacement = airTime * v3InitialVelocity.x;
    }

    //Function creates draw points so DrawPath function can draw projectile path
    private void CalculatePath() {

        //Adds object start position to the first point of pathpoints array
        Vector3 launchPos = transform.position;
        pathPoints.Add(launchPos);

        //For loop to populate path points all along the calculated projectile path
        for (int i = 0; i <= simulationSteps; i++) {
            //moves path points along in calculated path based on time progression through the movement
            float simTime = (i / (float)simulationSteps) * airTime;

            //suvat formula for displacement: s = ut + 1/2at*2
            Vector3 displacement = v3InitialVelocity * simTime + v3Acceleration * simTime * simTime * 0.5f;

            //create draw point and add it to the pathpoints array
            Vector3 drawPoint = launchPos + displacement;
            pathPoints.Add(drawPoint);
        }
    }

    //Function called to draw calculated projectile path 
    void DrawPath() {
        //For loop to go through each pathPoint in array to draw the line
        for (int i = 0; i < pathPoints.Count - 1; i++) {
            //Draw line from current point in array to the next
            Debug.DrawLine(pathPoints[i], pathPoints[i + 1], Color.green);
        }
    }

    //Function to detect collision with terrain & alter velocity to simulate bounce
    void OnCollisionEnter(Collision collision) {
        //When object collides with terrain turn of launch so that object stops dropping & calculates and draws
        //new path from the terrain
        detectCollision = true;
        launch = false;
        simulate = false;

        //If object hits terrain ,
        //Calculation to alter velocity to simulate coefficient of restitution from object hitting terrain 
        if (collision.transform.position.y < transform.position.y)
        {
            launchVelocity = 0.8f * -v3CurrentVelocity.y;
        }
    
        //Start launch from terrain with new slower velocity only if velcity isn't to low
        //if velocity drops below o.4f while still simulating object goes crazy and can't recover
        if (launchVelocity > 0.4f) launch = true;
    }

    // Function called from reset button & when object falls to far off the screen,
    // turns off launch and resets object back to orignal position
    public void ResetLaunch() {
        //Stops launch
        detectCollision = false;
        launch = false;
        simulate = false;
        //resets object back to original position
        transform.position = startPos;
        transform.rotation = startRot;

        v3InitialVelocity = Vector3.zero;
        v3CurrentVelocity = Vector3.zero;

        //resets velocity back to what velocity slider is set to
        launchVelocity = UIManager.velAmount;
    }

    //Fixed update to better simulate physics movement
    private void FixedUpdate() {
        //if launch is engaged send object along calculate path
        if (simulate) {
            //Start movement from current postion
            Vector3 currentPos = transform.position;
            //work out current velocity
            v3CurrentVelocity += v3Acceleration * Time.fixedDeltaTime;

            //work out displacement
            Vector3 displacement = v3CurrentVelocity * Time.fixedDeltaTime;

            //Increment position by the displacement equation result
            currentPos += displacement;

            //Translate into object movement
            transform.position = currentPos;

            //If object falls of plane eventually it will reset once it falls far enough
            if (transform.position.y < -30) ResetLaunch();
        }
        //If bounce is either not happening or has reach the end then keep object at start postion
        else if(!detectCollision) {
           transform.position = startPos;
           transform.rotation = startRot;
        }
    }

    // Update is called once per frame
    void Update() {
        //When object is stopped calculate projectile path and draw it
        if (simulate == false) {
            //Create pathpoints array to populate with calculate path function
            pathPoints = new List<Vector3>();
            CalculateProjectile();
            CalculatePath();
        }
        //Call function to draw calculated path
        DrawPath();

        //Intitiate launch of object 
        if (launch && !simulate) {
            simulate = true;
            v3CurrentVelocity = v3InitialVelocity;
        }
    }
}
