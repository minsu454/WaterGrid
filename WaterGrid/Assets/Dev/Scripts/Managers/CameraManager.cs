using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public BoxCollider2D bound;

    public Vector3 offset;

    private Vector3 minbound;   //박스콜라이더 영역 최소값
    private Vector3 maxbound;   //박스콜라이더 영역 최댓값

    private float halfWidth;    //카메라의 반 너비
    private float halfHeight;   //카메라의 반 높이

    private Camera targetCamera;

    public bool enabledMove;

    void Start()
    {
        targetCamera = GetComponent<Camera>();
        minbound = bound.bounds.min;
        maxbound = bound.bounds.max;
    }

    public void LateUpdate()
    {
        if (enabledMove == false)
            return;

        // 해상도 변경 대응
        halfHeight = targetCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

        // 플레이어 카메라 이동
        Vector3 targetPos = offset;

        targetPos.x = Mathf.Clamp(targetPos.x, minbound.x + halfWidth, maxbound.x - halfWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, minbound.y + halfHeight, maxbound.y - halfHeight);

        targetPos = Vector3.Lerp(transform.position, targetPos, 2.5f * Time.deltaTime);
        targetPos.z = this.transform.position.z;

        this.transform.position = targetPos;
    }
}
