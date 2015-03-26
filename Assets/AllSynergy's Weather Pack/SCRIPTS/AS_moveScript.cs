using UnityEngine;
using System.Collections;

public class AS_moveScript : MonoBehaviour
{

    public float speedFactor = 14.5f;        // affects speed of tornado/cloud (or any effect). Bigger number faster speed, smaller number slower speed.
    public float frequenzy = 1.5f;          // after this we change direction again      //well it gets randomised somewhat later

    private float previous = 0;          // previous time (when we changed direction)


	// a function which changes direction ( just angle changes randomly and then with update function we go forward every frame)
	void DirectionChange () {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0f, 360f), transform.eulerAngles.z);
	}
	


    void Update()
    {

        transform.Translate(Vector3.forward * Time.deltaTime * speedFactor, Space.Self);
  

        if (Time.time > previous + frequenzy)
        {
            DirectionChange();
            previous = Time.time;
            frequenzy = frequenzy + Random.Range(-1.5f, 1.5f);           // some randomisation to frequenzy

        }
    }


}
