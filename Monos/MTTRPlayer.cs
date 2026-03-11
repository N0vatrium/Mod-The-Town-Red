using BepInEx.Unity.IL2CPP.UnityEngine;
using UnityEngine;
using KeyCode = BepInEx.Unity.IL2CPP.UnityEngine.KeyCode;

namespace MTTR.Monos
{
    public class MTTRPlayer : MonoBehaviour
    {
        private bool _triggered = false;
        void Update()
        {
            if (Input.GetKeyInt(KeyCode.P) && !_triggered)
            {
                _triggered = true;
                var pttr = GetComponent<PTTRPlayer>();
                if (pttr != null)
                {
                    

                }

                return;
            }

            if (!Input.GetKeyInt(KeyCode.P))
            {
                _triggered = false;
            }
        }
    }
}
