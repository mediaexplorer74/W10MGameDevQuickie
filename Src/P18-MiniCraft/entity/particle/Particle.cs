// Decompiled with JetBrains decompiler
// Type: GameManager.entity.particle.Particle


using System.IO;

#nullable disable
namespace GameManager.entity.particle
{
  public class Particle : Entity
  {
    public Particle()
    {
    }

    public Particle(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
    }
  }
}
