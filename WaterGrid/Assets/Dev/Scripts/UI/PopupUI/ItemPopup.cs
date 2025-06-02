using Common.Time;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPopup : BasePopupUI
{
    private int getPumpCount;                                   //펌프 얻는 갯수
    [SerializeField] private TextMeshProUGUI pumpText;          //펌프 갯수 텍스트
    private int getHammerCount;                                 //해머 얻는 갯수
    [SerializeField] private TextMeshProUGUI hammerText;        //해머 갯수 텍스트

    public override void Init<T>(T option)
    {
        base.Init(option);

        getPumpCount = InGameLoader.Dev.GetPumpCount;
        getHammerCount = InGameLoader.Dev.GetHammerCount;
        SetText();

        TimeType.InGame.SetTime(0);
    }

    /// <summary>
    /// Pump Item 얻는 함수
    /// </summary>
    public void GetPump()
    {
        InGameLoader.Item.Upgrade(ItemType.Pump, getPumpCount);
        Close();
    }

    /// <summary>
    /// Hammer Item 얻는 함수
    /// </summary>
    public void GetHammer()
    {
        InGameLoader.Item.Upgrade(ItemType.Hammer, getHammerCount);
        Close();
    }

    /// <summary>
    /// 갯수 텍스트 설정 함수
    /// </summary>
    public void SetText()
    {
        pumpText.text = getPumpCount.ToString();
        hammerText.text = getHammerCount.ToString();
    }

    public override void Close()
    {
        TimeType.InGame.SetTime(1);

        base.Close();
    }
}
