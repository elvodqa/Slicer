using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slicer.Core;
using System;

namespace Slicer;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ImGuiRenderer _imGuiRenderer;
    private bool IsEditing;
    private System.Numerics.Vector2 _roomSize = new();
    private System.Numerics.Vector3 _roomColor = new();
    private string _roomName = "";


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

        Global.Room = new();
        Global.Room.Name = "Unnamed";
        Global.Room.BackgroundColor = Color.Black;
        Global.Room.Size = new Vector2(20, 10);

        _roomSize = new System.Numerics.Vector2(Global.Room.Size.X, Global.Room.Size.Y);
        _roomColor = new System.Numerics.Vector3(Global.Room.BackgroundColor.R, Global.Room.BackgroundColor.G, Global.Room.BackgroundColor.B);

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
        GraphicsDevice.Clear(Global.Room.BackgroundColor);

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
            if (ImGui.BeginCombo("", Global.Room.Name))
            {
                // Get every room name from Rooms folder
                string[] rooms = System.IO.Directory.GetFiles("Rooms", "*.json");
                foreach (string room in rooms)
                {
                    string roomName = room.Replace("Rooms\\", "").Replace(".json", "");
                    if (ImGui.Selectable(roomName))
                    {
                        Global.Room = Room.Load(roomName);
                        _roomName = roomName;
                        _roomSize = new System.Numerics.Vector2(Global.Room.Size.X, Global.Room.Size.Y);
                        _roomColor = new System.Numerics.Vector3(Global.Room.BackgroundColor.R, Global.Room.BackgroundColor.G, Global.Room.BackgroundColor.B);
                    }
                }
                ImGui.EndCombo();
            }

        }

        ImGui.End();
    }

    private void EditorImGuiLayout()
    {
        // Room Inspector
        ImGuiWindowFlags inspectorWindowFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
        ImGui.SetNextWindowPos(new(0, 35));
        ImGui.SetNextWindowSize(new(300, Window.ClientBounds.Height - 70));
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
            Global.Room.BackgroundColor = new Color(_roomColor.X, _roomColor.Y, _roomColor.Z);
            ImGui.Text($"Room Color: {Global.Room.BackgroundColor}");
        }

        ImGui.End();

        ImGuiWindowFlags saveWindowFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
        ImGui.SetNextWindowPos(new(0, Window.ClientBounds.Height - 35));
        ImGui.SetNextWindowSize(new(300, 35));
        ImGui.Begin("Save", saveWindowFlags);
        if (ImGui.Button("Save"))
        {
            if (Global.Room.Name == "Unnamed" || Global.Room.Name == "None")
            {
                ImGui.OpenPopup("Save As");

            }
            else
            {
                Global.Room.Save();
            }
        }
        ImGui.SameLine();
        if (ImGui.Button("Load"))
        { }
        ImGui.SameLine();
        if (ImGui.Button("New"))
        { 
            Global.Room = new();
            Global.Room.Name = "Unnamed";
            Global.Room.BackgroundColor = Color.Black;
            Global.Room.Size = new Vector2(20, 10);
        }
        ImGui.SameLine();
        if (ImGui.Button("Delete"))
        { }

        ImGui.SetNextWindowSize(new(300, 200));
        if (ImGui.BeginPopupModal("Save As"))
        {
            ImGui.Text("Name of the map:");
            ImGui.SameLine();
            ImGui.InputText("##SaveAs", ref _roomName, 100);
          
            if (ImGui.Button("Save"))
            {
                Global.Room.Name = _roomName;
                Global.Room.Save();
                ImGui.CloseCurrentPopup();
            }
            
            if (ImGui.Button("Cancel"))
            {
                ImGui.CloseCurrentPopup();
            }
            ImGui.EndPopup();
        }

        ImGui.End();

       
    }
}

