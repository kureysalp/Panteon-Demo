using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HorizontalObstacle : MonoBehaviour
{
    public Transform obstalce;
    public Transform target;    

    public float time;

    private void Start()
    {
        obstalce.transform.DOMove(target.position, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
