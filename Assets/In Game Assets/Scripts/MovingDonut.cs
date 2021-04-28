using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingDonut : MonoBehaviour
{
    public float time;

    private void Start()
    {
        transform.DOLocalMoveX(-.1f, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
