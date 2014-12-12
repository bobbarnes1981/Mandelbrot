using System;
using System.IO;

namespace Mandelbrot
{
    class Mandelbrot
    {
        private const byte MAX_ITERATIONS = 0xFF;
        private const double BitmapWidth = 2560;
        private const double BitmapHeight = 2048;

        private Colour[] Colours = new Colour[MAX_ITERATIONS];

        private Colour getColour(double pX, double pY)
        {
            double x1, x0;
            x1 = x0 = pX;
            double y1, y0;
            y1 = y0 = pY;
            int iteration = 0;

            while (x1 * x1 + y1 * y1 <= (2 * 2) && iteration < MAX_ITERATIONS)
            {
                double xtemp = x1 * x1 - y1 * y1 + x0;
                y1 = 2 * x1 * y1 + y0;
                x1 = xtemp;
                iteration++;
            }
            if (iteration == MAX_ITERATIONS)
            {
                return new Colour(0, 0, 0);
            }
            return Colours[iteration];
        }

        public Mandelbrot()
        {
            // generate colour list
            int increment = 0;
            for (byte r = 0x00; r < 0xff && increment < MAX_ITERATIONS; r += 25)
            {
                for (byte g = 0x00; g < 0xff && increment < MAX_ITERATIONS; g += 25)
                {
                    for (byte b = 0x00; b < 0xff && increment < MAX_ITERATIONS; b += 25)
                    {
                        Colours[increment] = new Colour(r, g, b);
                        increment++;
                    }
                }
            }
        }

        public void GenerateBitmap(double pL, double pR, double pT, double pB, string pFileName)
        {
            /* File INFO
             * (255,255,255)RGB			24 bit per pixel
             * 2560*2048*3 = 15728640 (0xF00000)	raw bitmap size
             * 15728640 + 54 = 15728694 (0xF00036)	file size
             * 2048 = 0x800				height
             * 2560 = 0xa00				width
             */
            byte[] Header = new byte[] {
                0x42, 0x4d,			//	00	2	42 4d		BM
                0x36, 0x00, 0xf0, 0x00,		//	02	4	36 00 f0 00	file size
                0x00, 0x00,			//	06	2	00 00		reserved
                0x00, 0x00,			//	08	2	00 00		reserved
                0x36, 0x00, 0x00, 0x00,		//	10	4	36 00 00 00	offset for data, 54 bytes
                0x28, 0x00, 0x00, 0x00,		//	14	4	28 00 00 00 	size of this header (realitve to here), 40 bytes
                0x00, 0x0a, 0x00, 0x00,		//	18	4	00 0a 00 00	bitmap width in pixels
                0x00, 0x08, 0x00, 0x00,		//	22	4	00 08 00 00	bitmap height in pixels
                0x01, 0x00,			//	26	2	01 00		number of colour planes, 1 (only 1 is valid)
                0x18, 0x00,			//	28	2	18 00		bits per pixel, 24
                0x00, 0x00, 0x00, 0x00,		//	30	4	00 00 00 00 	compression method, none (BI_RGB)
                0x00, 0x00, 0xf0, 0x00,		//	34	4	00 00 f0 00	size of raw bmp data (after header)
                0x13, 0x0b, 0x00, 0x00,		//	38	4	13 0b 00 00	pixels/meter horizontal
                0x13, 0x0b, 0x00, 0x00,		//	42	4	13 0b 00 00	pixels/meter vertical
                0x00, 0x00, 0x00, 0x00,		//	46	4	00 00 00 00	colours in the palette, none
                0x00, 0x00, 0x00, 0x00};	//	50	4	00 00 00 00	important colours, none

            double Width = pR - pL;
            double Height = pT - pB;

            double hstep = (Width/BitmapWidth);
            double vstep = (Height/BitmapHeight);

            Console.WriteLine(pFileName);

            FileStream fileStream = new FileStream(pFileName, FileMode.CreateNew, FileAccess.Write);

            fileStream.Write(Header, 0, Header.Length);

            // if these lops are limited by 'width/height' rounding errors can cause the output to skew
            int ypixelcount = 0;
            for (double y = pB; ypixelcount < BitmapHeight; y+=vstep)
            {
                ypixelcount++;
                int xpixelcount = 0;
                for (double x = pL; xpixelcount < BitmapWidth; x+=hstep)
                {
                    xpixelcount++;
                    Colour currentColour = getColour(x, y);
                    fileStream.Write(currentColour.GetBytes(), 0, 3);
                    //currentColour.Write(cout);
                }
            }

            fileStream.Close();

            Console.WriteLine("Done!");
        }
    }
}
