using System;
using GLFW;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Audio.OpenAL;
using System.Threading;

namespace Mario
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

        //pozycja sluchacza X
        static float listenerPosX = 0.0f;

        //pozycja sluchacza Z
        static float listenerPosZ = 0.0f;

        //glosnosc
        static float volume = 0.05f;

        public static void KeyProcessor(IntPtr window, Keys key, int scanCode, InputState state, ModifierKeys mods)
        {

            int sourcestate;
            AL.GetSource(source, ALGetSourcei.SourceState, out sourcestate);

            if (state == InputState.Press)
            {
                if (key == Keys.Space) //obsluga klawisza spacja
                {
                    if ((ALSourceState)sourcestate == ALSourceState.Paused)
                    {
                        Console.WriteLine("Odtwarza"); //uruchamia utwor
                        AL.SourcePlay(source);
                    }
                    if ((ALSourceState)sourcestate != ALSourceState.Paused)
                    {
                        Console.WriteLine("Zatrzymano"); //pauzuje utwor
                        AL.SourcePause(source);
                    }
                }

                if (key == Keys.Up)
                {
                    listenerPosZ -= 1f; //przenosi sluchacza do przodu o 1f 
                    Console.WriteLine("Pozycja przód(-)/tył(+) wynosi: " + listenerPosZ);
                }
                else if (key == Keys.Down)
                {
                    listenerPosZ += 1f; //przenosi sluchacza do tylu o 1f 
                    Console.WriteLine("Pozycja przód(-)/tył(+) wynosi: " + listenerPosZ);
                }
                else if (key == Keys.Left)
                {
                    listenerPosX -= 1f; //przenosi sluchacza w lewo o 1f 
                    Console.WriteLine("Pozycja lewo(-)/prawo(+) wynosi: " + listenerPosX);
                }
                else if (key == Keys.Right)
                {
                    listenerPosX += 1f; //przenosi sluchacza w prawo o 1f
                    Console.WriteLine("Pozycja lewo(-)/prawo(+) wynosi: " + listenerPosX);
                }

                if (key == Keys.NumpadAdd)
                {
                    if (volume >= 0.5f) {
                        Console.WriteLine("Nie radzę dawać głośniej");
                    }
                    else {
                    volume += 0.01f; //glosnosc wyzej o 0.01f
                        Console.WriteLine("Głośność +");
                    }
                }
                if (key == Keys.NumpadSubtract)
                {
                    if (volume == 0)
                    {
                        Console.WriteLine("Dźwięk wyciszony");
                        
                    }
                    else
                    {
                        volume -= 0.01f; //glosnosc wyzej o 0.01f
                        Console.WriteLine("Głośność -");
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

            //Zdefiniowane nuty
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
            

            // Melodia
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

            // Częstotliwość próbkowania 48 KHz
            int sampleRate = 48000;

            // Czas trwania pojedynczej nuty (w sekundach)
            double duration = 0.45;

            // Liczba próbek dla jednej nuty
            int totalSamples = (int)(sampleRate * duration);

            // Alokuj tablicę na wszystkie próbki
            short[] data = new short[melody.Length * totalSamples]; 

            // Generuj próbki dla każdej nuty w sekwencji
            for (int i = 0; i < melody.Length; i++)
            {
                float frequency = melody[i];
                if (frequency > 0)
                {
                    
                    for (int j = 0; j < totalSamples; j++)
                    {   

                        // Oblicz aktualny czas próbki
                        double time = (double)j / sampleRate;

                        // Wygeneruj próbkę dźwiękową dla danej nuty w danym czasie
                        data[i * totalSamples + j] = signal(time, frequency, short.MaxValue);
                        
                    }
                }
            }

            // Przekazanie danych próbek do bufora dźwiękowego
            AL.BufferData(buf, ALFormat.Stereo16, data, sampleRate);

            // Utworzenie źródła dźwięku
            source = AL.GenSource();

            // Powiązanie bufora z źródłem dźwięku
            AL.BindBufferToSource(source, buf);

            // Uruchomienie odtwarzania dźwięku
            AL.SourcePlay(source);

            // Ustawienie zapętlenia odtwarzania
            AL.Source(source, ALSourceb.Looping, true);
        }

        public static void FreeSound()
        {
            // Usunięcie źródła dźwięku
            AL.DeleteSource(source);

            // Usunięcie bufora dźwiękowego
            AL.DeleteBuffer(buf);

            // Zerowanie kontekstu OpenAL
            if (context != ALContext.Null)
            {
                // Przełączenie na nullowy kontekst
                ALC.MakeContextCurrent(ALContext.Null);

                // Zniszczenie kontekstu
                ALC.DestroyContext(context);
            }
            context = ALContext.Null;

            // Zamknięcie urządzenia OpenAL
            if (device != ALDevice.Null)
            {
                ALC.CloseDevice(device);
            }
            device = ALDevice.Null;
        }

        public static short signal(double t, double f, double A)
        {
            // Generowanie próbki dźwiękowej dla danej nuty w danym czasie
            return (short)(A * Math.Sign(Math.Sin(0.85 * Math.PI * f * t)));
        }

        public static void Main(string[] args)
        {
           
            Glfw.Init();

            // Tworzenie okna GLFW
            Window window = Glfw.CreateWindow(200, 200, "Mario", GLFW.Monitor.None, Window.None);

            // Ustawienie utworzonego okna jako bieżącego kontekstu
            Glfw.MakeContextCurrent(window);

            // Ustawienie callbacka dla obsługi klawiatury
            Glfw.SetKeyCallback(window, kc);

            // Inicjalizacja dźwięku
            InitSound();
            Console.Write(
@"


░██████╗██╗░░░██╗██████╗░███████╗██████╗░  ███╗░░░███╗░█████╗░██████╗░██╗░█████╗░  ██████╗░██████╗░░█████╗░░██████╗
██╔════╝██║░░░██║██╔══██╗██╔════╝██╔══██╗  ████╗░████║██╔══██╗██╔══██╗██║██╔══██╗  ██╔══██╗██╔══██╗██╔══██╗██╔════╝
╚█████╗░██║░░░██║██████╔╝█████╗░░██████╔╝  ██╔████╔██║███████║██████╔╝██║██║░░██║  ██████╦╝██████╔╝██║░░██║╚█████╗░
░╚═══██╗██║░░░██║██╔═══╝░██╔══╝░░██╔══██╗  ██║╚██╔╝██║██╔══██║██╔══██╗██║██║░░██║  ██╔══██╗██╔══██╗██║░░██║░╚═══██╗
██████╔╝╚██████╔╝██║░░░░░███████╗██║░░██║  ██║░╚═╝░██║██║░░██║██║░░██║██║╚█████╔╝  ██████╦╝██║░░██║╚█████╔╝██████╔╝
╚═════╝░░╚═════╝░╚═╝░░░░░╚══════╝╚═╝░░╚═╝  ╚═╝░░░░░╚═╝╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░╚════╝░  ╚═════╝░╚═╝░░╚═╝░╚════╝░╚═════╝░

                                           ▒▒▒▒▒▒▒▒▒▄▄▄▄▒▒▒▒▒▒▒
                                           ▒▒▒▒▒▒▄▀▀▓▓▓▀█▒▒▒▒▒▒
                                           ▒▒▒▒▄▀▓▓▄██████▄▒▒▒▒
                                           ▒▒▒▄█▄█▀░░▄░▄░█▀▒▒▒▒
                                           ▒▒▄▀░██▄░░▀░▀░▀▄▒▒▒▒
                                           ▒▒▀▄░░▀░▄█▄▄░░▄█▄▒▒▒
                                           ▒▒▒▒▀█▄▄░░▀▀▀█▀▒▒▒▒▒
                                           ▒▒▒▄▀▓▓▓▀██▀▀█▄▀▀▄▒▒
                                           ▒▒█▓▓▄▀▀▀▄█▄▓▓▀█░█▒▒
                                           ▒▒▀▄█░░░░░█▀▀▄▄▀█▒▒▒
                                           ▒▒▒▄▀▀▄▄▄██▄▄█▀▓▓█▒▒
                                           ▒▒█▀▓█████████▓▓▓█▒▒
                                           ▒▒█▓▓██▀▀▀▒▒▒▀▄▄█▀▒▒
                                           ▒▒▒▀▀▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒


");


            Console.WriteLine("Klawiszami + i - na numpadzie można zmieniać głośność odtwarzania.");
            Console.WriteLine("Klawiszem Spacja można zatrzymywać i odtwarzać utwór.");
            Console.WriteLine("Strzałkami można zmieniać pozycję 'słuchacza'.");


            while (!Glfw.WindowShouldClose(window))
            {
                // Ustawienie poziomu głośności słuchacza
                AL.Listener(ALListenerf.Gain, volume);

                // Aktualizacja pozycji słuchacza dźwięku
                AL.Listener(ALListener3f.Position, listenerPosX, 0f, listenerPosZ);

                // Pobieranie i obsługa zdarzeń okna
                Glfw.PollEvents();

            }

            // Zwolnienie zasobów dźwiękowych
            FreeSound();

            // Zakończenie biblioteki GLFW
            Glfw.Terminate();
        }
    }
}
