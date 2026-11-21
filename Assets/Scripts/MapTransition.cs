using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    Unity.Cinemachine.CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    enum Direction { Left, Right }

    private void Awake()
    {
        confiner = FindAnyObjectByType<Unity.Cinemachine.CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (confiner != null)
            {
                confiner.BoundingShape2D = mapBoundry;
                confiner.InvalidateBoundingShapeCache();
            }

            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 oldPos = player.transform.position;
        Vector3 newPos = oldPos;

        switch (direction)
        {
            case Direction.Left:
                newPos.x += 2f;
                break;
            case Direction.Right:
                newPos.x -= 2f;
                break;
        }

        player.transform.position = newPos;
        Unity.Cinemachine.CinemachineCore.OnTargetObjectWarped(player.transform, newPos - oldPos);
    }
}