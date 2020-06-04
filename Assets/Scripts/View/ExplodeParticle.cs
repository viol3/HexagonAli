using System.Collections;
using HexagonAli.Managers;
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

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetColor(Color color)
        {
            var main = _particle.main;
            main.startColor = color;
        }
        public void Explode()
        {
            _particle.Play();
            gameObject.SetActive(true);
            StartCoroutine(DisableAfter(1f));
        }

        IEnumerator DisableAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            PoolManager.Instance.DespawnExplodeParticle(this);
        }
    }
}
