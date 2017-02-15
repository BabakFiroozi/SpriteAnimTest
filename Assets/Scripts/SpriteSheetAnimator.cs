using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteSheetAnimator : MonoBehaviour {

    public enum AnimStates
    {
        None = -1,
        Walk,
        Attack
    }

    AnimStates _currentAnimState = AnimStates.None;

    public float MoveSpeed = 1;
    public int DirectionsCount = 8;
    public Animator SpriteAnimator;

    public float FrameSize = 40;
    public float FramesCountH = 8;
    public float FramesCountV = 8;

    Transform MoveTarget;
    float _lastAngle = -1000;
    float _currentAngle;
    Transform _tr;

    List<float> _anglesList = new List<float>();
    float _lastStepAngleChanged;

    Material _animSprite;

    //public List<SpriteAnimData> AnimData = new List<SpriteAnimData>();

    List<Vector2> _frameUVsVec = new List<Vector2>();

    // Use this for initialization
    void Start () {

        _tr = transform;
        _currentAngle = 0;

        _lastAngle = _currentAngle;

        float angleDiff = 360.0f / DirectionsCount;
        for (float ang = 0; ang <= 360; ang += angleDiff)
        {
            _anglesList.Add(ang);
        }

        var _spriteMat = GetComponent<SpriteRenderer>().material;

        float sheetSizeU = FrameSize * FramesCountH;
        float sheetSizeV = FrameSize * FramesCountV;

        List<Rect> allUVsRect = new List<Rect>();

        //foreach (var data in AnimData)
        //{
        //    Rect uvRect = new Rect();

        //    uvRect.xMin = (float)(((data.startFrame - 1) % FramesCountH) * FrameSize) / sheetSizeU;
        //    uvRect.yMin = (float)(((data.startFrame - 1) % FramesCountV) * FrameSize) / sheetSizeV;

        //    uvRect.width = FrameSize / sheetSizeU;
        //    uvRect.height = FrameSize / sheetSizeV;

        //    allUVsRect.Add(uvRect);
        //}

    }

	
	// Update is called once per frame
	void Update ()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (CameraControl.Instance.TargetTransform == null)
            return;

        MoveTarget = CameraControl.Instance.TargetTransform;

        Vector3 targetDir = MoveTarget.position - _tr.position;
        targetDir.y = 0;
        _currentAngle = Mathf.Atan2(targetDir.z, targetDir.x) * Mathf.Rad2Deg;

        if (_currentAngle > 360) _currentAngle -= 360;
        if (_currentAngle < 0) _currentAngle += 360;

        Debug.Log(_currentAngle);

        for (int i = 0; i < _anglesList.Count - 1; ++i)
        {
            float ang1 = _anglesList[i];
            float ang2 = _anglesList[i + 1];
            if(_currentAngle >= ang1 && _currentAngle < ang2)
            {
                float angHalf = (ang1 + ang2) / 2;
                float needAngle = _currentAngle < angHalf ? ang1 : ang2;
                if (needAngle >= 360)
                    needAngle = 0;
                if(needAngle != _lastStepAngleChanged)
                {
                    AngleChanged();
                    _lastStepAngleChanged = needAngle;
                }
            }
            
        }

        Vector3 movePos = _tr.position;

        if (targetDir.magnitude > .3f)
        {
            Vector3 pos = _tr.position;
            pos += targetDir.normalized * MoveSpeed * Time.deltaTime;
            _tr.position = pos;

            if (_currentAnimState != AnimStates.Walk)
                ChangeState(AnimStates.Walk);
        }
        else
        {
            if (_currentAnimState != AnimStates.Attack)
                ChangeState(AnimStates.Attack);
        }

    }

    void ChangeState(AnimStates state)
    {
        if (state == AnimStates.Walk)
            PlayAnim("walk_");
        if (state == AnimStates.Attack)
            PlayAnim("attack_");
    }

    void AngleChanged()
    {
    }

    public void PlayAnim(string animName)
    {
        string angAnimName = animName + _lastStepAngleChanged.ToString();
        SpriteAnimator.Play(angAnimName);
    }

    public void SetAnimDirection(Vector3 dir)
    {
        _currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
