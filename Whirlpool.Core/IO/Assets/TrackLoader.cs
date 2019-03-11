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
