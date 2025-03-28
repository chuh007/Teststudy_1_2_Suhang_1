using UnityEngine;

namespace Code.UI
{
    public class ActiveCircle : MonoBehaviour
    {
        private void Awake()
        {
            SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}