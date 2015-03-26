using UnityEngine;
using System.Collections;


// object (tornado/whatever particle effect) should have character controller component 
// with this tornado will somewhat take obstacles into account (not by far best and/or final solution so modify all you want... just temporary example how it could be done one way quickly)
public class AS_moveScript2 : MonoBehaviour
{


    public float speedFactor = 444f;        // affects speed..... Bigger number faster speed, smaller number slower speed.
    public float frequency = 2f;            // after this we change direction again      //well it gets randomised somewhat later
    
    private CharacterController controlTornado;             // so we can take account obstacles
    private float previous = 0;


    void Start()
    {
      controlTornado = GetComponent<CharacterController>();
    }

    // to change direction
    void DirectionChange()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0f, 360f), transform.eulerAngles.z);
    }

    
    void Update()
    {
      
        controlTornado.SimpleMove(transform.TransformDirection(Vector3.forward) * Time.deltaTime * speedFactor);

        if (Time.time > previous + frequency)
        {
          //  Debug.Log("changing direction");
            DirectionChange();
            previous = Time.time;
            frequency = Random.Range(-0.5f, 0.5f);      // some randomisation

        }
    }


}
