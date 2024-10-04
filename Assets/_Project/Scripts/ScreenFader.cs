using System.Collections;
using UnityEngine;

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
