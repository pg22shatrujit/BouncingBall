using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//For Testing: Added to a game object with a rigidbody to compare with the bouncing ball without a rigidbody
public class ActualPhysics : MonoBehaviour
{

    [SerializeField]
    float jumpSpeed = 100.0f, //The increase in the sphere's velocity when the jump key is pressed
          labelPosition = 0.5f; //Distance above the top of the object to display the label

    [SerializeField]
    Color textColor; //Color for the gizmo


    [SerializeField]
    KeyCode jumpControl = KeyCode.Space; //The key used to make the sphere jump, serialized so different sphere's can have different inputs

    private Rigidbody mRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the jump key gets pressed and increment the sphere's velocity if it is
        if(Input.GetKeyDown(jumpControl))
        {
            mRigidBody.velocity = new Vector3(mRigidBody.velocity.x,
                                              mRigidBody.velocity.y + jumpSpeed,
                                              mRigidBody.velocity.z);
        }
    }

    //Draws a gizmo over the top of the object letting the user know the jump key and that this object uses Unity Physics
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = textColor;


        UnityEditor.Handles.Label(offsetLabel(gameObject),
                                  jumpControl + " - Physics",
                                  style);
    }

    //Finds the top of the object and adds an offset, used to determine where to display the gizmo
    private Vector3 offsetLabel(GameObject GO)
    {
        float yOffset = GO.transform.position.y + GO.transform.localScale.y/2 + labelPosition;

        return new Vector3(GO.transform.position.x, yOffset, GO.transform.position.z);

    }

}
