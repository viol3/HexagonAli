using HexagonAli.Helpers;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class CameraManager : LocalSingleton<CameraManager>
    {
#pragma warning disable 0649
        [SerializeField]
        private Camera mainCamera;
#pragma warning restore 0649
        public void Init()
        {
            mainCamera.orthographicSize = GetOrthographicSize();
        }

        static float GetOrthographicSize()
        {
            float normalSize = GetDefaultOrtographicSize();
            if (GameConfiguration.Instance.columnCount > 8)
            {
                return Mathf.Min(GameConfiguration.Instance.columnCount * normalSize / 8f, 15f);
            }
            return normalSize;
        }

        static float GetDefaultOrtographicSize()
        {
            float ratio = (float)Screen.width / Screen.height;
            if (ratio < 0.5f)
            {
                return 6.3f;
            }
            return 5f;
        }

        public float GetTopOfCamera()
        {
            return mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        }
    }
}
