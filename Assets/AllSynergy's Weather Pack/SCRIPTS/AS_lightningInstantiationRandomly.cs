using UnityEngine;
using System.Collections;


// This will instantiate randomly (between minDelay/maxDelay)... used in lightning-scene2 as an example 
public class AS_lightningInstantiationRandomly : MonoBehaviour {


    public float minDelay = 1;           // min delay before next lightning will be instantiated
    public float maxDelay = 5;           // max delay before next lightning will be instantiated

    public GameObject instantiatedEffectPrefab;                         // here you put instantiated lightning-effect
    public GameObject[] instantiatedEffectsArray;                       // or put several effects (and enable option --> next line from false to true).. in this example we use just a couple of effects
    public GameObject glow;                                             // here you put the "glow-effect" 
    public int effectArraySize = 3;

    public bool useSeveral = false;                 // false = just one instantiated effect (instantiatedEffectPrefab)  , true = use several effects so things look more random


    float nextInstantiationWaitTime = 1;         // default (after 1 second instantiate first)... after this value goes random
    float previous;                                  // when the effect was instantiated last time                               


    void Start()
    {
        previous = Time.time;
      //  instantiatedEffectsArray = new GameObject[effectArraySize];

    }
	


    // instantiates effects randomly 
	void Update () {


        if (Time.time > previous + nextInstantiationWaitTime)
        {
            if (useSeveral == false)
            {
                Instantiate(instantiatedEffectPrefab, transform.position, Quaternion.identity);         // instantiate the effect
                Instantiate(glow, transform.position, Quaternion.identity);                             // instantiate the glow effect

                nextInstantiationWaitTime = Random.Range(minDelay, maxDelay);                           // get next wait time (after that instantiates another effect)
                previous = Time.time;

       
            }
            else if (useSeveral == true)
            {

              /*   for (int i = 0; i <= effectArraySize - 1; i++)       // go through every effect and do something you want....
                {


                }
                */

                // or just take one effect randomly and instantiate that
                int chosenForInitialization = Random.Range(1, effectArraySize);
                Instantiate(instantiatedEffectsArray[chosenForInitialization], transform.position, Quaternion.identity);         // instantiate the effect

                Instantiate(glow, transform.position, Quaternion.identity);                                                      // instantiate the glow effect

                nextInstantiationWaitTime = Random.Range(minDelay, maxDelay);                                                    // get next wait time (after that instantiates another effect)
                previous = Time.time;

            

            }

        }


	}





}
