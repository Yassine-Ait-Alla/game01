using UnityEngine;
using System.Collections;

public class ground : MonoBehaviour
{
	private void		OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
			Motor.jump = false;
	}
}
