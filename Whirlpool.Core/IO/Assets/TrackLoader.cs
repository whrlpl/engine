using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Audio;

namespace Whirlpool.Core.IO.Assets
{
    public class TrackLoader
    {
        public static Track LoadAsset(string fileName)
        {
            Track track = new Track();
            track.audioFileReader = new NAudio.Wave.AudioFileReader(fileName);
            return track;
        }
    }
}
