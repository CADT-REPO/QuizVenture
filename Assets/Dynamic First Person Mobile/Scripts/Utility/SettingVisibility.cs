using UnityEngine;

namespace FirstPersonMobileTools.Utility
{
    public class SettingVisibility : MonoBehaviour
    {
        [SerializeField] private GameObject settingObject; // Reference to the Setting GameObject

        private void Start()
        {
            if (settingObject != null)
            {
                settingObject.SetActive(false); // Hide the Setting at the start
            }
            else
            {
                Debug.LogWarning("Setting Object is not assigned in the inspector.");
            }
        }

        // Call this method to show the Setting
        public void ShowSetting()
        {
            if (settingObject != null)
            {
                settingObject.SetActive(true);
            }
        }

        // Call this method to hide the Setting
        public void HideSetting()
        {
            if (settingObject != null)
            {
                settingObject.SetActive(false);
            }
        }
    }
}
