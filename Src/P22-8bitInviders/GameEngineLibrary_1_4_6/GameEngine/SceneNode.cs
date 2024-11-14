// Decompiled with JetBrains decompiler
// Type: GameEngine.SceneNode
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class SceneNode : List<SceneNode>
  {
    protected SceneNode Root;
    public SceneNode Parent;
    protected GameScreen Screen;
    public string Name;
    public Vector3 Position;
    public Vector3 Direction;
    public Vector3 Up;
    public float Rotation;
    public float Scale;
    public bool Visible = true;
    public bool NodeUpdatesPaused;
    public bool handleInput;
    public bool blockInput = true;
    public bool Delete;

    public virtual float Width { get; set; }

    public virtual float Height { get; set; }

    public virtual Vector3 Pivot { get; set; }

    public SceneNode(GameScreen screen)
    {
      this.Name = "scenenode";
      this.Screen = screen;
      this.Direction = Vector3.Forward;
      this.Up = Vector3.Up;
      this.Scale = 1f;
    }

    public SceneNode(Vector2 pos, GameScreen screen)
    {
      this.Name = "scenenode";
      this.Screen = screen;
      this.Position = pos.ToVector3();
      this.Direction = Vector3.Forward;
      this.Up = Vector3.Up;
      this.Scale = 1f;
    }

    public SceneNode(Vector3 pos, GameScreen screen)
    {
      this.Name = "scenenode";
      this.Screen = screen;
      this.Position = pos;
      this.Direction = Vector3.Forward;
      this.Up = Vector3.Up;
      this.Scale = 1f;
    }

    public SceneNode(string name, GameScreen screen)
    {
      this.Name = name;
      this.Screen = screen;
      this.Direction = Vector3.Forward;
      this.Up = Vector3.Up;
      this.Scale = 1f;
    }

    public SceneNode(string name, Vector2 pos, GameScreen screen)
    {
      this.Name = name;
      this.Screen = screen;
      this.Position = pos.ToVector3();
      this.Direction = Vector3.Forward;
      this.Up = Vector3.Up;
      this.Scale = 1f;
    }

    public SceneNode(string name, Vector3 pos, GameScreen screen)
    {
      this.Name = name;
      this.Screen = screen;
      this.Position = pos;
      this.Direction = Vector3.Forward;
      this.Up = Vector3.Up;
      this.Scale = 1f;
    }

    public virtual void Initialize()
    {
    }

    public virtual void Unload()
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Unload();
    }

    public new void Add(SceneNode childItem)
    {
      childItem.Parent = this;
      childItem.Root = this.Root != null ? this.Root : this;
      base.Add(childItem);
    }

    public virtual void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      for (int index = this.Count - 1; index >= 0; --index)
      {
        if (this[index].Delete)
          this.RemoveChildNodeNow(this[index]);
      }
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Update(gameTime, ref worldTransform);
    }

    public void RemoveChildNodes()
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
      {
        sceneNode.Delete = true;
        sceneNode.RemoveChildNodes();
      }
    }

    public void RemoveByName(string name)
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
      {
        if (sceneNode.Name.ToUpper() == name.ToUpper())
        {
          this.Remove(sceneNode);
          break;
        }
      }
    }

    public void RemoveSelf()
    {
      this.RemoveChildNodes();
      this.Delete = true;
    }

    public void RemoveChildNodeNow(SceneNode node)
    {
      foreach (SceneNode node1 in (List<SceneNode>) node)
        node1.RemoveChildNodeNow(node1);
      this.Remove(node);
    }

    public virtual bool HandleInput(GestureSample gesture)
    {
      if (!this.Visible)
        return false;
      List<SceneNode> sceneNodeList = new List<SceneNode>();
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
      {
        if (sceneNode.handleInput)
          sceneNodeList.Add(sceneNode);
      }
      for (int index = sceneNodeList.Count - 1; index >= 0; --index)
      {
        if (sceneNodeList[index].HandleInput(gesture) && sceneNodeList[index].blockInput)
        {
          this.RemoveChildNodeNow(sceneNodeList[index]);
          this.Add(sceneNodeList[index]);
          return true;
        }
      }
      return false;
    }

    public virtual void SetSourceRect()
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
    }

    public virtual void Draw()
    {
      if (!this.Visible)
        return;
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Draw();
    }

    public virtual void Draw(ICamera camera)
    {
      if (!this.Visible)
        return;
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Draw(camera);
    }
  }
}
