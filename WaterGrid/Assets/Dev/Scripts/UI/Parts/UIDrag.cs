using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrag : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
        image.color = new Color(1, 1, 1, 0.5f);
        image.raycastTarget = false;
        image.gameObject.SetActive(true);
    }

    public void Hide() => image.gameObject.SetActive(false);

    public void OnUpdate()
    {
        image.rectTransform.position = InputManager.InputScreenPoint;
    }
}
