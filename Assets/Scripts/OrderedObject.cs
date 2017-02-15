using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OrderedObject : MonoBehaviour
{
	public float ScaleFactor = 1.4f;
	public float ScaleOffset = 1.7f;
	const float MIN_RANGE_Y = 1;
	const float MAX_RANGE_Y = -1;
	Transform _tr;
	// Update is called once per frame
	void Awake ()
	{
		_tr = transform;
		_tr.position = new Vector3(_tr.position.x, _tr.position.y, _tr.position.y);
		Renderer ren = GetComponent<Renderer>();
		if(ren != null)
		{
			Bounds renBound = ren.bounds;
			//int orderId = -(int)(renBound.min.y * 10);
			//spRenderer.sortingOrder = orderId;
			_tr.position = new Vector3(_tr.position.x, _tr.position.y, renBound.min.y);
		}
	}
}