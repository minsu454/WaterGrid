using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningIcon : MonoBehaviour, IObjectPoolable<WarningIcon>
{
    [SerializeField] private Vector3 offsetVec = new Vector3(-0.4f, 0.6f, 2);
    [SerializeField] private SpriteRenderer FillAmountRenderer;     //감지바 랜더러

    private MaterialPropertyBlock propertyBlock;                    //머터리얼 복사본 생성하지 않고 값 수정하기 위한 변수

    public event Action<WarningIcon> ReturnEvent;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(Transform targetTr)
    {
        transform.position = targetTr.position + offsetVec;
        ResetIcon();
    }

    /// <summary>
    /// 감지될때 실행될 함수(아직 투명하지만 알람은 울릴때)
    /// </summary>
    public void Warning()
    {
        SetFillAmount(0);
    }

    /// <summary>
    /// 드러날때 실행될 함수(태블릿에서 확인해서 투명화 해제)
    /// </summary>
    public void GameOver()
    {
        Debug.Log("GameOver");
    }

    /// <summary>
    /// 아이콘 리셋해주는 함수
    /// </summary>
    private void ResetIcon()
    {
        FillAmountRenderer.gameObject.SetActive(true);
        SetFillAmount(0);
    }

    /// <summary>
    /// 감지바 설정 함수
    /// </summary>
    public void SetFillAmount(float value)
    {
        FillAmountRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_FillAmount", value);
        FillAmountRenderer.SetPropertyBlock(propertyBlock);
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }
}
