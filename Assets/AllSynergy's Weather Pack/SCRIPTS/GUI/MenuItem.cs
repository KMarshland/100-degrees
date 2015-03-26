using UnityEngine;
using System.Collections;


// This is used for mainmenu items
public class MenuItem : MonoBehaviour
{

    void Start() { Cursor.visible = true;  }



    // change color
    void OnMouseEnter()
    {

        GetComponent<Renderer>().material.color = Color.red;



    }

    // change color
    void OnMouseExit()
    {

        GetComponent<Renderer>().material.color = Color.white;



    }


    // menu working ( jump to different scenes when clicking mouse )
    void OnMouseUp()
    {

       /* if (gameObject.tag == "MenuTextQuit")
        {
            Debug.Log("Quitting");
            Application.Quit();
        }*/
        if (gameObject.tag == "1menu_CloudScene1")
        {
    
            Application.LoadLevel(1);
        }
        else if (gameObject.tag == "2menu_CloudScene2RainClouds")
        {
          
            Application.LoadLevel(2);
        }
        else if (gameObject.tag == "3menu_HailStorm")
        {
       
            Application.LoadLevel(3);
        }
        else if (gameObject.tag == "4menu_Lightning")
        {
        
            Application.LoadLevel(4);
        }

        else if (gameObject.tag == "5menu_Lightning2")
        {

            Application.LoadLevel(5);
        }


        else if (gameObject.tag == "6menu_NorthernLights1")
        {
         
            Application.LoadLevel(6);
        }

        else if (gameObject.tag == "7menu_NorthernLights2")
        {

            Application.LoadLevel(7);
        }

        else if (gameObject.tag == "8menu_NorthernLights3")
        {

            Application.LoadLevel(8);
        }

        else if (gameObject.tag == "9menu_Rain1")
        {

            Application.LoadLevel(9);
        }

        else if (gameObject.tag == "10menu_Rain2")
        {

            Application.LoadLevel(10);
        }
        else if (gameObject.tag == "11menu_Rain3")
        {

            Application.LoadLevel(11);
        }
        else if (gameObject.tag == "12menu_SandStorm1")
        {

            Application.LoadLevel(12);
        }
        else if (gameObject.tag == "13menu_SandStorm2")
        {

            Application.LoadLevel(13);
        }

        else if (gameObject.tag == "14menu_Snow")
        {

            Application.LoadLevel(14);
        }

        else if (gameObject.tag == "15menu_SnowStorm")
        {

            Application.LoadLevel(15);
        }

            /////////////////////////////////////
        else if (gameObject.tag == "16menu_info")
        {
            Debug.Log("Loading Info scene");
            Application.LoadLevel(16);
        }


    }


}
