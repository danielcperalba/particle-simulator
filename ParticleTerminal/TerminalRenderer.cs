using Spectre.Console;
using System.Numerics;
using System.Text;

public class TerminalRenderer
{
    private static readonly char[] Density = { ' ', '.', ':', '+', '*','#', '@'};

    private int _width;
    private int _height;
    private float[,] _heatmap = null!;
    private (float r, float g, float b)[,] _colormap = null!;

    public int Width => _width;
    public int Height => _height;

    public TerminalRenderer()
    {
        Resize();
    }

    public void Resize()
    {
        _width = Console.WindowHeight - 1;
        _height = Console.WindowHeight - 2;
        _heatmap = new float[_width, _height];
        _colormap = new (float, float, float)[_width, _height];
    }

    public void Clear()
    {
        Array.Clear(_heatmap, 0, _heatmap.Length);
        Array.Clear(_colormap, 0, _colormap.Length);
    }

    public void Plot(IReadOnlyList<Particle> particles)
    {
        foreach(var p in particles)
        {
            int x = (int)p.Position.X;
            int y = (int)p.Position.Y;

            if (x < 0 || x >= _width || y < 0 || y >= _height) continue;

            float intensity = p.Life / p.MaxLife; // 1 = novo, 0 = morrendo

            if (intensity > _heatmap[x, y])
            {
                _heatmap[x, y] = intensity;

                _colormap[x, y] = intensity switch
                {
                    > 0.6f => (0.6f, 0.85f, 1.0f), // azul bem claro
                    > 0.3f => (0.3f, 0.60f, 0.9f), // azul médio
                    _ => (0.1f, 0.30f, 0.6f) //azul escuro
                };
            }
        }
    }

    public void Render()
    {
        var sb = new StringBuilder(_width * _height * 20);

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _height; x++)
            {
                float heat = _heatmap[x, y];

                if (heat < 0.01f)
                {
                    sb.Append(' ');
                    continue;
                }

                int charIdx = (int)(heat * (Density.Length - 1));
                charIdx = Math.Clamp(charIdx, 0, Density.Length - 1);
                char c = Density[charIdx];

                // Cor via ANSI RGB
                var (r, g, b) = _colormap[x, y];
                int ri = (int)(r * 255);
                int gi = (int)(g * 255);
                int bi = (int)(b * 255);

                sb.Append($"\x1b[38;2;{ri};{gi};{bi}m{c}\x1b[0m");
            }
            if (y < _height - 1) sb.Append('\n');
        }

        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }
}