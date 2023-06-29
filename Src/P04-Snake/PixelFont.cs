using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Snake
{
    class PixelFont
    {
        int width = 5;
        int height = 5;

        public int Width => width;
        public int Height => height;

        Dictionary<char, byte[]> characters = new Dictionary<char, byte[]>();

        public PixelFont()
        {
            characters = new Dictionary<char, byte[]>()
            {
                ['0'] = new byte[]
                {
                    0b01110,
                    0b10001,
                    0b10101,
                    0b10001,
                    0b01110,
                },
                ['1'] = new byte[]
                {
                    0b01100,
                    0b10100,
                    0b00100,
                    0b00100,
                    0b11111,
                },
                ['2'] = new byte[]
                {
                    0b11110,
                    0b00001,
                    0b01111,
                    0b10000,
                    0b11111,
                },
                ['3'] = new byte[]
                {
                    0b11110,
                    0b00001,
                    0b01110,
                    0b00001,
                    0b11110,
                },
                ['4'] = new byte[]
                {
                    0b00110,
                    0b01010,
                    0b10010,
                    0b11111,
                    0b00010,
                },
                ['5'] = new byte[]
                {
                    0b11111,
                    0b10000,
                    0b11110,
                    0b00001,
                    0b11110,
                },
                ['6'] = new byte[]
                {
                    0b01111,
                    0b10000,
                    0b11110,
                    0b10001,
                    0b01110,
                },
                ['7'] = new byte[]
                {
                    0b11111,
                    0b10001,
                    0b00010,
                    0b00100,
                    0b00100,
                },
                ['8'] = new byte[]
                {
                    0b01110,
                    0b10001,
                    0b01110,
                    0b10001,
                    0b01110,
                },
                ['9'] = new byte[]
                {
                    0b01110,
                    0b10001,
                    0b01111,
                    0b00001,
                    0b11110,
                },
                ['A'] = new byte[]
                {
                    0b01110,
                    0b10001,
                    0b10001,
                    0b11111,
                    0b10001,
                },
                ['B'] = new byte[]
                {
                    0b11110,
                    0b10001,
                    0b11110,
                    0b10001,
                    0b11110,
                },
                ['C'] = new byte[]
                {
                    0b01111,
                    0b10000,
                    0b10000,
                    0b10000,
                    0b01111,
                },
                ['D'] = new byte[]
                {
                    0b11110,
                    0b10001,
                    0b10001,
                    0b10001,
                    0b11110,
                },
                ['E'] = new byte[]
                {
                    0b11111,
                    0b10000,
                    0b11110,
                    0b10000,
                    0b11111,
                },
                ['F'] = new byte[]
                {
                    0b11111,
                    0b10000,
                    0b11110,
                    0b10000,
                    0b10000,
                },
                ['G'] = new byte[]
                {
                    0b01111,
                    0b10000,
                    0b10111,
                    0b10001,
                    0b01110,
                },
                ['H'] = new byte[]
                {
                    0b10001,
                    0b10001,
                    0b11111,
                    0b10001,
                    0b10001,
                },
                ['I'] = new byte[]
                {
                    0b01110,
                    0b00100,
                    0b00100,
                    0b00100,
                    0b01110,
                },
                ['J'] = new byte[]
                {
                    0b01110,
                    0b00100,
                    0b00100,
                    0b00100,
                    0b11000,
                },
                ['K'] = new byte[]
                {
                    0b10001,
                    0b10010,
                    0b11100,
                    0b10010,
                    0b10001,
                },
                ['L'] = new byte[]
                {
                    0b10000,
                    0b10000,
                    0b10000,
                    0b10000,
                    0b11111,
                },
                ['M'] = new byte[]
                {
                    0b10001,
                    0b11011,
                    0b10101,
                    0b10101,
                    0b10001,
                },
                ['N'] = new byte[]
                {
                    0b10001,
                    0b11001,
                    0b10101,
                    0b10011,
                    0b10001,
                },
                ['O'] = new byte[]
                {
                    0b01110,
                    0b10001,
                    0b10001,
                    0b10001,
                    0b01110,
                },
                ['P'] = new byte[]
                {
                    0b11110,
                    0b10001,
                    0b11110,
                    0b10000,
                    0b10000,
                },
                ['Q'] = new byte[]
                {
                    0b01110,
                    0b10001,
                    0b10101,
                    0b10011,
                    0b01111,
                },
                ['R'] = new byte[]
                {
                    0b11110,
                    0b10001,
                    0b11110,
                    0b10001,
                    0b10001,
                },
                ['S'] = new byte[]
                {
                    0b01111,
                    0b10000,
                    0b01110,
                    0b00001,
                    0b11110,
                },
                ['T'] = new byte[]
                {
                    0b11111,
                    0b00100,
                    0b00100,
                    0b00100,
                    0b00100,
                },
                ['U'] = new byte[]
                {
                    0b10001,
                    0b10001,
                    0b10001,
                    0b10001,
                    0b01110,
                },
                ['V'] = new byte[]
                {
                    0b10001,
                    0b10001,
                    0b10001,
                    0b01010,
                    0b00100,
                },
                ['W'] = new byte[]
                {
                    0b10001,
                    0b10101,
                    0b10101,
                    0b01010,
                    0b01010,
                },
                ['X'] = new byte[]
                {
                    0b10001,
                    0b01010,
                    0b00100,
                    0b01010,
                    0b10001,
                },
                ['Y'] = new byte[]
                {
                    0b10001,
                    0b01010,
                    0b00100,
                    0b00100,
                    0b00100,
                },
                ['Z'] = new byte[]
                {
                    0b11111,
                    0b00010,
                    0b00100,
                    0b01000,
                    0b11111,
                },
                ['<'] = new byte[]
                {
                    0b00010,
                    0b00100,
                    0b01000,
                    0b00100,
                    0b00010,
                },
                ['>'] = new byte[]
                {
                    0b01000,
                    0b00100,
                    0b00010,
                    0b00100,
                    0b01000,
                },
            };
        }

        public void DrawString(string message, Action<int, int> renderPixel, bool rightAlign = false)
        {
            if (rightAlign == true)
            {
                DrawString(RightAligned(message), renderPixel);
            }
            else
            {
                (int x, int y) position = (0, 0);
                foreach (var character in message.ToUpper())
                {
                    if (character == '\n')
                    {
                        position.y += height + 1;
                        position.x = 0;
                    }
                    else if (characters.ContainsKey(character) == false)
                    {
                        position.x += width + 1;
                    }
                    else
                    {
                        var c = characters[character];
                        for (var y = 0; y < height; y++)
                        {
                            for (var x = 0; x < width; x++)
                            {
                                var bit = ((c[y] >> (width - 1 - x)) & 1) == 1 ? true : false;
                                if (bit)
                                {
                                    renderPixel(x + position.x, y + position.y);
                                }
                            }
                        }
                        position.x += width + 1;
                    }
                }
            }
        }

        private string RightAligned(string message)
        {
            var lines = message.Split('\n');
            var widestLine = lines.Max(line => line.Length);
            var paddedLines = lines.Select(line => line.PadLeft(widestLine));
            return string.Join("\n", paddedLines);
        }
    }
}
