using System;

namespace CratePusher.Graphics
{
    public static class AnimationCurves
    {
        public static float Fade(float t)
        {
            if (t > 0.5f) t -= 1;
            return 4 * t * t;
        }

        public static float Movement(float t)
        {
            return t < 0.5f
                ? 2 * t * t
                : 1 - 2 * (t - 1) * (t - 1);
        }
    }
}