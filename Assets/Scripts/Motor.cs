using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Motor : MonoBehaviour
{

	public float			moveSpeed = 10.0f;
	public float			drag = 0.5f;
	public float			terminalRotationSpeed = 25.0f;
	private float			lastBoost;

	public VirtualJoystick	moveJoystick;

	public float			boostSpeed = 40.0f;
	public float			boostCoolDown = 2.0f;

	private Rigidbody		controller;
	private Transform		camTransform;

	private void Start ()
	{
		controller = GetComponent<Rigidbody>();
		controller.maxAngularVelocity = terminalRotationSpeed;
		controller.drag = drag;
		camTransform = Camera.main.transform;
		lastBoost = Time.time - boostCoolDown;
	}
	
	private void Update ()
	{
		Vector3 dir =  Vector3.zero;

		dir.x = Input.GetAxis("Horizontal"); 
		dir.z = Input.GetAxis("Vertical"); 

		if (dir.magnitude > 1)
			dir.Normalize();

		if (moveJoystick.InputDirection != Vector3.zero)
		{
			dir = moveJoystick.InputDirection * 3;
		}

		//Rotate direction Vector with canera
		Vector3 rotatedDir = camTransform.TransformDirection(dir);
		rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);
		rotatedDir = 3 * rotatedDir.normalized * dir.magnitude;

		controller.AddForce(rotatedDir * moveSpeed);

	}

	public void Boost()
	{
		if (Time.time - lastBoost > boostCoolDown)
		{
		//	lastBoost = Time.time;
			controller.AddForce(controller.velocity.normalized * boostSpeed,ForceMode.VelocityChange);
		}
	}
}
