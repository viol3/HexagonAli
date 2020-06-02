using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonAli.Helpers
{
    public class FaderManager : GenericSingleton<FaderManager>
    {
        public Image fadeImage;
        public float fadeTime = 0.25f;

        public IEnumerator CloseTheater()
        {
            fadeImage.raycastTarget = true;
            fadeImage.DOFade(1f, fadeTime);
            yield return new WaitForSeconds(fadeTime);
        }

        public IEnumerator OpenTheater(float delay = 0f)
        {
            fadeImage.DOFade(0f, fadeTime).SetDelay(delay);
            yield return new WaitForSeconds(fadeTime + delay);
            fadeImage.raycastTarget = false;
        }
    }
}
