using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed = 2.0f;
    Vector3 objToFollow;
    bool shake;

    [SerializeField] float shakeAmount = 0.1f;

    public void Shake()
    {
        if (shake)
            shake = false;
        else
            shake = true;
    }

    void Update()
    {
        float interpolation = speed * Time.deltaTime;

        objToFollow = new Vector3(player.transform.position.x - 0.2f, player.transform.position.y + 5.5f, player.transform.position.z - 0.2f);

        Vector3 position;
        position.y = Mathf.Lerp(transform.position.y, objToFollow.y, interpolation);
        position.x = Mathf.Lerp(transform.position.x, objToFollow.x, interpolation);
        position.z = Mathf.Lerp(transform.position.z, objToFollow.z, interpolation);

        if (shake)
            position = transform.position + Random.insideUnitSphere * shakeAmount;

        transform.position = position;
    }
}
