using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add velocity and simulate bouncing for a gameObject without a rigidbody
public class BouncingBallPhysics : MonoBehaviour
{

    [SerializeField]
    float jumpSpeed = 100.0f, //The increase in the sphere's velocity when the jump key is pressed
          gravitationalAcceleration = -9.8f, //The change in the sphere's velocity due to gravity
          labelPosition = 0.5f; //Distance above the top of the object to display the label

    [SerializeField]
    Color textColor; //Color for the gizmo

    [SerializeField, Range(0.0f, 1.0f)]
    float bounciness = 0.5f; //The bounciness of the sphere (ratio of final/inital speed when colliding with the ground)

    [SerializeField]
    KeyCode jumpControl = KeyCode.Space; //The key used to make the sphere jump, serialized so different sphere's can have different inputs

    float velocity, //The sphere's current velocity
          radius; //Radius of the sphere, used to calculate collision with the ground
        

    // Start is called before the first frame update
    void Start()
    {
        velocity = 0; //Initialize velocity to 0

        radius = gameObject.transform.localScale.y / 2; //Inititalize radius to half the object's scale along the Y-axis
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the jump key gets pressed and increment the sphere's velocity if it is
        if(Input.GetKeyDown(jumpControl))
        {
            velocity += jumpSpeed;
        }

        //Draw gizmos on balls
    }

    //Physics calculations get called in FixedUpdate() to update current velocity and position
    private void FixedUpdate()
    {
        UpdateVelocity();
        UpdatePosition();
    }

    //Updates the object's current velocity
    private void UpdateVelocity()
    {
        //If the object's current Y-position if not very close to the radius of the sphere (i.e. It isn't on the ground), increase velocity by acceleration * time
        if (!Mathf.Approximately(gameObject.transform.position.y, radius))
            velocity += gravitationalAcceleration * Time.fixedDeltaTime;
        //Otherwise if it's close to radius distance from the floor and velocity is pretty small, zero out velocity (To stop it from jiggling when it's on the floor
        else if (Mathf.Abs(velocity) < 0.5f)
            velocity = 0;
    }

    //Calls setYPosition() to update the sphere's position
    private void UpdatePosition()
    {
        //Sets the sphere's new Y position
        setYPosition(gameObject.transform.position.y + velocity * Time.fixedDeltaTime);

        //Checks for collision with the floor
        CheckForFloorAndBounce();
    }

    //Checks for collision with the floor and calculates velocity after bounce
    private void CheckForFloorAndBounce()
    {
        //Checks if the Y-position is not greater than the radius of the sphere
        if (gameObject.transform.position.y <= radius)
        {
            //If true, makes the new velocity the inverse of the old vevlocity times the bounciness ratio
            velocity *= -1 * bounciness;
        }
    }

    //Takes a float param and sets the new Y-position of the gameObject
    private void setYPosition(float newYValue)
    {
        //If the new position is greater than the radius (The object is not going through the floor), update the object's transform with the new position
        if(newYValue > radius)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, newYValue, gameObject.transform.position.z);
        //Otherwise, update the object's Y-position to it's radius (So it's just resting on the ground)
        else
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, radius, gameObject.transform.position.z);
    }

    //Draws a gizmo over the top of the object letting the user know the jump key and that this object does not use Unity Physics
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = textColor;

        
        UnityEditor.Handles.Label(offsetLabel(gameObject),
                                  jumpControl + " - No Physics", 
                                  style);
    }

    //Finds the top of the object and adds an offset, used to determine where to display the gizmo
    private Vector3 offsetLabel(GameObject GO)
    {
        float yOffset = GO.transform.position.y + GO.transform.localScale.y/2 + labelPosition;

        return new Vector3(GO.transform.position.x, yOffset, GO.transform.position.z);

    }
}
