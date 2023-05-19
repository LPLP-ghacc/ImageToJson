using pp1.Core;
using System.Drawing;
using System.Drawing.Imaging;

public class Program
{
    public static void Main(string[] args)
    {
        string a = "C:/Users/alpha/source/repos/pp1/pp1/symbolsLibs/atest1.png";

        //SaveImage(a, "C:/Users/alpha/source/repos/pp1/pp1/symbolsLibs/finalimage.json");

        BuildImage("C:/Users/alpha/source/repos/pp1/pp1/symbolsLibs/finalimage.json", "C:/Users/alpha/source/repos/pp1/pp1/symbolsLibs/testafinal.png");
    }

    public static void SaveImage(string pathToImage, string pathToSave)
    {
        var a = new Bitmap(pathToImage);
        var res = a.Size;
        List<Pixel> pixs = new List<Pixel>();

        for (int i = 0; i < res.Height; i++)
        {
            for (int j = 0; j < res.Width; j++)
            {
                var color = a.GetPixel(j, i);

                Pixel p = new Pixel(j + 1, i + 1, color.A, color.R, color.G, color.B);

                pixs.Add(p);
            }
        }

        var array = new ArrayProfile(pixs.ToArray(), new ProfileSize(res.Width.ToString(), res.Height.ToString()));

        ColorArrayJson.SaveProfile(array, pathToSave);
    }

    public static void BuildImage(string pathToFile, string pathToSave)
    {
        var profile = ColorArrayJson.OpenProfile(pathToFile);
        ProfileSize size = profile.size; 
        List<Pixel> pixels = profile.pixels.ToList();
        Bitmap bitmap = new Bitmap(Parse(size.x), Parse(size.y), PixelFormat.Format32bppPArgb);

        pixels.ForEach(p =>
        {
            int x = Parse(p.x);
            int y = Parse(p.y);
            int a = Parse(p.a);
            int r = Parse(p.r);
            int g = Parse(p.g);
            int b = Parse(p.b);

            bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(a, r, g, b));
        });

        bitmap.Save(pathToSave);
    }

    private static int Parse(object s)
    {
        return int.Parse(s.ToString());
    }
}