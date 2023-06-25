using System;
using GLFW;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Audio.OpenAL;
using System.Threading;

namespace PMLabs
{
    public class BC : IBindingsContext
    {
        public IntPtr GetProcAddress(string procName)
        {
            return Glfw.GetProcAddress(procName);
        }
    }

    class Program
    {
        static KeyCallback kc = KeyProcessor;
        static ALDevice device;
        static ALContext context;
        static int buf;
        static int source;

        public static void KeyProcessor(IntPtr window, Keys key, int scanCode, InputState state, ModifierKeys mods)
        {

            int sourcestate;
            AL.GetSource(source, ALGetSourcei.SourceState, out sourcestate);

            if (state == InputState.Press)
            {
                if (key == Keys.Space)
                {
                    if ((ALSourceState)sourcestate == ALSourceState.Paused)
                    {
                        Console.WriteLine("Play");
                        AL.SourcePlay(source);
                    }
                    if ((ALSourceState)sourcestate != ALSourceState.Paused)
                    {
                        Console.WriteLine("Pause");
                        AL.SourcePause(source);
                    }
                }
            }

        }

        public static void InitSound()
        {
            device = ALC.OpenDevice(null);
            context = ALC.CreateContext(device, new ALContextAttributes());
            ALC.MakeContextCurrent(context);

            buf = AL.GenBuffer();
            float C4 = 261.63f;
            float E4 = 329.63f;
            float Fs4 = 369.99f;
            float G4 = 392.00f;
            float Gs4 = 415.30f;
            float A4 = 440.00f;
            float As4 = 466.16f;
            float B4 = 493.88f;
            float C5 = 523.25f;
            float D5 = 587.33f;
            float Ds5 = 622.25f;
            float E5 = 659.25f;
            float F5 = 698.46f;
            float Fs5 = 739.99f;
            float G5 = 783.99f;
            float A5 = 880.00f;
            float C6 = 1046.50f;
            
            float[] melody = {

E5, E5, 0, E5, 0,
C5, E5, 0, G5, 0, 0, 0, G4, 0, 0, 0,

C5, 0, 0, G4, 0, 0, E4, 0, 0,
A4, 0, B4, 0, As4, A4, 0,
G4, E5, 0, G5, A5, 0,
F5, G5, 0, E5, 0, C5, D5, B4, 0, 0,

C5, 0, 0, G4, 0, 0, E4, 0, 0,
A4, 0, B4, 0, As4, A4, 0,
G4, E5, 0, G5, A5, 0,
F5, G5, 0, E5, 0, C5, D5, B4, 0, 0,

G5,Fs5,F5,D5,E5, 0,
G4,A4,C5,0,
A4,C5,D5,0,0,
G5,Fs5,F5,D5,E5,0,
C6,C6,C6,0,0,0,

G5, Fs5, F5, D5, E5, 0,
G4, A4, C5, 0,
A4, C5, D5, 0, 0,
Ds5, 0, 0, D5, 0, 0, C5,0,0,0,0,

C5, C5, C5, 0,
C5, D5, E5, C5, A4, G4, 0,0,0,
C5, C5, C5, 0,
C5, D5, E5, 0, 0, 0,

C5, C5, C5, 0,
C5, D5, E5, C5, A4, G4, 0, 0,
E5, E5, E5, 0,
C5, E5, G5, 0, 0,
G4, 0, 0, 0,

C5, 0, 0, G4, 0, 0, E4, 0, 0,
A4, 0, B4, 0, As4, A4, 0,
G4, E5, 0, G5, A5, 0,
F5, G5, 0, E5, 0, C5, D5, B4, 0,0,

C5, 0, 0, G4, 0, 0, E4, 0, 0,
A4, 0, B4, 0, As4, A4, 0,
G4, E5, 0, G5, A5, 0,
F5, G5, 0, E5, 0, C5, D5, B4, 0,0,

E5, C5, G4, 0,
G4, A4, F5, F5, A4, 0,
B4, A5, A5, A5, G5, F5,
E5, C5, A4, G4, 0, 0,

E5, C5, G4, 0,
G4, A4, F5, F5, A4, 0,
B4, F5, F5, F5, E5, D5, C5,0,
G4,0, E4,0, C4,0,0,0,0,

C5, G4, E4,0,
A4, B4, A4,0,
Gs4,As4, Gs4,0,
G4, Fs4, G4,0,0,0,

 };

            int sampleRate = 48000; // Częstotliwość próbkowania 48 KHz
            double duration = 0.45; // Czas trwania pojedynczej nuty (w sekundach)
            int totalSamples = (int)(sampleRate * duration); // Liczba próbek dla jednej nuty

            short[] data = new short[melody.Length * totalSamples]; // Alokuj tablicę na wszystkie próbki

            // Generuj próbki dla każdej nuty w sekwencji
            for (int i = 0; i < melody.Length; i++)
            {
                float frequency = melody[i];
                if (frequency > 0)
                {
                    
                    for (int j = 0; j < totalSamples; j++)
                    {
                        double time = (double)j / sampleRate;
                        data[i * totalSamples + j] = signal(time, frequency, short.MaxValue);
                        
                    }
                }
            }

            AL.BufferData(buf, ALFormat.Stereo16, data, sampleRate);
            source = AL.GenSource();
            AL.BindBufferToSource(source, buf);
            AL.SourcePlay(source);
            AL.Source(source, ALSourceb.Looping, true);
        }

        public static void FreeSound()
        {
            AL.DeleteSource(source);
            AL.DeleteBuffer(buf);

            if (context != ALContext.Null)
            {
                ALC.MakeContextCurrent(ALContext.Null);
                ALC.DestroyContext(context);
            }
            context = ALContext.Null;

            if (device != ALDevice.Null)
            {
                ALC.CloseDevice(device);
            }
            device = ALDevice.Null;
        }

        public static short signal(double t, double f, double A)
        {
            return (short)(A * Math.Sign(Math.Sin(0.85 * Math.PI * f * t)));
        }

        public static void Main(string[] args)
        {

            Glfw.Init();
            Window window = Glfw.CreateWindow(500, 500, "OpenAL", GLFW.Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Glfw.SetKeyCallback(window, kc);
            InitSound();
            Console.Write(
        @"

____▒▒▒▒▒
—-▒▒▒▒▒▒▒▒▒
—–▓▓▓░░▓░
—▓░▓░░░▓░░░
—▓░▓▓░░░▓░░░
—▓▓░░░░▓▓▓▓ 
——░░░░░░░░
—-▓▓▒▓▓▓▒▓▓
–▓▓▓▒▓▓▓▒▓▓▓
▓▓▓▓▒▒▒▒▒▓▓▓▓
░░▓▒░▒▒▒░▒▓░░
░░░▒▒▒▒▒▒▒░░░
░░▒▒▒▒▒▒▒▒▒░░
—-▒▒▒ ——▒▒▒
–▓▓▓———-▓▓▓
▓▓▓▓———-▓▓▓▓

");

            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents();
            }

            FreeSound();
            Glfw.Terminate();
        }
    }
}
