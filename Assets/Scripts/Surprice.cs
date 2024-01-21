using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Surprice : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _delaySandwichCoroutine;

    [SerializeField] private int _maxFade;
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeDuration = 15f;

    private void Start()
    {
        StartCoroutine(StartFadingAfterDelay());
    }

    private IEnumerator StartFadingAfterDelay()
    {
        yield return new WaitForSeconds(_delay);
        StartCoroutine(FadingSequence());
    }

    private IEnumerator FadingSequence()
    {
        while (true)
        {
            yield return StartCoroutine(FadeCoroutine(true));
            yield return new WaitForSeconds(_delay);
            yield return StartCoroutine(FadeCoroutine(false));
            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator FadeCoroutine(bool fadeIn)
    {
        UnityEngine.Color color = _image.color;

        if (!GameManager.Instance.GetIsOver())
        {
            float normalizedMaxFade = _maxFade / 255f;

            float increment = (15f - _fadeDuration) / 15f;

            float startAlpha = fadeIn ? 0 : normalizedMaxFade;
            float endAlpha = fadeIn ? normalizedMaxFade : 0;

            color = _image.color;

            while ((fadeIn && color.a < endAlpha) || (!fadeIn && color.a > endAlpha))
            {
                color.a += increment * Time.deltaTime;

                color.a = Mathf.Clamp(color.a, 0, normalizedMaxFade);

                _image.color = color;

                yield return null;
            }

            if (fadeIn && color.a >= normalizedMaxFade)
            {
                yield return new WaitForSeconds(_delaySandwichCoroutine);

                while (color.a > 0)
                {
                    color.a -= increment * Time.deltaTime;

                    color.a = Mathf.Clamp(color.a, 0, normalizedMaxFade);

                    _image.color = color;

                    yield return null;
                }
            }
        }
    }
}
