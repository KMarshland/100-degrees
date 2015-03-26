using UnityEngine;
using System.Collections;


// just for moving gameobject ( used to move raincloud in on of the demoscenes ) 
public class AS_cloudMoveSimple : MonoBehaviour {


    public Vector3 moveDirection = new Vector3(1, 0, 0);        // default direction (move when/if needed)
    public float moveSpeed = 2.0f;                          // set default move speed 


	
	void Update () {

        // Move the object in world-space 
        transform.Translate(moveDirection * Time.deltaTime * moveSpeed, Space.World);

	}
}
