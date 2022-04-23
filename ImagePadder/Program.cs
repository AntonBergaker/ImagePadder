using System;
using System.IO;
using System.Drawing;

namespace ImagePadder {
    internal class Program {
        static void Main(string[] args) {

            if (args.Length < 2) {
                Console.WriteLine("Usage: ImagePadder.exe <input_image_path> <output_image_path>");
                return;
            }

            string inputImagePath = args[0];
            string outputPath = args[1];

            if (!File.Exists(inputImagePath)) {
                Console.WriteLine("Input image could not be found");
                return;
            }

            Image imageBase;

            try {
                imageBase = Image.FromFile(inputImagePath);

            }
            catch (Exception e) {
                Console.WriteLine("Failed to import image");
                return;
            }

            
            if (imageBase is not Bitmap bitmap) {
                Console.WriteLine("Image is not a bitmap");
                return;
            }

            Grid<int> oldImage = ImageToArray(bitmap);

            Grid<int> newImage = new Grid<int>(
                NearestPowerOf2(oldImage.Width), 
                NearestPowerOf2(oldImage.Height)
            );

            int heightPadding = (newImage.Height - oldImage.Height)/2;
            int widthPadding = (newImage.Width - oldImage.Width)/2;

            // Copy center
            for (int x = 0; x < newImage.Width; x++) {
                for (int y = 0; y < newImage.Height; y++) {
                    newImage[x, y] = oldImage[
                        Clamp(x - widthPadding, 0, oldImage.Width-1),
                        Clamp(y - heightPadding, 0, oldImage.Height-1)
                    ];
                }
            }
            

            Image newImageImage = ArrayToImage(newImage);
            newImageImage.Save(outputPath);
        }
        public static Bitmap ArrayToImage(Grid<int> array) {
            // Copy into bitmap
            Bitmap bitmap = new Bitmap(array.Width, array.Height);

            for (int x = 0; x < array.Width; x++) {
                for (int y = 0; y < array.Height; y++) {
                    bitmap.SetPixel(x, y, Color.FromArgb(array[x, y]));
                }
            }
            return bitmap;
        }

        public static Grid<int> ImageToArray(Bitmap bmp) {
            // Slow but I give up
            int width = bmp.Width;
            int height = bmp.Height;
            Grid<int> pixels = new Grid<int>(width, height);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Color pixel = bmp.GetPixel(x, y);
                    pixels[x, y] = pixel.ToArgb();
                }
            }
            return pixels;
        }

        public static int NearestPowerOf2(int number) {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log(number, 2)));
        }

        public static int Clamp(int value, int min, int max) {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
