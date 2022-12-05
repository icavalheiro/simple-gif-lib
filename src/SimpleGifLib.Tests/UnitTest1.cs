using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace SimpleGifLib.Tests;

public class UnitTest1 : IDisposable
{
    const string IMAGE_A_PATH = "./Assets/a.gif";
    const string IMAGE_A_FRAME_PATH = "./Assets/frame{i}.png";

    readonly string TempPathFolder;

    public UnitTest1()
    {
        TempPathFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(TempPathFolder);
    }

    public void Dispose()
    {
        Directory.Delete(TempPathFolder, true);
    }

    public string GetPathToImageA()
    {
        var currentDir = Directory.GetCurrentDirectory();
        return Path.Combine(currentDir, IMAGE_A_PATH);
    }

    public string GetPathToImageAFrame(int frame)
    {
        var currentDir = Directory.GetCurrentDirectory();
        var framePath = IMAGE_A_FRAME_PATH.Replace("{i}", frame.ToString());
        return Path.Combine(currentDir, framePath);
    }

    [Fact]
    public void Saves8FilesForImageA()
    {
        var observed = GifConverter.ConvertToImages(GetPathToImageA(), TempPathFolder).Length;
        const int expected = 8;

        Assert.Equal(expected, observed);
    }

    [Fact]
    public void Returns80AsFrameDelayForImageA()
    {
        var observed = GifConverter.GetGifFrameTime(GetPathToImageA());
        const int expected = 80;

        Assert.Equal(expected, observed);
    }

    [Fact]
    public void ExtractsFramesCorrectlyForImageA()
    {
        var observedSavedFilePaths = GifConverter.ConvertToImages(GetPathToImageA(), TempPathFolder);

        for(int i = 0; i < 8; i ++)
        {
            IImageFormat format;
            var observedImg = Image<Rgba32>.Load(observedSavedFilePaths[i]);
            var expectedImg = Image<Rgba32>.Load(GetPathToImageAFrame(i), out format);

            var observed = observedImg.ToBase64String(format);
            var expected = expectedImg.ToBase64String(format);

            Assert.Equal(expected, observed);
        }
    }

}