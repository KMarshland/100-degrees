using UnityEngine;
using System.Collections;

public class AS_followPlayerForRainScene : MonoBehaviour
{

	
    public GameObject playerGameObject;         // something which you want particle effect to follow
    public Vector3 offset;                      // if you need to offset effects slightly
    public bool followingOn;             
    public float Frequency = 0.1f;              // update position at this frequency ( with some randomisation )
    public GUIStyle textStyle;                  // just general style for text
    public bool GUIon = true;                   // hide buttons if not needed by changing this from true to false (and if it's not needed better REMOVE the ENTIRE OnGUI-function)
  

    void Start()
    {
       // Frequency = 0.1f;
        InvokeRepeating("changePosition", Frequency, Frequency);

    }


    void changePosition()
    {
        if (followingOn)
        transform.position = playerGameObject.transform.position + offset; 
        
        

    }


  




    // DISABLE/REMOVE THIS FUNCTION WHEN NOT NEEDED
    void OnGUI()
    {

        if (GUIon == true)
        {

     

            if (GUI.Button(new Rect(10, 240, 120, 20), "Follow Player"))
                followingOn = true;

            if (GUI.Button(new Rect(130, 240, 120, 20), "Do NOT follow"))
                followingOn = false;



            if (followingOn == true)
                 GUI.Label(new Rect(10, 290, 200, 30), "Follow player = true", textStyle);

            else
                GUI.Label(new Rect(10, 290, 200, 30), "Follow player = false", textStyle);

           
        }
    }
}
