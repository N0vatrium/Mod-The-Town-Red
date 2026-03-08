using UnityEngine;

namespace MTTR.Helpers
{
    internal static class Timing
    {
        private static float TICK_RATE = 1 / 60;
        private static float RARE_TICK_RATE = 1;

        private static float _timeSinceTick = 0f;
        private static float _timeSinceRareTick = 0f;

        /// <summary>
        /// Ticks based on <see cref="TICK_RATE"/>
        /// </summary>
        public static bool IsTick()
        {
            //TODO need to be done in a separate coroutine
            _timeSinceTick += Time.deltaTime;

            if (_timeSinceTick >= TICK_RATE/2)
            {
                return true;
            }

            if (_timeSinceTick >= TICK_RATE)
            {
                _timeSinceTick = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ticks based on <see cref="RARE_TICK_RATE"/>
        /// </summary>
        public static bool IsRareTick() {
            _timeSinceRareTick += Time.deltaTime;

            if (_timeSinceRareTick >= RARE_TICK_RATE)
            {
                _timeSinceRareTick = 0;
                return true;
            }

            return false;
        }
    }
}
