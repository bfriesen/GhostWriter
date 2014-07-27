using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GhostWriter
{
    public static class Sound
    {
        private static readonly string[] carriageReturns = new[]
        {
            @"SoundFiles\CarriageReturn1.wav",
            @"SoundFiles\CarriageReturn2.wav",
            @"SoundFiles\CarriageReturn3.wav",
            @"SoundFiles\CarriageReturn4.wav",
            @"SoundFiles\CarriageReturn5.wav",
        };

        private static readonly string[] spaces = new[]
        {
            @"SoundFiles\Space1.wav",
            @"SoundFiles\Space2.wav",
            @"SoundFiles\Space3.wav",
            @"SoundFiles\Space4.wav",
            @"SoundFiles\Space5.wav",
        };

        private static readonly string[] keystrokes = new[]
        {
            @"SoundFiles\Keystroke1.wav",
            @"SoundFiles\Keystroke2.wav",
            @"SoundFiles\Keystroke3.wav",
            @"SoundFiles\Keystroke4.wav",
            @"SoundFiles\Keystroke5.wav",
        };

        private static int _carriageReturnIndex;
        private static int _spaceIndex;
        private static int _keystrokeIndex;

        static Sound()
        {
            Enabled = true;
        }

        public static bool Enabled { get; set; }

        public static void PlayCarriageReturn()
        {
            PlaySpace();
            //if (Enabled)
            //{
            //    mciSendString("play " + (carriageReturns[_carriageReturnIndex++ % carriageReturns.Length]), new StringBuilder(), 65534, IntPtr.Zero);
            //}
        }

        public static void PlaySpace()
        {
            if (Enabled)
            {
                mciSendString("play " + (spaces[_spaceIndex++ % spaces.Length]), new StringBuilder(), 65534, IntPtr.Zero);
            }
        }

        public static void PlayKeystroke()
        {
            if (Enabled)
            {
                mciSendString("play " + (keystrokes[_keystrokeIndex++ % keystrokes.Length]), new StringBuilder(), 65534, IntPtr.Zero);
            }
        }

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
    }
}