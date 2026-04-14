using System.Numerics;
using System.Xml.Schema;

public interface IForce { void Apply(Particle p); }

public class Gravity : IForce
{
    public float Strength = 9.8f;
    public void Apply(Particle p) =>
        p.ApplyForce(new Vector2(0, Strength * p.Mass));
}

public class Wind : IForce
{
    public float Strength = 0f;
    public void Apply(Particle p) =>
        p.ApplyForce(new Vector2(Strength * p.Mass, 0));
}