using UnityEngine.Events;
using UnityEngine;

namespace FirstPersonMobileTools.Utility
{

    public class Setting : MonoBehaviour
    {

        [SerializeField] private UnityEvent[] Default_Values;
        
        public void Default()
        {
            foreach (var item in Default_Values)
            {
                item?.Invoke();
            }
        }
        private void Start()
        {
            // Uncomment if needed
            // Default(); // Ensure this isn't accidentally triggering Default
        }

    }

}
