namespace Mandelbrot
{
    class Colour
    {
        private byte[] col = new byte[3];

        public Colour()
            : this(0x00, 0x00, 0x00)
        {
        }

        public Colour(byte r, byte g, byte b)
        {
            col[2] = r;
            col[1] = g;
            col[0] = b;
        }

        public byte R
        {
            get { return col[2]; }
            set { col[2] = value; }
        }

        public byte G
        {
            get { return col[1]; }
            set { col[1] = value; }
        }

        public byte B
        {
            get { return col[0]; }
            set { col[0] = value; }
        }

        public override string ToString()
        {
            return string.Format("[{0},{1},{2}]", col[2], col[1], col[0]);
        }

        public byte[] GetBytes()
        {
            return col;
        }
    }
}
