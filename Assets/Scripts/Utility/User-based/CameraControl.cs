using UnityEngine;
using System.Collections;
using KofTools;

public class CameraControl : MonoBehaviour {
	
	public static float x, y, z;
	public float rotationX, rotationY, rotationZ;
	
	bool madeTool = false;
	
	private const int LevelArea = 10000;
	private const int ScrollArea = 25;
	private const int ScrollSpeed = 25;
	private const int DragSpeed = 100;
	private const int ZoomSpeed = 25;
	private const int ZoomMin = -20;
	private const int ZoomMax = 1000;
	private const int PanSpeed = 50;
	private const int PanAngleMin = 30;
	private const int PanAngleMax = 80;
		
	private static Vector3 right = Vector3.right;
	private static Vector3 forward = Vector3.forward;
	private static Vector3 curRot;
	
	private static Quaternion rot;
	private static bool hasRotChanged = true;
	
	// Update is called once per frame
	void Update() {
		
		JobController.Update();
		x = transform.position.x;
		y = transform.position.y;
		z = transform.position.z;
		
		if (hasRotChanged){
			rot = Quaternion.Euler(0,GetComponent<Camera>().transform.eulerAngles.y, 0);
			hasRotChanged = false;
		}
		curRot = GetComponent<Camera>().transform.eulerAngles;
		float negScroll = -ScrollSpeed * Time.deltaTime;
		float posScroll = ScrollSpeed * Time.deltaTime;
		// Init camera translation for this frame.
		var translation = Vector3.zero;
		var rotation = Vector3.zero;
		// Zoom in or out
		var zoomDelta = Input.GetAxis("Mouse ScrollWheel")*ZoomSpeed*Time.deltaTime;
		if (zoomDelta!=0){
			translation -= Vector3.up * ZoomSpeed * zoomDelta;
		}
		// Start panning camera if zooming in close to the ground or if just zooming out.
//           var pan = camera.transform.eulerAngles.x - zoomDelta * PanSpeed;
//
//           pan = Mathf.Clamp(pan, PanAngleMin, PanAngleMax);

//if (zoomDelta < 0 || camera.transform.position.y < (ZoomMax / 2))
//
 //          {
//
 //              camera.transform.eulerAngles = new Vector3(pan, 0, 0);
//
//          }

 

            // Move camera with arrow keys

		translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		rotation += new Vector3(Input.GetAxis ("RotateCamera"),Input.GetAxis ("RotateCamera2"),0);
		 
			// Rotation Code
		
		
	//	    if (Input.GetKey("e"))
	//	{
	  //      transform.Rotate(1,0,0);
	//	}
		
	//	if (Input.GetKey("z"))
		{
	  //      transform.Rotate(-1,0,0);
		//}
            // Move camera with mouse
			
			if (Input.GetMouseButton(2)) // MMB
            {
                // Hold button and drag camera around
                translation -= new Vector3(Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime, 0,
                                   Input.GetAxis("Mouse Y") * DragSpeed * Time.deltaTime);
            }
            else
            {
				if (!MenuControl.scrollingDisabled){
					if (Input.mousePosition.x < ScrollArea)
	                {
	                    //translation += right * -ScrollSpeed * Time.deltaTime;
						translation += rotY(curRot.y, right, negScroll);
						//translation.z -= ordinal.z;
	                }
					
	                if (Input.mousePosition.x >= Screen.width - ScrollArea)
	                {
	                    //translation += right * ScrollSpeed * Time.deltaTime;
						translation += rotY(curRot.y, right, posScroll);
						//translation.z += ordinal.z;
	                }
					
	                if (Input.mousePosition.y < ScrollArea)
	                {
	                    //translation += forward * -ScrollSpeed * Time.deltaTime;
						translation += rotY (curRot.y, forward, negScroll);
						//translation.x -= ordinal.x;
	                }
	                if (Input.mousePosition.y > Screen.height - ScrollArea)
	                {
	                    //translation += forward * ScrollSpeed * Time.deltaTime;
						translation += rotY (curRot.y, forward, posScroll);
						//translation.x += ordinal.x;
	                }
					// change vector if it has changed this update
					/*
					if (rotVal != camera.transform.eulerAngles){
						rotVal = camera.transform.eulerAngles;
						rot = rotate (rot, rotVal.y, new Vector3(0,1,0));
					}//*/
					
					//translation = x_Vectors(translation, ((new Vector3(rot.x, 0, rot.z)).normalized)+(new Vector3(1,1,1)));
					//translation = x_Vectors(translation, (new Vector3(rot.x, 0, rot.z)).normalized);
	            }
			}
            
			// Keep camera within level and zoom area
            var desiredPosition = GetComponent<Camera>().transform.position + translation;

            if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x)
            {
                translation.x = 0;
            }

            if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
            {
                translation.y = 0;
            }

            if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z)
            {
                translation.z = 0;
            }
            // Finally move camera parallel to world axis
			
			translation = rot * translation;
			
			if (rotation != Vector3.zero){
				hasRotChanged = true;
			}
			
			if (rotation.x + GetComponent<Camera>().transform.eulerAngles.x > 80 && rotation.x + GetComponent<Camera>().transform.eulerAngles.x < 90){
				rotation.x = 0;
			}else if( rotation.x + GetComponent<Camera>().transform.eulerAngles.x < 280 && rotation.x + GetComponent<Camera>().transform.eulerAngles.x > 270){
				rotation.x = 280;
			}

