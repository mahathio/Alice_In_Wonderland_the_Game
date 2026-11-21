using System.Collections.Generic;
using UnityEngine;

public class PartyFollower : MonoBehaviour
{
    [SerializeField] Transform leader;
    [SerializeField] float moveSpeed = 4.5f;
    [SerializeField] float followDistance = 1.2f;
    [SerializeField] float recordSpacing = 0.25f;

    readonly Queue<Vector3> pathPoints = new Queue<Vector3>();
    Vector3 lastRecordedLeaderPos;

    private void Start()
    {
        if (leader != null)
        {
            lastRecordedLeaderPos = leader.position;
        }
    }

    private void Update()
    {
        if (leader == null) return;

        if (Vector3.Distance(leader.position, lastRecordedLeaderPos) >= recordSpacing)
        {
            pathPoints.Enqueue(leader.position);
            lastRecordedLeaderPos = leader.position;
        }

        float distToLeader = Vector3.Distance(transform.position, leader.position);
        if (distToLeader <= followDistance) return;

        if (pathPoints.Count > 0)
        {
            Vector3 targetPoint = pathPoints.Peek();
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint) <= 0.05f)
            {
                pathPoints.Dequeue();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leader.position, moveSpeed * Time.deltaTime);
        }
    }

    public void OnLeaderWarped(Vector3 delta)
    {
        transform.position += delta;
        lastRecordedLeaderPos += delta;

        if (pathPoints.Count == 0) return;

        Queue<Vector3> shiftedPoints = new Queue<Vector3>(pathPoints.Count);
        while (pathPoints.Count > 0)
        {
            shiftedPoints.Enqueue(pathPoints.Dequeue() + delta);
        }

        while (shiftedPoints.Count > 0)
        {
            pathPoints.Enqueue(shiftedPoints.Dequeue());
        }
    }
}