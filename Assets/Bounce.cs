using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.XR;

//Bounce Script
public class Bounce : MonoBehaviour
{
    //var for parent cube on one of the alreadt created spheres in the scene
    [SerializeField] private MoveObject Cube;

    //start & end values for bounce
    public float start_Y, end_Y;

    //Floats for storing the orignal start postion of object, lerp value, and the base if there is a parent
    //object on the selected object
    public float startPosY, lerpFloat_y, basePoint;

    //Lerp boolean for preventing user from starting bounce coroutine while a bounce is in progress
    bool lerping;

    //Floats for storing speed and height of bounce
    public static float bounceSpeed, bounceHeight; 

    //Boolean used to change end value in case object bounces into a ceiling
    public static bool detectTopCollision = false;

    //Sets the start position and sets the lerp float to the current start position so object doesn't move
    //script starts
    private void Awake() {
        startPosY = transform.position.y;
        lerpFloat_y = transform.position.y;
    }

    //Function that starts the bounce called from the "Start Bounce" button
    public void StartBounce() {
        //Only starts if bounce isn't already in progress
        if (!lerping) {
            StartCoroutine(BounceLerp());
        }
    }


    //Function to engage bounce
    private IEnumerator BounceLerp() {
        //Set lerping to true to prevent user from starting another bounce while this on is in progress
        lerping = true;

        //Set time to zero
        float time = 0f;

        //If parent object is present set start value of current object to right above parent
        if (transform.parent) start_Y = transform.parent.position.y + 1.1f;
        //No parent, so set start value to start position of object
        else start_Y = startPosY;

            //Start creating lerpFloat values for one sec or continuously if Bounce contiuous toggle is set in UIManager
            while (time < 1f || UIManager.bounceContinuous) {

            //If there is no object collision set end value to bounce height selected by bounce height slider
            if (!detectTopCollision) end_Y = start_Y + UIManager.bounceHgtAmount;

            //Generate lerpFloat values by accessing Lerp function library and feeding in Linear ease &
            //setting pingpong to true
            lerpFloat_y = LerpFunctions.Lerping(start_Y, (end_Y*2), LerpFunctions.LerpPerc(1, time, true));

            //Increment Time and multiply by value in bounce speed slider to increase speed of bounce
            time += Time.deltaTime*bounceSpeed;

            //IEnumerator must return but we don't want to return anything so we return null
            yield return null;
        }

        //Set lerp float to beginning start postion so object ends where it began
        lerpFloat_y = startPosY;
        //Set lerping to false to allow user to start another bouncew since this one has ended
        lerping = false;
    }

    //Collision detection function in case object bounces into something vertically
    void OnCollisionEnter(Collision collision){
        //if the object hits the bottom of another object change end value of bounce to just below hit object
        //so that it looks like object is bouncing off
        if (collision.transform.position.y > transform.position.y) {
            end_Y = collision.transform.position.y - 1.1f;
            //Set to true so that BounceLerp function won't override end value with bounce height slider value
            detectTopCollision = true;
        }
        //object must have hit the top of another object therefore start value is reset to just above the hit
        //object
        else start_Y = collision.transform.position.y + 1.1f;
    }


    // Update is called once per frame
    void Update(){

        //If parent object is present and not set start value of current object to right above parent
        if (transform.parent && !lerping) lerpFloat_y = transform.parent.position.y + 1.1f;

        //Translate lerpFloat values into object movement
        transform.position = new Vector3(transform.position.x, lerpFloat_y, transform.position.z);
    }
}
