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
    private bool IsEditing;


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
        var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Input.GetKeyboardState();
        Input.GetMouseState();

        if (Input.IsKeyPressed(Keys.F1, true))
        {
            if (!IsEditing)
            {
                SwitchToEditor();
            } else
            {
                SwitchToTesting();
            }
        }

        Input.FixScrollLater();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _imGuiRenderer.BeforeLayout(gameTime);
        DebugImGuiLayout();
        if (!IsEditing)
        {

        }
        if (IsEditing)
        {
            EditorImGuiLayout();
        }
        
        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }

    private void SwitchToTesting()
    {
        IsEditing = false;
    }

    private void SwitchToEditor()
    {
        IsEditing = true;
        // Stop game update


    }


    private void DebugImGuiLayout()
    {
        ImGuiWindowFlags topWindowFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 0));
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(Window.ClientBounds.Width, 35));
        ImGui.Begin("| Editor", topWindowFlags);
        if (Global.Room == null)
        {
            ImGui.Text($"| Room not set! ");
        }
        else
        {
            ImGui.Text($"| Room: {Global.Room.Name} ");
        }
        ImGui.SameLine();
        ImGui.Text($"| FPS: {Math.Round(ImGui.GetIO().Framerate)} ");
        ImGui.SameLine();
        if (IsEditing)
        {
            ImGui.Text("| Editing ");
        }
        else
        {
            ImGui.Text("| Playing ");
        }
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

        if (IsEditing)
        {
            ImGui.SameLine();
            // TODO: COMBO BOX for room selector. Also a 'New' and 'Save' button for saving rooms.
        }

        ImGui.End();
    }

    private System.Numerics.Vector2 _roomSize = new();
    private System.Numerics.Vector3 _roomColor = new();
    private void EditorImGuiLayout()
    {
        // Room Inspector
        ImGuiWindowFlags inspectorWindowFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
        ImGui.SetNextWindowPos(new(0, 35));
        ImGui.SetNextWindowSize(new(300, Window.ClientBounds.Height - 35));
        ImGui.Begin("Inspector", inspectorWindowFlags);
        ImGui.InputFloat2("Size", ref _roomSize);
        if (Global.Room != null)
        {
            Vector2 vec = new((int)_roomSize.X, (int)_roomSize.Y);
            Global.Room.Size = vec;
        }
        ImGui.Separator();
        ImGui.ColorPicker3("Background \nColor", ref _roomColor);
        if (Global.Room != null)
        {
            Color col = new();
            col.R = (byte)_roomColor.X;
            col.G = (byte)_roomColor.Y;
            col.B = (byte)_roomColor.Z;
            Global.Room.BackgroundColor = col;
        }



        ImGui.End();


    }
}

