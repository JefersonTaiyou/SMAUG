using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] float speedOffset;
    [SerializeField] float timeOffset;
    [SerializeField] Vector2 posOffSet;

    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float topLimit;
    [SerializeField] float bottomLimit;

    private void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = player.transform.position;

        endPos.x += posOffSet.x;
        endPos.y += posOffSet.y;
        endPos.z = -10;

        transform.position = new Vector3(
            Mathf.Clamp(endPos.x, leftLimit, rightLimit), 
            Mathf.Clamp(endPos.y, bottomLimit, topLimit),
            endPos.z
            );


    }

    private void OnDrawGizmos()
    {
        // desenhando a caixa de limite das bordas do game
        //sem mostrar nada Gizmos.color = Color.clear;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
    }

}