using System.Numerics;

public class Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float Mass = 1f;
    public float Life;
    public float MaxLife;
    public bool IsAlive => Life > 0f;

    public void ApplyForce(Vector2 force) =>
        Acceleration += force / Mass;

    public void Update(float dt)
    {
        Velocity += Acceleration * dt;
        Position += Velocity * dt;
        Acceleration = Vector2.Zero;
        Life -= dt;
    }
}