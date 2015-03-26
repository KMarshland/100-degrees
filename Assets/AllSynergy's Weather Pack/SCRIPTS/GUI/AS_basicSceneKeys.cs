using UnityEngine;
using System.Collections;


// Right now only needed to go back to mainmenu
public class AS_basicSceneKeys : MonoBehaviour {

	
	
	
	void Update () {


        if (Input.GetKeyDown("left alt"))
        {
            Application.LoadLevel(0);   // back to mainmenu
        }
  

	}
}
