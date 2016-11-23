using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour
{
	public float		forceRequired = 10.0f;

	private void		OnCollisionEnter(Collision col)
	{
		if (col.impulse.magnitude > forceRequired)
			Destroy(gameObject);
	}

	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}
}
