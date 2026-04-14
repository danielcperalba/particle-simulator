using System.Numerics;

public class ParticleWorld
{
    private List<Particle> _particles = new();
    private List<IForce> _forces = new();
    public IReadOnlyList<Particle> Particles => _particles;

    public void AddForce(IForce force) => _forces.Add(force);

    public void Emit(Vector2 position, Vector2 velocity, float life)
    {
        _particles.Add(new Particle
        {
            Position = position,
            Velocity = velocity,
            Life = life,
            MaxLife = life
        });
    }

    public void Update(float dt)
    {
        foreach(var p in _particles)
        {
            foreach(var force in _forces)
                force.Apply(p);
            p.Update(dt);
        }
        _particles.RemoveAll(p => !p.IsAlive);
    }
}