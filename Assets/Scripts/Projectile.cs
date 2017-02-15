using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Projectile : MonoBehaviour {

    public Animator SprAnimator;
    public float MaxShootDistance = 8;
    public Vector3 TargetPos;

    public float ShootTime = 2;
	public float RiseDistCoef = .5f;
	public float RiseTimeCoef = .5f;


    Transform _tr;


    void Awake()
    {
        _tr = transform;

		GameObject.DestroyObject(gameObject, ShootTime);
    }

	// Use this for initialization
	void Start ()
    {
        Vector3 shootDir = TargetPos - _tr.position; shootDir.y = 0;

        ShootTime *= shootDir.magnitude / MaxShootDistance;

//		Vector3 risePos = _tr.position + shootDir * RiseDistCoef;
        Vector3 landPos = _tr.position + shootDir;

//      var seq = DOTween.Sequence();
//		seq.Append(_tr.DOMove(risePos, ShootTime * RiseTimeCoef).SetEase(Ease.Linear));
//		seq.Append(_tr.DOMove(landPos, ShootTime * RiseTimeCoef).SetEase(Ease.Linear));

		_tr.DOMove (landPos, ShootTime).SetEase (Ease.Linear);

        var seq2 = DOTween.Sequence();
		seq2.Append(_tr.DOScale(1.5f, ShootTime * RiseTimeCoef).SetEase(Ease.Linear));
		seq2.Append(_tr.DOScale(1, ShootTime * (1 - RiseTimeCoef)).SetEase(Ease.Linear));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
