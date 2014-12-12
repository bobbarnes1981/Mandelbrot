using System;

namespace Mandelbrot
{
    class Program
    {
        static void Main(string[] args)
        {
            double L = -2;
            double R = 1;
            double T = 1.5;
            double B = -1.5;
            double W = R - L;
            double H = T - B;

            int mag = 1;
            double bitW = W / mag;
            double bitH = H / mag;

            Mandelbrot mySet = new Mandelbrot();

            for (int Y = 0; Y < mag; Y++)
            {
                for (int X = 0; X < mag; X++)
                {
                    string outputfilename = string.Format("output_{0}_{1}.bmp", X, Y);
                    //mySet.GenerateBitmap(L, R, T, B, outputfilename.c_str());
                    double cL = L + (bitW * X);
                    double cR = L + (bitW * (X + 1));
                    double cT = B + (bitH * (Y + 1));
                    double cB = B + (bitH * Y);
                    Console.WriteLine("{0} {1} {2} {3} {4}", cL, cR, cT, cB, outputfilename);
                    mySet.GenerateBitmap(cL, cR, cT, cB, outputfilename);
                }
            }
        }
    }
}
