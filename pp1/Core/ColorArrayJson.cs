using Newtonsoft.Json;

namespace pp1.Core
{
    public static class ColorArrayJson
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
        public string? x { get; set; }
        public string? y { get; set; }

        public ProfileSize(string? x, string? y)
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

        public string? x { get; set; }
        public string? y { get; set; }

        public byte? a { get; set; }
        public byte? r { get; set; }
        public byte? g { get; set; }
        public byte? b { get; set; }
    }
}
