using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Runtime.InteropServices;

namespace BitmapLib
{
    /// <summary>
    /// Methods to work with Bitmaps as 2D integer arrays.
    /// </summary>
    public class BitmapAdapter
    {
        /// <summary>
        /// Bitmap which is used for processing.
        /// </summary>
        public Bitmap Image { get; set; }

        /// <summary>
        /// Defines the parameter for which to generate a 2D Array.
        /// </summary>
        public enum MatrixType { Red, Green, Blue, Alpha };

        /// <summary>
        /// Creates a new BitmapAdapter using an existing Bitmap.
        /// </summary>
        /// <param name="newImage">Existing Bitmap.</param>
        public BitmapAdapter(Bitmap newImage)
        {
            Image = new Bitmap(newImage);
        }

        /// <summary>
        /// Creates a new BitmapAdapter using an existing Image.
        /// </summary>
        /// <param name="newImage">Existing Image.</param>
        public BitmapAdapter(Image newImage)
        {
            Image = new Bitmap(newImage);
        }

        /// <summary>
        /// Creates a new BitmapAdapter with an empty Bitmap of given dimensions.
        /// </summary>
        /// <param name="Width">Width (in pixels) of Bitmap.</param>
        /// <param name="Height">Height (in pixels) of Bitmap.</param>
        public BitmapAdapter(int Width, int Height)
        {
            Image = new Bitmap(Width, Height);
        }

