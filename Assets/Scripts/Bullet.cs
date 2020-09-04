using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Speed;
    Vector3 lastPos;

    public GameObject decal;

    private void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        RaycastHit hit;

        Debug.DrawLine(lastPos, transform.position);
        if(Physics.Linecast(lastPos, transform.position, out hit))
        {
            print(hit.transform.name);

            //накладываем нашу декаль
            GameObject d = Instantiate<GameObject>(decal);
            d.transform.position = hit.point + hit.normal * 0.01f;
            d.transform.rotation = Quaternion.LookRotation(-hit.normal);

            //Transform h = Instantiate(decal, hit.point, hitRotation);
            //h.transform.position = hit.point + hit.normal * 0.01f;
            //h.parent = hit.transform;
            Destroy(d, 10);//уничтожение декали через 10 секунд

            //уничтожаем пулю
            Destroy(gameObject);
        }
        lastPos = transform.position;
    }
}
