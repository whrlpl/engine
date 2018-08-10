using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using NAudio; // openal SUCKS i go to BED
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Whirlpool.Core.Audio
{
    public class LoopedWaveStream : WaveStream
    {
        WaveStream sourceWaveStream;

        public LoopedWaveStream(WaveStream sourceWaveStream)
        {
            this.sourceWaveStream = sourceWaveStream;
        }

        public override WaveFormat WaveFormat => sourceWaveStream.WaveFormat;
        public override long Length => sourceWaveStream.Length;
        public override long Position { get => sourceWaveStream.Position; set => sourceWaveStream.Position = value; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int pos = 0;
            while (pos < count)
            {
                int byteCount = sourceWaveStream.Read(buffer, offset + pos, count);
                if (byteCount == 0 || sourceWaveStream.Position > sourceWaveStream.Length)
                {
                    if (sourceWaveStream.Position == 0)
                        break;
                    sourceWaveStream.Position = 0;
                }
                pos += byteCount;
            }
            return pos;
        }
    }
    public class Track
    {
        public AudioFileReader audioFileReader;
        float pitch;
        SmbPitchShiftingSampleProvider pitchShift;
        WaveOutEvent output;

        public EventHandler<StoppedEventArgs> OnPlaybackStop;
        public void Play()
        {
            pitchShift = new SmbPitchShiftingSampleProvider(audioFileReader.ToSampleProvider());
            output = new WaveOutEvent();
            pitchShift.PitchFactor = pitch;
            output.Init(audioFileReader);
            output.PlaybackStopped += OnPlaybackStop;
            output.Play();
        }

        public void PlayBeginning()
        {
            output.Stop();
            output.Play();
        }

        public void SetPitch(float pitch)
        {
            pitchShift.PitchFactor = pitch;
        }

        public void SetVolume(float volume)
        {
            output.Volume = volume;
        }

        public void Stop()
        {
            output.Stop();
        }

    }
}
