using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {

    public GameObject ShootProjectile;

    Transform _tr;

	// Use this for initialization
	void Start () {

        _tr = transform;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.y = 1;

            var obj = GameObject.Instantiate(ShootProjectile) as GameObject;
            obj.transform.position = _tr.position;
            obj.GetComponent<Projectile>().TargetPos = targetPos;
        }

    }
}
