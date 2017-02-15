using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class OrderedObjectEditor : MonoBehaviour
{
	public float ScaleFactor = 1.4f;
	public float ScaleOffset = 1.7f;

	// Update is called once per frame
	void Update ()
	{
		var tr = transform;	
		tr.position = new Vector3(tr.position.x, tr.position.y, tr.position.y);
		Renderer ren = GetComponent<Renderer>();
		if(ren != null)
		{
			Bounds renBound = ren.bounds;
			//int orderId = -(int)(renBound.min.y * 10);
			//spRenderer.sortingOrder = orderId;
			tr.position = new Vector3(tr.position.x, tr.position.y, renBound.min.y);
		}
	}
}