using System.Diagnostics;
using System.Numerics;

Console.CursorVisible = false;
Console.Clear();

var world = new ParticleWorld();
var renderer = new TerminalRenderer();
var rng = Random.Shared;
var clock = Stopwatch.StartNew();
long lastMs = 0;

var gravity = new Gravity { Strength = 60f };
var wind = new Wind { Strength = 0f };

world.AddForce(gravity);
world.AddForce(wind);

int emitRate = 5;
float windStep = 3f;
int rateStep = 2;
int minRate = 1;
int maxRate = 40;
float maxWind = 60f;

void DrawHud()
{
    string windDir = wind.Strength < -0.5f ? "<-" : wind.Strength > 0.5f ? "->" : ".";
    string windStr = $"Vento: {windDir} {Math.Abs(wind.Strength),5:F1}";
    string rainStr = $"Chuva: {new string('|', emitRate / 2 + 1)} {emitRate}gps";
    string controls = "↑↓ densidade   ←→ vento   Q sair";

    Console.SetCursorPosition(0, renderer.Height);
    Console.Write($"\x1b[36m{windStr} {rainStr} \x1b[90m{controls}\x1b[0m   ");
}

while (true)
{
    long now = clock.ElapsedMilliseconds;
    float dt = Math.Min((now - lastMs) / 1000f, 0.05f);
    lastMs = now;

    // Emite particulas em leque
    for (int i = 0; i < emitRate; i++)
    {
        float spawnX = (float)rng.NextDouble() * renderer.Width;

        float baseSpeedY = 25f + (float)rng.NextDouble() * 15f;
        float baseSpeedX = wind.Strength * 0.4f;

        world.Emit(
            position: new Vector2(spawnX, 0),
            velocity: new Vector2(baseSpeedX, baseSpeedY),
            life: (float)renderer.Height / baseSpeedY + 0.5f
        );
    }

    world.Update(dt);

    renderer.Clear();
    renderer.Plot(world.Particles);
    renderer.Render();
    DrawHud();

    // Controle básico no teclado
    while (Console.KeyAvailable)
    {
        var key = Console.ReadKey(intercept: true).Key;
        switch (key)
        {
            case ConsoleKey.UpArrow:
                emitRate = Math.Min(emitRate + rateStep, maxRate);
                break;
            case ConsoleKey.DownArrow:
                emitRate = Math.Max(emitRate - rateStep, minRate);
                break;
            case ConsoleKey.RightArrow:
                wind.Strength = Math.Min(wind.Strength + windStep, maxWind);
                break;
            case ConsoleKey.LeftArrow:
                wind.Strength = Math.Max(wind.Strength - windStep, -maxWind);
                break;
            case ConsoleKey.Q:
                Console.CursorVisible = true;
                Console.Clear();
                return;
        }
    }

    int elapsed = (int)(clock.ElapsedMilliseconds - now);
    if (elapsed < 16) Thread.Sleep(16 - elapsed);
}