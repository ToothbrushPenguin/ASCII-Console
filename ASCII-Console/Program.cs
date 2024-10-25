using System.Drawing;

namespace ASCII_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Instantiate the AsciiArt class
            AsciiArt gen = new AsciiArt();

            // Load an image from a file
            string inputImagePath = "input_image.jpg";  // Replace with the actual image path
            string outputImagePath = "output_image.jpg";  // Output file path

            // Make sure the input image path is correct
            if (!System.IO.File.Exists(inputImagePath))
            {
                Console.WriteLine($"Input file not found: {inputImagePath}");
                return;
            }

            // Load the image
            using (Image image = Image.FromFile(inputImagePath))
            {
                // Apply Difference of Gaussians
                Image dogImage = gen.DifferenceOfGaussians(image, 1.0, 0.2); // Using sigma values 1.0 and 2.0

                // Apply Sobel filter on the DoG image
                Image sobelImage = gen.SobelFilter(dogImage);

                // Save the final result as a new image
                string finalOutputPath = "output_sobel_dog.jpg";  // Define the final output image name
                sobelImage.Save(finalOutputPath);

                Console.WriteLine($"Processed image saved to: {finalOutputPath}");
            }

        }
    }
}
