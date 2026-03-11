using BepInEx.Unity.IL2CPP.UnityEngine;
using MTTR.Helpers;
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
                    Tools.WriteLog("trigger");
                    Tools.SpawnItemAt("efd1b8713911dd74bb4fe054cb71e62a", transform.position);
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
