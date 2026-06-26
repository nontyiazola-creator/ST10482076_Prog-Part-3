using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CybersecurityChatbott
{
    public static class AudioPlayer
    {

        // =========================
        // GREETING SOUND
        // =========================
        public static void PlayGreeting()
        {
            PlayWav("greeting.wav");
        }

        // =========================
        // TOPIC SOUND
        // =========================
        public static void PlayTopicAudio(int topicId)
        {
            string topicFile = $"topic{topicId}.wav";
            string fallback = "select.wav";

            if (!PlayWav(topicFile))
            {
                PlayWav(fallback);
            }
        }

        // =========================
        // SAFE WAV PLAYER
        // =========================
        private static bool PlayWav(string fileName)
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, fileName);

                if (!File.Exists(path))
                {
                    SystemSounds.Beep.Play();
                    return false;
                }

                Task.Run(() =>
                {
                    try
                    {
                        using SoundPlayer player = new SoundPlayer(path);
                        player.Load();
                        player.Play(); // NON-BLOCKING
                    }
                    catch
                    {
                        SystemSounds.Beep.Play();
                    }
                });

                return true;
            }
            catch
            {
                SystemSounds.Beep.Play();
                return false;
            }
        }

        // =========================
        // TEXT TO SPEECH (reflection-based, optional)
        // =========================
        public static void Speak(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            try
            {
                // Try to load System.Speech.Synthesis.SpeechSynthesizer via reflection
                var synthType = Type.GetType("System.Speech.Synthesis.SpeechSynthesizer, System.Speech");
                if (synthType == null)
                {
                    Debug.WriteLine("TTS not available (System.Speech missing)");
                    return;
                }

                var synth = Activator.CreateInstance(synthType);
                if (synth == null)
                    return;

                try
                {
                    synthType.GetProperty("Volume")?.SetValue(synth, 100);
                    synthType.GetProperty("Rate")?.SetValue(synth, 0);
                    var speakMethod = synthType.GetMethod("Speak", new[] { typeof(string) });
                    speakMethod?.Invoke(synth, new object[] { text });
                }
                finally
                {
                    (synth as IDisposable)?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TTS error: " + ex.Message);
                try { SystemSounds.Beep.Play(); } catch { }
            }
        }
    }
}