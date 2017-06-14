using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterdropDistortion : MonoBehaviour
{ 
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float fadeSpeed;
    private float offset;
    private bool activated;
    private Renderer renderer;

    // Use this for initialization
    void Start()
    {
        activated = false;
        renderer = gameObject.GetComponent<Renderer>();
        renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated == true)
        {
            offset += scrollSpeed * Time.deltaTime;
            renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0.0f));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Activate();
        }
    }

    public void Activate()
    {
        activated = true;
        renderer.enabled = true;
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {        
        yield return new WaitForSeconds(fadeSpeed);
        renderer.enabled = false;
        activated = false;
        offset = 0.0f;
    }
}
