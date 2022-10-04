using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellViewer : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        gameObject.SetActive(true);
        
        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        while (transform.localScale.x < GridSystem.CellSize.x * 0.9f)
        {
            transform.localScale = transform.localScale + Vector3.one * 5.0f * Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(GridSystem.CellSize.x * 0.9f, GridSystem.CellSize.y * 0.9f, 0);
    }

    public void StartDelite()
    {
        StartCoroutine(Delite());
    }

    IEnumerator Delite()
    {
        Vector3 scale = transform.localScale;
        while (scale.x > 0.1f)
        {
            float delta = 2.0f * Time.deltaTime;
            scale.x = Mathf.Clamp(scale.x - delta, 0.0f, 5.0f);
            scale.y = Mathf.Clamp(scale.y - delta, 0.0f, 5.0f);
            transform.localScale = scale;
            yield return null;
        }
        Destroy(gameObject);
    }
}
