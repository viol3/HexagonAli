using System.Collections;
using UnityEngine;

namespace HexagonAli.View
{
    public class ExplodeParticle : MonoBehaviour
    {
        private ParticleSystem _particle;
        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }
    
        public void Explode(Vector2 position, Color color)
        {
            var main = _particle.main;
            main.startColor = color;
            transform.position = position;
            _particle.Play();
            gameObject.SetActive(true);
            StartCoroutine(DisableAfter(1f));
        }

        IEnumerator DisableAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(false);
        }
    }
}
