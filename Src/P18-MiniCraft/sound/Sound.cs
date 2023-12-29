// Decompiled with JetBrains decompiler
// Type: GameManager.sound.Sound

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace GameManager.sound
{
  public class Sound
  {
    public static Sound PlayerHurt;
    public static Sound PlayerDeath;
    public static Sound MonsterHurt;
    public static Sound Test;
    public static Sound Pickup;
    public static Sound Bossdeath;
    public static Sound Craft;
    private SoundEffect clip;

    public static void InitSound(ContentManager content)
    {
      if (Sound.PlayerHurt == null)
        Sound.PlayerHurt = new Sound("snd\\playerhurt", content);
      if (Sound.PlayerDeath == null)
        Sound.PlayerDeath = new Sound("snd\\death", content);
      if (Sound.MonsterHurt == null)
        Sound.MonsterHurt = new Sound("snd\\monsterhurt", content);
      if (Sound.Test == null)
        Sound.Test = new Sound("snd\\test", content);
      if (Sound.Pickup == null)
        Sound.Pickup = new Sound("snd\\pickup", content);
      if (Sound.Bossdeath == null)
        Sound.Bossdeath = new Sound("snd\\bossdeath", content);
      if (Sound.Craft != null)
        return;
      Sound.Craft = new Sound("snd\\craft", content);
    }

    private Sound(string name, ContentManager content)
    {
      this.clip = content.Load<SoundEffect>(name);
    }

    public void Play()
    {
      if (!miniprefs.UseSound)
        return;
      this.clip.Play();
    }
  }
}
