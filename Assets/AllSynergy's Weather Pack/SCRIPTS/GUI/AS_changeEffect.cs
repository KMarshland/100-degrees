using UnityEngine;
using System.Collections;

public class AS_changeEffect : MonoBehaviour {

    // just a list of effects 
    public GameObject Effect1;  
    public GameObject Effect2;
    public GameObject Effect3;

  

    void OnGUI()
    {


        if (GUI.Button(new Rect(10, 10, 300, 20), "Effect 1"))
        {

       
            Effect1.gameObject.SetActive(true);
            Effect2.gameObject.SetActive(false);
            Effect3.gameObject.SetActive(false);
       

        }


        if (GUI.Button(new Rect(320, 10, 300, 20), "Effect 2"))
        {

            Effect1.gameObject.SetActive(false);
            Effect2.gameObject.SetActive(true);
            Effect3.gameObject.SetActive(false);
      

        }


        if (GUI.Button(new Rect(10, 50, 300, 20), "Effect 3"))
        {
            Effect1.gameObject.SetActive(false);
            Effect2.gameObject.SetActive(false);
            Effect3.gameObject.SetActive(true);
    

        }


    }




}
