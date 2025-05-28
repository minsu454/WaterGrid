using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private Camera main;
    private Bounds2D bounds;

    private Vector2 startPos;
    private Vector2 cameraPos;
    private float cameraZ = -10;

    [SerializeField] private float speed;

    private float halfWidth;    //카메라의 반 너비
    private float halfHeight;   //카메라의 반 높이

    private event Action OnLateUpdateEvent;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(Bounds2D bounds)
    {
        this.bounds = bounds;
        main = Camera.main;

        SetCameraRange();
    }

    public void LateUpdate()
    {
        OnLateUpdateEvent?.Invoke();
    }

    public void OnMoveStart(Vector2 startPos)
    {
        this.startPos = startPos;
        cameraPos = main.transform.position;
        OnLateUpdateEvent += OnLateUpdate;
    }

    public void OnMoveStop()
    {
        OnLateUpdateEvent -= OnLateUpdate;
    }

    private void OnLateUpdate()
    {
        Vector2 touchScreenPos = Camera.main.ScreenToViewportPoint(startPos - InputManager.InputScreenPoint);

        touchScreenPos = cameraPos + (touchScreenPos * speed);

        touchScreenPos.x = Mathf.Clamp(touchScreenPos.x, bounds.min.x + halfWidth, bounds.max.x - halfWidth);
        touchScreenPos.y = Mathf.Clamp(touchScreenPos.y, bounds.min.y + halfHeight, bounds.max.y - halfHeight);

        main.transform.position = (Vector3)touchScreenPos + (Vector3.forward * cameraZ);
    }

    public void SetCameraRange()
    {
        halfHeight = main.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Vector3 center = bounds.Center;
        Vector3 size = bounds.Size;

        Gizmos.DrawWireCube(center, size);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
