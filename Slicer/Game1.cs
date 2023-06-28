using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slicer;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ImGuiRenderer _imGuiRenderer;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.Title = "Slicer";

        _graphics.PreferredBackBufferWidth = 1080;
        _graphics.PreferredBackBufferHeight = 680;
        _graphics.ApplyChanges();
        
    }

    protected override void Initialize()
    {
        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _imGuiRenderer.BeforeLayout(gameTime);
        EditorImGuiLayout();
        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }

    private void EditorImGuiLayout()
    {
        // No title bar, no resize, no move
        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 0));
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(Window.ClientBounds.Width, 35));
        ImGui.Begin("Editor", windowFlags);
        ImGui.Text("Level 1");
        ImGui.SameLine();
        ImGui.Text("FPS: " + Math.Round(ImGui.GetIO().Framerate));
        //ImGui.SameLine();
        //ImGui.Text("Mouse: " + Mouse.GetState().Position);
        ImGui.SameLine();
        if (ImGui.Button("Stop"))
        {
            //
        }
        ImGui.SameLine();
        if (ImGui.Button("Save Data"))
        {
            
        }
        ImGui.SameLine();
        if (ImGui.Button("Settings"))
        {
            
        }
        ImGui.End();

    }
}

