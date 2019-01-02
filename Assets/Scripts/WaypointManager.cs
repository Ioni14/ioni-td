using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private Transform[] intermediates;

    public Transform GetStart()
    {
        return start;
    }

    public Transform GetWaypoint(int index)
    {
        if (IsEnd(index)) {
            return end;
        }

        return intermediates[index];
    }

    public bool IsEnd(int index)
    {
        return intermediates.Length <= index;
    }
}
