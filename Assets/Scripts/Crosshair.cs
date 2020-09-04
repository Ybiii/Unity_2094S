using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public float currentSpread;
    public float speedSpread;

    public Parts[] parts;
    public PlayerMovement playerMovement;

    float t;
    float curSpread;

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.moveAmount > 0)
        {
            currentSpread = 10 * (3 + playerMovement.moveAmount);
        }
        else
        {
            currentSpread = 15;
        }

        CrosshairUpdate();
    }

    public void CrosshairUpdate()
    {
        t = 0.005f * speedSpread;
        curSpread = Mathf.Lerp(curSpread, currentSpread, t);

        for (int i = 0; i < parts.Length; i++)
        {
            Parts p = parts[i];
            p.trans.anchoredPosition = p.pos * curSpread;
        }
    }

    [System.Serializable]
    public class Parts
    {
        public RectTransform trans;
        public Vector2 pos;
    }
}
