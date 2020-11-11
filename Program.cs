using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace imagesharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = System.IO.Directory.GetFiles("Data");
            foreach(string file in files){
                processFile(file);
            }
        }

        static void processFile(string filename){
            
            int threashold = 130;
            using(var image = Image.Load(filename)){

                int pixelCount = image.Height * image.Width;
                int pixelCountTransparent = 0;
                int pixelCountCloud = 0;
                int pixelCountNotCloud = 0;

                // Image.LoadPixelData()
                for (int y = 0; y < image.Height; y++)
                {
                    Image<Rgba32> image2 = image.CloneAs<Rgba32>();
                    Span<Rgba32> pixelRowSpan = image2.GetPixelRowSpan(y);
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = pixelRowSpan[x];
                        if(pixel.A > 0){
                            if(pixel.R > threashold
                            && pixel.G > threashold
                            && pixel.B > threashold){
                                ++pixelCountCloud;
                            } else{
                                ++pixelCountNotCloud;
                            }

                        } else {
                            ++pixelCountTransparent;
                        }


                        // pixelRowSpan[x] = new Rgba32(x/255, y/255, 50, 255);
                    }
                }

                // Console.WriteLine($"File: {filename}. Transparent: {pixelCountTransparent*100/pixelCount}%, Cloud: {pixelCountCloud*100/(pixelCount-pixelCountTransparent)}%, NotCloud: {pixelCountNotCloud*100/(pixelCount-pixelCountTransparent)}%");
                Console.WriteLine($"File: {filename}. Cloud Cover: {pixelCountCloud*100/(pixelCount-pixelCountTransparent)}%");

                // Console.WriteLine($"Transparent: {pixelCountTransparent}({pixelCountTransparent*100/pixelCount}%)");
                // Console.WriteLine($"Cloud: {pixelCountCloud}({pixelCountCloud*100/(pixelCount-pixelCountTransparent)}%)");
                // Console.WriteLine($"NotCloud: {pixelCountNotCloud}({pixelCountNotCloud*100/(pixelCount-pixelCountTransparent)}%)");
            }
        }
    }
}
