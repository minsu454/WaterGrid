using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LinkedObject : MonoBehaviour
{
    private int maxCount;
    private List<Line> _lineList;
    private Line line;

    public event Action LinkEvent;

    public void StartHold()
    {
        line = GameManager.instance.lineObjectPool.GetObject();

        line.Link(transform);
        line.gameObject.SetActive(true);
    }

    public void EndHold()
    {
        line.Unlink();
        line = null;
    }

    public void EndHold(LinkedObject linkedObject)
    {
        linkedObject.Set(line);
        Set(line);

        LinkEvent?.Invoke();
    }

    public void Set(Line line)
    {
        _lineList.Add(line);
    }
}