            GetComponent<Camera>().transform.position += translation;		
			GetComponent<Camera>().transform.eulerAngles += rotation;
        }
    }

	public static Vector3 x_Vectors(Vector3 a, Vector3 b){
		return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
	}
	
	// small problem for when yRot = 0, and probably for when yRot = 180 also
	public static Vector3 rotY(float yRot, Vector3 input, float scale){
		Vector3 tVal = new Vector3(1, 0, 1) * scale;
		Vector3 iScale = input * scale;
		
		float fCos = Mathf.Cos(yRot);
		float fSin = Mathf.Sin(yRot);
		
		right = Vector3.right * (1-fSin);
		if (right.x == 0){
			right.x = 1;
			if (fCos < 0){
				right.x *= -1;
			}
		}
		forward = Vector3.forward * fCos;
		if (forward.z == 0){
			forward.z = 1;
			if (fSin < 0){
				forward.z *= -1;
			}
		}
		tVal = right + forward;
		
		
		/*
		float fCos = Mathf.Cos (yRot);
		float fSin = Mathf.Sin (yRot);
		float fSum = 1.0f-fCos;
		// multiply retVal by fCos
		tVal.z *= fSin;
		tVal.x *= fSin;
		
		if (yRot != 0 && yRot != 180){
			// if iScale.x == 0 add tVal.x
			if (iScale.x == 0){
				iScale.z *= 1+tVal.z;
				iScale.x += (tVal.x);
			}else if (iScale.z == 0){
				iScale.x *= 1+tVal.x;
				iScale.z += (tVal.z);
			}
		}//*/
		
		//print ("Dx, Dz = <"+(iScale.x-Camera.current.transform.position.x) + ", "+(iScale.z - Camera.current.transform.position.z) + ">");
		
		// return iScale
		return x_Vectors(iScale, tVal);
	}
	// rotataes <pos> around an arbitrary axis -- not working how it should
	public static Vector3 rotate(Vector3 pos, float rot, Vector3 axis){
		Vector3 retVal = Vector3.zero;
		Vector3 m_pos = pos, m_ax = axis; // m_pos = position, m_ax = rotation axis
		float m_rot = rot;
		float w;
		Matrix4x4 r = Matrix4x4.zero;
		
		// set matrix values according to m_ax value
		float fCos = Mathf.Cos (m_rot);
		float fSin = Mathf.Sin (m_rot);
		float fSum = 1.0f - fCos;
		
		m_ax.Normalize();
		
		r.m00 = (m_ax.x * m_ax.x) * fSum + fCos;
		r.m01 = (m_ax.x * m_ax.y) * fSum - (m_ax.z * fSin);
		r.m01 = (m_ax.x * m_ax.z) * fSum + (m_ax.y * fSin);
		
		r.m10 = (m_ax.y * m_ax.x) * fSum + (m_ax.z * fSin);
		r.m11 = (m_ax.y * m_ax.y) * fSum + fCos;
		r.m12 = (m_ax.y * m_ax.z) * fSum - (m_ax.x * fSin);
		
		r.m20 = (m_ax.z * m_ax.x) * fSum - (m_ax.y * fSin);
		r.m21 = (m_ax.z * m_ax.y) * fSum + (m_ax.x * fSin);
		r.m22 = (m_ax.z * m_ax.z) * fSum + fCos;
		
		r.m03 = r.m13 = r.m23 = r.m30 = r.m31 = r.m32 = 0.0f;
		r.m33 = 1.0f;
		
		retVal.x = m_pos.x * r.m00 + m_pos.y * r.m10 + m_pos.z * r.m20 + r.m30;
		retVal.y = m_pos.x * r.m01 + m_pos.y * r.m11 + m_pos.z * r.m21 + r.m31;
		retVal.z = m_pos.x * r.m02 + m_pos.y * r.m12 + m_pos.z * r.m22 + r.m32;
		w = m_pos.x * r.m03 + m_pos.y * r.m13 + m_pos.z * r.m23 + r.m33;
		
		retVal.x /= w;
		retVal.y /= w;
		retVal.z /= w;
		
		print ("Rotation Vector = <"+retVal.x + ", "+ retVal.y + ", "+retVal.z + ">");
		
		return retVal;
	}
	
	// Use this for initialization
	void Start () {
		JobController.Start();
	}
	
	/*
	
	// Update is called once per frame
	void Update () {
		
		Controller.Update();		
		
		if (Input.GetKey(KeyCode.UpArrow)){
			y += 1;
		} else if (Input.GetKey(KeyCode.DownArrow)){
			y -= 1;
		} else if (Input.GetKey(KeyCode.RightArrow)){
			x += 1;
		} else if (Input.GetKey(KeyCode.LeftArrow)){
			x -= 1;
		} else if (Input.GetKey(KeyCode.A)){
			rotationY += 1;
		} else if (Input.GetKey(KeyCode.D)){
			rotationY -= 1;
		} else if (Input.GetKey(KeyCode.W)){
			rotationX += 1;
		} else if (Input.GetKey(KeyCode.S)){
			rotationX -= 1;
		} else if (Input.GetKey(KeyCode.Z)){
			rotationZ += 1;
		} else if (Input.GetKey(KeyCode.X)){
			rotationZ -= 1;
		}
		
		z += ((Input.GetAxis("Mouse ScrollWheel")));

		
		transform.position = getVector();
		transform.Rotate((rotationX), (rotationY), (rotationZ));
		rotationX = 0;
		rotationY = 0;
		rotationZ = 0;
	}
	
	public Vector3 getVector(){
		Vector3 vector = new Vector3(x, y, z);
		return vector;
	}*/
	
	
}
