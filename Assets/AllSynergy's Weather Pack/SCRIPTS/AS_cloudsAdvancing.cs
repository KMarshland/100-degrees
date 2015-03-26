using UnityEngine;
using System.Collections;

// You can remove this script from cloud object and just use cloud as it is... this is only for cloud movement which you can handle in many different ways



// just sample script to move clouds and after "moveObjectAfterThis" has been reached move cloud back to it's starting position (with small randomization)
public class AS_cloudsAdvancing : MonoBehaviour {


    //  moveDirection COULD OF COURSE BE WHATEVER YOU CHOOSE                   
    public Vector3 moveDirection = Vector3.right;            // moving towards x             
  //  public Vector3 moveDirection = -Vector3.right;         // moving towards -x
  //  public Vector3 moveDirection = Vector3.forward;        // moving towards z
  //  public Vector3 moveDirection = -Vector3.forward;       // moving towards -z

    public float moveSpeed = 5.0f;                          // set default move speed for clouds


    // 4 direction where to move as an example
    public enum direction
    {
        x,              // if moving towards x ( Vector3.right ) choose this
        xMinus,         // if moving towards -x ( -Vector3.right ) choose this
        z,              // if moving towards z ( Vector3.forward ) choose this
        zMinus,          // if moving towards -z ( -Vector3.forward ) choose this
        none            // do not use  ( clouds move on and on )
        
    }
    
    public direction directionOfMovement = direction.x;         // choosing one of them (direction.x means that clouds move towards x-direction) 
    public float moveObjectAfterThis = 200;                    // after this (x-position value) jump to start location (with slightly altered position using Random.Range)

    private Vector3 startPosition;                              // this stores clouds starting position



    void Start(){


        startPosition = transform.position;     // this could alse be randomized a little bit


    }



	// Update is called once per frame
	void Update () {




        // Move the object in world-space 
        transform.Translate(moveDirection * Time.deltaTime * moveSpeed, Space.World);



        // moving towards x and after it goes far enough just put it back to starting location
        if ( directionOfMovement == direction.x )
        {

            if (transform.position.x > moveObjectAfterThis)
                //transform.position = startPosition;             // jump back to starting position (might be nice to randomize it a little bit)
                transform.position = startPosition + new Vector3(Random.Range(-40, 40), Random.Range(-5, 5), Random.Range(-40, 40)); 
        }






            // moving towards minux x
        else if (directionOfMovement == direction.xMinus)
        {


            if (transform.position.x < moveObjectAfterThis)
                //transform.position = startPosition;             // jump back to starting position (might be nice to randomize it a little bit)
                transform.position = startPosition + new Vector3(Random.Range(-40, 40), Random.Range(-5, 5), Random.Range(-40, 40));
        }


               // moving towards z direction
        else if (directionOfMovement == direction.z)
        {


            if (transform.position.z > moveObjectAfterThis)
                //transform.position = startPosition;             // jump back to starting position (might be nice to randomize it a little bit)
                transform.position = startPosition + new Vector3(Random.Range(-40, 40), Random.Range(-5, 5), Random.Range(-40, 40));
              
        }

               // moving towards minus z
        else if (directionOfMovement == direction.zMinus)
        {


            if (transform.position.z < moveObjectAfterThis)
                //transform.position = startPosition;             // jump back to starting position (might be nice to randomize it a little bit)
                transform.position = startPosition + new Vector3(Random.Range(-40, 40), Random.Range(-5, 5), Random.Range(-40, 40));
        }


        else if (directionOfMovement == direction.none)
        {
            // nothing really.... cloud keeps moving



            // or you could destroy it after awile....
            //  if (Time.time > YourTime...whateverlimit you need)     
           // Destroy(gameObject);        
        }

	}
}
