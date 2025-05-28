using Common.Time;
using System;
using UnityEngine;

public class WarningIcon : MonoBehaviour, IObjectPoolable<WarningIcon>
{
    [SerializeField] private Vector3 offsetVec = new Vector3(-0.4f, 0.6f, 2);
    [SerializeField] private SpriteRenderer FillAmountRenderer;     //감지바 랜더러

    private float curCount = 0;
    [SerializeField] private float maxCount;

    private IWarningable warningable = null;
    private MaterialPropertyBlock propertyBlock;                    //머터리얼 복사본 생성하지 않고 값 수정하기 위한 변수

    public event Action<WarningIcon> ReturnEvent;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(IWarningable warningable)
    {
        this.warningable = warningable;
        warningable.Outline.enabled = true;
        transform.position = warningable.transform.position + offsetVec;
        WarningManager.Instance.warningContainer.Add(warningable.Name, warningable);
        ResetIcon();
    }

    /// <summary>
    /// 감지될때 실행될 함수(아직 투명하지만 알람은 울릴때)
    /// </summary>
    public void Warning()
    {
        SetFillAmount(0);
    }

    private void Update()
    {
        if (curCount < maxCount)
            curCount += TimeType.InGame.Get() * Time.deltaTime;
        else
        {
            TimeManager.SetTime(TimeType.InGame, 0f);
            GameOver();
        }

        SetFillAmount(curCount / maxCount);
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

    /// <summary>
    /// 바 멈추기
    /// </summary>
    public void Stop()
    {
        curCount = 0;
        WarningManager.Instance.warningContainer.Remove(warningable.Name, warningable);
        warningable.Outline.enabled = false;
        warningable = null;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }
}
