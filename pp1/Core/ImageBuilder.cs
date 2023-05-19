using System.Drawing;
using System.Drawing.Imaging;

namespace pp1.Core
{
    public static class ImageBuilder
    {
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
            Bitmap bitmap = new Bitmap(size.x.Parse(), size.y.Parse(), PixelFormat.Format32bppPArgb);

            pixels.ForEach(p =>
            {
                int x = p.x.Parse();
                int y = p.y.Parse();
                int a = p.a.Parse();
                int r = p.r.Parse();
                int g = p.g.Parse();
                int b = p.b.Parse();

                bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(a, r, g, b));
            });

            bitmap.Save(pathToSave);
        }

        private static int Parse(this object s)
        {
            return int.Parse(s.ToString());
        }
    }
}
