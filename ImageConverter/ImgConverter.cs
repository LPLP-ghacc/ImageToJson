using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageConverter
{
    public static class ImgConverter
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

            JsonBuilder.SaveProfile(array, pathToSave);
        }

        public static void BuildImage(string pathToFile, string pathToSave)
        {
            var profile = JsonBuilder.OpenProfile(pathToFile);
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

        #region FileJsonBuilding

        public static class JsonBuilder
        {
            public static ArrayProfile OpenProfile(string path)
            {
                ArrayProfile data = JsonConvert.DeserializeObject<ArrayProfile>(File.ReadAllText(path));

                return data;
            }

            public static void SaveProfile(ArrayProfile profile, string path)
            {
                var jsonProfile = JsonConvert.SerializeObject(profile);

                File.WriteAllText(path, jsonProfile);
            }
        }

        [Serializable]
        public class ArrayProfile
        {
            public ProfileSize size = new ProfileSize();

            public Pixel[] pixels = new Pixel[0];

            public ArrayProfile(Pixel[] pixels, ProfileSize size)
            {
                this.pixels = pixels;

                this.size = size;
            }
        }

        [Serializable]
        public class ProfileSize
        {
            public string x { get; set; }
            public string y { get; set; }

            public ProfileSize(string x, string y)
            {
                this.x = x;
                this.y = y;
            }

            public ProfileSize()
            {

            }
        }

        [Serializable]
        public class Pixel
        {
            public Pixel(int x, int y, byte a, byte r, byte g, byte b)
            {
                this.x = x.ToString();
                this.y = y.ToString();

                this.a = a;
                this.r = r;
                this.g = g;
                this.b = b;
            }

            public string x { get; set; }
            public string y { get; set; }

            public byte a { get; set; }
            public byte r { get; set; }
            public byte g { get; set; }
            public byte b { get; set; }
        }

        #endregion
    }
}
