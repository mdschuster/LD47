using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fade(0f, 2f)); //fade to transparent when scene is loaded
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator fade(float to, float time)
    {
        float alpha = this.gameObject.GetComponent<RawImage>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color theColor = new Color(0, 0, 0, Mathf.Lerp(alpha, to, t));
            this.gameObject.GetComponent<RawImage>().color = theColor;
            yield return null;
        }
        Color newColor = new Color(0, 0, 0, to);
        this.gameObject.GetComponent<RawImage>().color = newColor;
    }
}
