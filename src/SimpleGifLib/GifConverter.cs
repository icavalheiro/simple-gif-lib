using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace SimpleGifLib;
public static class GifConverter
{
    /// <summary>
    /// Converts the given GIF into multiple PNG files and save the files into the dist folder.
    /// </summary>
    /// <param name="pathToGif">Path to the GIF file in the File System</param>
    /// <param name="distFolder">Destination folder where the frames will be saved at.</param>
    /// <returns>A list with the full filenames for each of the frames in order</returns>
    public static string[] ConvertToImages(string pathToGif, string distFolder)
    {
        var gif = Image<Rgba32>.Load(pathToGif);
        var toReturn = new List<string>();

        for(int i = 0; i < gif.Frames.Count; i ++)
        {
            var frame = gif.Frames.CloneFrame(i);
            var frameImagePath = Path.Combine(distFolder, "frame" + i + ".png");
            frame.SaveAsPng(frameImagePath);
            toReturn.Add(frameImagePath);
        }

        return toReturn.ToArray();
    }

    /// <summary>
    /// Loads the gif into memory and tries to read the frametime of the rootframe.
    /// </summary>
    /// <param name="pathToGif">Gif to be read</param>
    /// <returns>The frame time in milliseconds</returns>
    public static int GetGifFrameTime(string pathToGif)
    {
        var gif = Image<Rgba32>.Load(pathToGif);
        int time = gif.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay;
        return time * 10;//Converting (1/100) to (1/1000) time scale
    }
}
