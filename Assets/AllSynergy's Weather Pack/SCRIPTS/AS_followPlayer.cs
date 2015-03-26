using UnityEngine;
using System.Collections;

public class AS_followPlayer : MonoBehaviour {

	
    public GameObject playerGameObject;         // something which you want particle effect to follow
    public Vector3 offset;                      // if you need to offset effects slightly

    public float Frequency = 0.3f;              // update position at this frequency ( with some randomisation )


    void Start()
    {
        Frequency = 0.3f;
        InvokeRepeating("changePosition", Frequency, Frequency*Random.value);

    }


    void changePosition()
    {
        
        transform.position = playerGameObject.transform.position + offset; 
        
    }


    /*
	void Update () {

        transform.position = playerGameObject.transform.position + offset;          // effect follows player (and takes offset as well if any)

	}*/


}
