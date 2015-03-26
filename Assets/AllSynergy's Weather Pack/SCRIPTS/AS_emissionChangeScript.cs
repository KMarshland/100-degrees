using UnityEngine;
using System.Collections;

public class AS_emissionChangeScript : MonoBehaviour
{

    public GUIStyle textStyle;      // just general style for text

    public int minEmission = 44;       // normal number
    public int mediumEmission = 444;   // slightly higher
    public int highEmission = 1111;     // very high

    private enum effectType
    {
        light,
        medium,
        heavy
    }

    private effectType type;

    private new ParticleSystem particleSystem;


    void Start()
    {

        particleSystem = GetComponent<ParticleSystem>();

    }


      void OnGUI(){

     if (GUI.Button(new Rect(10, 270, 120, 20), "Light rain"))
            {
            
                type = effectType.light;
                particleSystem.emissionRate = minEmission;
      

            }


            if (GUI.Button(new Rect(130, 270, 120, 20), "Medium rain"))
            {
             
                type = effectType.medium;

                particleSystem.emissionRate = mediumEmission;

            }


            if (GUI.Button(new Rect(250, 270, 120, 20), "Heavy rain"))
            {
          
                type = effectType.heavy;

                particleSystem.emissionRate = highEmission;
        
            }


          // check which type (how hard it rains) and change gui-text based on that
            if (type == effectType.light)
                GUI.Label(new Rect(Screen.width - 300, 90, Screen.width - 10, 30), "Rain type = light", textStyle);
            else if (type == effectType.medium)
                GUI.Label(new Rect(Screen.width - 300, 90, Screen.width - 10, 30), "Rain type = medium", textStyle);
            else
                GUI.Label(new Rect(Screen.width - 300, 90, Screen.width - 10, 30), "Rain type = heavy", textStyle);


        }







}
