using Gommon;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Ryujinx.Common.Utilities
{
    public static class Rainbow
    {
        public static bool CyclingEnabled { get; set; }

        public static void Enable()
        {
            if (!CyclingEnabled)
            {
                CyclingEnabled = true;
                Executor.ExecuteBackgroundAsync(async () =>
                {
                    while (CyclingEnabled)
                    {
                        await Task.Delay(15);
                        Tick();
                    }
                });
            }
        }

        public static void Disable()
        {
            CyclingEnabled = false;
        }
        
        
        public static float Speed { get; set; } = 1;
        
        public static Color Color { get; private set; } = Color.Blue;

        public static void Tick()
        {
            Color = HsbToRgb((Color.GetHue() + Speed) / 360);
            
            UpdatedHandler.Call(Color.ToArgb());
        }

        public static void Reset()
        {
            Color = Color.Blue;
            UpdatedHandler.Clear();
        }

        public static event Action<int> Updated
        {
            add => UpdatedHandler.Add(value);
            remove => UpdatedHandler.Remove(value);
        }

        internal static Event<int> UpdatedHandler = new();

        private static Color HsbToRgb(float hue, float saturation = 1, float brightness = 1)
        {
            int r = 0, g = 0, b = 0;
            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = (hue - (float)Math.Floor(hue)) * 6.0f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            return Color.FromArgb(Convert.ToByte(255), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
    }
}
