using GraphicsEngine;
using GraphicsEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EngineBasics.ScreenManagers
{
    public class ConsoleScreen : IScreenManager
    {
        private readonly char[] ascii_colors = { '@', '#', '*', '=', '+', ':', '-', '.', ' ' }; // { ' ', '.', '-', ':', '+', '=', '*', '@', '#' };

        public List<_Triangle> Triangles { get; set; }
        public List<_Line> Lines { get; set; }

        public char[][] Screen { get; set; }

        public ConsoleScreen(int width, int height)
        {
            width = Width();
            height = Height();

            Screen = new char[height][];
            for (int i = 0; i < height; i++)
                Screen[i] = new char[width];

            Triangles = new List<_Triangle>();
            Lines = new List<_Line>();
        }

        public void ClearScreen()
        {
            Triangles.Clear();
            Lines.Clear();
        }

        public void DrawLine(int x1, int y1, int x2, int y2, EngineColor color)
        {
            Lines.Add(new _Line(new PointF(x1, y1), new PointF(x2, y2), Color.FromArgb(color.A, color.R, color.G, color.B), 0));
        }

        public void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3, EngineColor color)
        {
            Triangles.Add(new _Triangle(new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), Color.FromArgb(color.A, color.R, color.G, color.B)));
        }

        public int Width()
        {
            return 200;
        }

        public int Height()
        {
            return 60;
        }

        public void Init()
        {
            ConsoleHelper.SetCurrentFont("Consolas", 10);
        }

        private char GetColor(int x, int y)
        {
            var a = IsInScreen(x, y);

            return Screen[y][x];
        }

        private char GetCharColor(int r, int g, int b)
        {
            var gray = (int)(r * .2126 + g * .7152 + b * .0722);

            var index = gray * ascii_colors.Length / 255;

            var color = ascii_colors[index];

            return color;
        }

        public void Render()
        {
            var h = Height();
            var w = Width();

            var triangles = new _Triangle[Triangles.Count()];
            Triangles.CopyTo(triangles);

            var lines = new _Line[Lines.Count()];
            Lines.CopyTo(lines);

            Clear(' ');

            foreach (var t in triangles)
            {
                Fill(t, (int)t.Center.X, (int)t.Center.Y, GetCharColor(t.Color.R, t.Color.G, t.Color.B));
            }

            foreach (var line in lines)
            {
                Line(line.P1.X, line.P1.Y, line.P2.X, line.P2.Y, GetCharColor(line.Color.R, line.Color.G, line.Color.B));
            }

            var sb = new StringBuilder();

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    sb.Append(Screen[i][j]);
                }
                sb.Append("\n");
            }

            Console.Write(sb.ToString());
        }

        private void Clear(char background)
        {
            var h = Height();
            var w = Width();

            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    Screen[i][j] = !IsBorder(j, i) ? background : 'X';

            Console.Clear();
        }

        private bool IsBorder(int x, int y) => x <= 0 || x >= Width() - 1 || y <= 0 || y >= Height() - 1;

        private bool IsInScreen(int x, int y) => x >= 0 && x <= Width() - 1 && y >= 0 && y <= Height() - 1;

        public bool IsClosed() => false;

        private void Point(int x, int y, char color)
        {
            Screen[y][x] = color;
        }

        public void Line(double x1, double y1, double x2, double y2, char color)
        {
            int w = (int)x2 - (int)x1;
            int h = (int)y2 - (int)y1;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                Point((int)x1, (int)y1, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x1 += dx1;
                    y1 += dy1;
                }
                else
                {
                    x1 += dx2;
                    y1 += dy2;
                }
            }
        }

        public void Fill(_Triangle t, int x, int y, char color)
        {
            if (IsInScreen(x, y) && Contains(t, x, y) && GetColor(x, y) != color)
            {
                Point(x, y, color);
                Fill(t, x + 1, y + 0, color);
                Fill(t, x + 0, y + 1, color);
                Fill(t, x - 1, y + 0, color);
                Fill(t, x + 0, y - 1, color);

                Fill(t, x + 1, y + 1, color);
                Fill(t, x - 1, y + 1, color);
                Fill(t, x - 1, y - 1, color);
                Fill(t, x + 1, y - 1, color);
            }
        }

        int sign(PointF p1, PointF p2, PointF p3) => (int)(p1.X - p3.X) * (int)(p2.Y - p3.Y) - (int)(p2.X - p3.X) * (int)(p1.Y - p3.Y);

        public bool Contains(_Triangle t, double x, double y)
        {
            var pt = new PointF((float)x, (float)y);

            int d1, d2, d3;
            bool has_neg, has_pos;

            d1 = sign(pt, t.P1, t.P2);
            d2 = sign(pt, t.P2, t.P3);
            d3 = sign(pt, t.P3, t.P1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

    }

    public static class ConsoleHelper
    {
        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
        {
            //Console.WriteLine("Set Current Font: " + font);

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
        }
    }
}
