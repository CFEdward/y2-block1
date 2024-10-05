using System.Collections;
using UnityEngine;
/*
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float intensity = 0f;
    [SerializeField] private Color color = Color.black;
    [SerializeField] private Material fadeMaterial = null;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        fadeMaterial.SetFloat("Intensity", intensity);
        fadeMaterial.SetColor("FadeColor", color);
        Graphics.Blit(source, destination, fadeMaterial);
    }

    public Coroutine StartFadeIn()
    {
        StopAllCoroutines();
        return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (intensity <= 1f)
        {
            intensity += speed * Time.deltaTime;
            yield return null;
        }
    }

    public Coroutine StartFadeOut()
    {
        StopAllCoroutines();
        return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (intensity >= 0f)
        {
            intensity -= speed * Time.deltaTime;
            yield return null;
        }
    }
}
*/

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private Color fadeColor;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public Coroutine FadeIn()
    {
        return StartCoroutine(FadeRoutine(1f, 0f));
    }

    public Coroutine FadeOut()
    {
        return StartCoroutine(FadeRoutine(0f, 1f));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        //Color newColor;
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            renderer.material.color = newColor;

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;

        renderer.material.color = newColor2;
    }
}