        /// <summary>
        /// Creates a Bitmap from a 2D array containing intensity values of pixels.
        /// </summary>
        /// <param name="IntensityMatrix">Integer array containing intensity values of pixels (0 to 255)</param>
        public void MatrixToBitmap(int[,] IntensityMatrix)
        {
            if (Image == null)
                throw new NullReferenceException("Image cannot be null");

            Bitmap newImage = new Bitmap(IntensityMatrix.GetLength(0), IntensityMatrix.GetLength(1));
            BitmapData imageData;
            IntPtr imagePointer;

            byte[] rawImage = new byte[IntensityMatrix.Length * 4];
            int i = 0, x = 0, y = 0, correctedValue;

            for(x=0; x < IntensityMatrix.GetLength(0); x++)
            {
                for(y=0; y < IntensityMatrix.GetLength(1); y++)
                {
                    if (IntensityMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (IntensityMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = IntensityMatrix[x, y];

                    rawImage[i] = Convert.ToByte(correctedValue);
                    rawImage[i + 1] = Convert.ToByte(correctedValue);
                    rawImage[i + 2] = Convert.ToByte(correctedValue);
                    rawImage[i + 3] = 255;
                    i += 4;
                }
            }

            imageData = newImage.LockBits(new Rectangle(0, 0, IntensityMatrix.GetLength(0), IntensityMatrix.GetLength(1)), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            imagePointer = imageData.Scan0;

            Marshal.Copy(rawImage, 0, imagePointer, rawImage.Length);

            newImage.UnlockBits(imageData);

            Image = newImage;
        }

        /// <summary>
        /// Creates a Bitmap from 2D arrays of R, G, and B values respectively.
        /// </summary>
        /// <param name="RMatrix">2D Array of R values of the pixels.</param>
        /// <param name="GMatrix">2D Array of G values of the pixels.</param>
        /// <param name="BMatrix">2D Array of B values of the pixels.</param>
        public void MatrixToBitmap(int[,] RMatrix, int[,] GMatrix, int[,] BMatrix)
        {
            if (Image == null)
                throw new NullReferenceException("Image cannot be null");
            Bitmap newImage = new Bitmap(RMatrix.GetLength(0), RMatrix.GetLength(1));
            BitmapData imageData;
            IntPtr imagePointer;

            byte[] rawImage = new byte[RMatrix.Length * 4];
            int i = 0, x = 0, y = 0, correctedValue;

            for(x=0; x < RMatrix.GetLength(0); x++)
            {
                for(y=0; y < RMatrix.GetLength(1); y++)
                {
                    if (BMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (BMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = BMatrix[x, y];
                    rawImage[i] = Convert.ToByte(correctedValue);

                    if (GMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (GMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = GMatrix[x, y];
                    rawImage[i + 1] = Convert.ToByte(correctedValue);

                    if (RMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (RMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = RMatrix[x, y];
                    rawImage[i + 2] = Convert.ToByte(correctedValue);

                    rawImage[i + 3] = 255;
                    i += 4;
                }
            }

            imageData = newImage.LockBits(new Rectangle(0, 0, RMatrix.GetLength(0), RMatrix.GetLength(1)), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            imagePointer = imageData.Scan0;
            Marshal.Copy(rawImage, 0, imagePointer, rawImage.Length);
            newImage.UnlockBits(imageData);

            Image = newImage;
        }

        /// <summary>
        /// Creates a Bitmap from 2D arrays of R, G, and B values respectively.
        /// </summary>
        /// <param name="AMatrix">2D Array of Alpha values of the pixels.</param>
        /// <param name="RMatrix">2D Array of R values of the pixels.</param>
        /// <param name="GMatrix">2D Array of G values of the pixels.</param>
        /// <param name="BMatrix">2D Array of B values of the pixels.</param>
        public void MatrixToBitmap(int[,] AMatrix, int[,] RMatrix, int[,] GMatrix, int[,] BMatrix)
        {
            if (Image == null)
                throw new NullReferenceException("Image cannot be null");

            Bitmap newImage = new Bitmap(RMatrix.GetLength(0), RMatrix.GetLength(1));
            BitmapData imageData;
            IntPtr imagePointer;

            byte[] rawImage = new byte[RMatrix.Length * 4];
            int i = 0, x = 0, y = 0, correctedValue;

            for(x=0; x < RMatrix.GetLength(0); x++)
            {
                for(y=0; y < RMatrix.GetLength(1); y++)
                {
                    if (BMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (BMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = BMatrix[x, y];
                    rawImage[i] = Convert.ToByte(correctedValue);

                    if (GMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (GMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = GMatrix[x, y];
                    rawImage[i + 1] = Convert.ToByte(correctedValue);

                    if (RMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (RMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = RMatrix[x, y];
                    rawImage[i + 2] = Convert.ToByte(correctedValue);

                    if (AMatrix[x, y] > 255)
                        correctedValue = 255;
                    else if (AMatrix[x, y] < 0)
                        correctedValue = 0;
                    else
                        correctedValue = AMatrix[x, y];
                    rawImage[i + 3] = Convert.ToByte(correctedValue);
                    i += 4;
                }
            }

            imageData = newImage.LockBits(new Rectangle(0, 0, RMatrix.GetLength(0), RMatrix.GetLength(1)), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            imagePointer = imageData.Scan0;
            Marshal.Copy(rawImage, 0, imagePointer, rawImage.Length);
            newImage.UnlockBits(imageData);

            Image = newImage;
        }

        /// <summary>
        /// Generates an Alpha, R, G, or B 2D array from a Bitmap.
        /// </summary>
        /// <param name="matrixType">Alpha,Red,Blue,Green</param>
        /// <returns>A 2D Array with the A, R, G, or B values of the pixels.</returns>
        public int[,] BitmapToMatrix(MatrixType matrixType)
        {
            if (Image == null)
                throw new NullReferenceException("Image cannot be null");

            BitmapData ImageData;
            IntPtr ImagePointer;
            byte[] rawImage = new byte[Image.Width * Image.Height * 4];

            ImageData = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            ImagePointer = ImageData.Scan0;

            Marshal.Copy(ImagePointer, rawImage, 0, rawImage.Length);

            int[,] outputMatrix = new int[Image.Width, Image.Height];

            int i = 0, x, y;

            switch (matrixType)
            {
                case MatrixType.Blue:
                    for(x = 0; x < Image.Width; x++)
                    {
                        for(y = 0; y < Image.Height; y++)
                        {
                            outputMatrix[x, y] = rawImage[i];
                            i += 4;
                        }
                    }
                    break;

                case MatrixType.Green:
                    for(x = 0; x < Image.Width; x++)
                    {
                        for(y = 0; y < Image.Height; y++)
                        {
                            outputMatrix[x, y] = rawImage[i + 1];
                            i += 4;
                        }
                    }
                    break;

                case MatrixType.Red:
                    for(x = 0; x < Image.Width; x++)
                    {
                        for(y = 0; y < Image.Height; y++)
                        {
                            outputMatrix[x, y] = rawImage[i + 2];
                            i += 4;
                        }
                    }
                    break;

                case MatrixType.Alpha:
                    for(x = 0; x < Image.Width; x++)
                    {
                        for(y = 0; y < Image.Height; y++)
                        {
                            outputMatrix[x, y] = rawImage[i + 3];
                            i += 4;
                        }
                    }
                    break;

                default:
                    break;
            }
            return outputMatrix;
        }

        /// <summary>
        /// Converts a Bitmap to grayscale.
        /// </summary>
        /// <returns>A grayscale Bitmap. Null if BitmapAdapter contains no Bitmap.</returns>
        public Bitmap Grayscale()
        {
            if (Image == null)
                return null;

            Bitmap newImage = new Bitmap(Image.Width, Image.Height);
            Graphics g = Graphics.FromImage(newImage);
            ColorMatrix grayTransform = new ColorMatrix
            (
                new float[][]
                {
                        new float[] {0.299f,0.299f,0.299f,0,0},
                        new float[] {0.587f,0.587f,0.587f,0,0},
                        new float[] {0.114f,0.114f,0.114f,0,0},
                        new float[] {0,0,0,1,0},
                        new float[] {0,0,0,0,1}
                }
            );

            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(grayTransform);

            g.DrawImage(Image, new Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attributes);

            g.Dispose();
            return newImage;
        }

        /// <summary>
        /// Creates a Grayscale Bitmap from color Bitmap.
        /// </summary>
        /// <param name="Image">Color image.</param>
        /// <returns>Returns a grayscale bitmap of the image.</returns>
        public static Bitmap Grayscale(Bitmap Image)
        {
            if (Image == null)
                return null;

            Bitmap newImage = new Bitmap(Image.Width, Image.Height);
            Graphics g = Graphics.FromImage(newImage);
            ColorMatrix grayTransform = new ColorMatrix
            (
                new float[][]
                {
                        new float[] {0.299f,0.299f,0.299f,0,0},
                        new float[] {0.587f,0.587f,0.587f,0,0},
                        new float[] {0.114f,0.114f,0.114f,0,0},
                        new float[] {0,0,0,1,0},
                        new float[] {0,0,0,0,1}
                }
            );

            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(grayTransform);

            g.DrawImage(Image, new Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attributes);

            g.Dispose();
            return newImage;
        }

    }

}
