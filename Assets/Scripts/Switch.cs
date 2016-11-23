using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{

	private Transform		target;

	private void OnEnterCollision(Collision col)
	{
		if (target != null)
			Destroy(target.gameObject);
	}

}
