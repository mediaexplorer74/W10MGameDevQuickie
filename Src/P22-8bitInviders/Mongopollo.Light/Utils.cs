// Decompiled with JetBrains decompiler
// Type: Mangopollo.Utils
// Assembly: Mangopollo.Light, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 113B3C86-1493-4043-B3A9-B6A17EC8E051
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\Mangopollo.Light.dll

using System;
using System.Reflection;

#nullable disable
namespace Mangopollo
{
  public class Utils
  {
    private static readonly Version _targetedVersion8 = new Version(8, 0);
    private static readonly Version _targetedVersion78 = new Version(7, 10, 8858);

    public static bool CanUseLiveTiles => true;//Environment.OSVersion.Version >= Utils._targetedVersion78;

    public static bool IsWP8 => true;//Environment.OSVersion.Version >= Utils._targetedVersion8;

    internal static void SetProperty(object instance, string name, object value)
    { 
      instance.GetType().GetProperty(name).GetSetMethod().Invoke(instance, new object[1]
      {
        value
      });
    }

    internal static object GetProperty(object instance, string name)
    {
      return instance.GetType().GetProperty(name).GetValue(instance, new object[0]);
    }
  }
}
