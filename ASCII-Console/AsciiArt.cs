using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII_Console
{
    public class AsciiArt
    {

        public Image DifferenceOfGaussians(Image input, double sigma1, double sigma2)
        {
            // Convert Image to Bitmap
            Bitmap inputBitmap = new Bitmap(input);

            // Apply Gaussian Blur with two different sigma values
            Bitmap blurredImage1 = ApplyGaussianBlur(inputBitmap, sigma1);
            Bitmap blurredImage2 = ApplyGaussianBlur(inputBitmap, sigma2);

            // Subtract the two blurred images to get the Difference of Gaussians
            Bitmap dogImage = new Bitmap(inputBitmap.Width, inputBitmap.Height);

            for (int x = 0; x < inputBitmap.Width; x++)
            {
                for (int y = 0; y < inputBitmap.Height; y++)
                {
                    // Get pixels from both blurred images
                    Color pixel1 = blurredImage1.GetPixel(x, y);
                    Color pixel2 = blurredImage2.GetPixel(x, y);

                    // Calculate the difference in grayscale
                    int gray1 = (pixel1.R + pixel1.G + pixel1.B) / 3;
                    int gray2 = (pixel2.R + pixel2.G + pixel2.B) / 3;
                    int diff = Math.Abs(gray1 - gray2);

                    // Clamp the result to [0, 255]
                    diff = Math.Min(255, Math.Max(0, diff));

                    // Set the difference to the DoG image
                    Color diffColor = Color.FromArgb(diff, diff, diff);
                    dogImage.SetPixel(x, y, diffColor);
                }
            }

            return dogImage;
        }

        private Bitmap ApplyGaussianBlur(Bitmap input, double sigma)
        {
            // Approximate Gaussian blur using a simple box filter
            int kernelSize = (int)(6 * sigma + 1); // Typically, kernel size is 6 * sigma + 1
            kernelSize = kernelSize % 2 == 0 ? kernelSize + 1 : kernelSize; // Ensure kernel size is odd

            Bitmap blurredImage = new Bitmap(input.Width, input.Height);

            // For simplicity, apply a box blur here (you can replace this with a more accurate Gaussian blur implementation)
            for (int x = kernelSize / 2; x < input.Width - kernelSize / 2; x++)
            {
                for (int y = kernelSize / 2; y < input.Height - kernelSize / 2; y++)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    for (int i = -kernelSize / 2; i <= kernelSize / 2; i++)
                    {
                        for (int j = -kernelSize / 2; j <= kernelSize / 2; j++)
                        {
                            Color pixel = input.GetPixel(x + i, y + j);
                            sumR += pixel.R;
                            sumG += pixel.G;
                            sumB += pixel.B;
                        }
                    }
                    int count = kernelSize * kernelSize;
                    Color blurredColor = Color.FromArgb(sumR / count, sumG / count, sumB / count);
                    blurredImage.SetPixel(x, y, blurredColor);
                }
            }

            return blurredImage;
        }
        public Image SobelFilter(Image input)
        {
            // Convert the Image to a Bitmap to easily manipulate pixels
            Bitmap inputBitmap = new Bitmap(input);
            Bitmap outputBitmap = new Bitmap(inputBitmap.Width, inputBitmap.Height);

            // Sobel kernels
            int[,] sobelX = new int[,]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };

            int[,] sobelY = new int[,]
            {
                { -1, -2, -1 },
                {  0,  0,  0 },
                {  1,  2,  1 }
            };

            for (int x = 1; x < inputBitmap.Width - 1; x++)
            {
                for (int y = 1; y < inputBitmap.Height - 1; y++)
                {
                    int pixelX = 0;
                    int pixelY = 0;

                    // Apply Sobel filter to calculate X and Y gradients
                    for (int filterX = 0; filterX < 3; filterX++)
                    {
                        for (int filterY = 0; filterY < 3; filterY++)
                        {
                            int imageX = x + filterX - 1;
                            int imageY = y + filterY - 1;

                            // Get pixel color (grayscale for simplicity)
                            Color pixelColor = inputBitmap.GetPixel(imageX, imageY);
                            int grayValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                            pixelX += grayValue * sobelX[filterX, filterY];
                            pixelY += grayValue * sobelY[filterX, filterY];
                        }
                    }

                    // Calculate gradient magnitude and clamp the result to [0, 255]
                    int magnitude = (int)Math.Sqrt((pixelX * pixelX) + (pixelY * pixelY));
                    magnitude = Math.Min(255, Math.Max(0, magnitude));

                    // Set the pixel in the output image
                    Color edgeColor = Color.FromArgb(magnitude, magnitude, magnitude);
                    outputBitmap.SetPixel(x, y, edgeColor);
                }
            }

            return outputBitmap;
        }

    }
}
