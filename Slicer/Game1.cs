using FontStashSharp;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slicer.Core;
using Slicer.Core.Solids;
using System;
using System.IO;

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
    private Camera _editorCamera;
    private Camera _gameCamera;
    private Texture2D _tilesetOutside;
    private bool _drawGrid = true;
    private float _viewScale = 8;
    private FontSystem _fontSystem;
    private Rectangle _selectionRect;

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

        Window.ClientSizeChanged += OnWindowResize;


    }

    protected override void Initialize()
    {
        if (!System.IO.Directory.Exists("Rooms"))
        {
            System.IO.Directory.CreateDirectory("Rooms");
        }

        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();

        Global.Room = new();
        Global.Room.Name = "Unnamed";
        Global.Room.BackgroundColor = Color.Black;
        Global.Room.Size = new Vector2(20, 10);

        _roomSize = new System.Numerics.Vector2(Global.Room.Size.X, Global.Room.Size.Y);
        _roomColor = new System.Numerics.Vector3(Global.Room.BackgroundColor.R, Global.Room.BackgroundColor.G, Global.Room.BackgroundColor.B);

        _editorCamera = new(Window.ClientBounds.Width, Window.ClientBounds.Height, new(100, 100));
        _gameCamera = new(Window.ClientBounds.Width, Window.ClientBounds.Height, new(100, 100));

        Global.Camera = _gameCamera;

        _fontSystem = new();
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/Silkscreen/slkscr.ttf"));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tilesetOutside = Content.Load<Texture2D>("Tilesets/outside");
    }

    private Vector2 _previousTileCursorPos;
    protected override void Update(GameTime gameTime)
    {
        var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Input.GetKeyboardState();
        Input.GetMouseState();

        if (ImGui.IsItemFocused() || ImGui.IsAnyItemActive() || ImGui.IsAnyItemFocused())
        {
            
        }
        else
        {
            if (!IsEditing)
            {
                _gameCamera.HandleInput();
            }
            else
            {
                Vector2 cameraMovement = Vector2.Zero;

                if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.A, false))
                {
                    cameraMovement.X = -1;
                }
                else if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D, false))
                {
                    cameraMovement.X = 1;
                }
                if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W, false))
                {
                    cameraMovement.Y = -1;
                }
                else if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S, false))
                {
                    cameraMovement.Y = 1;
                }

                if (cameraMovement != Vector2.Zero)
                {
                    cameraMovement.Normalize();
                }

                if (Input.IsKeyPressed(Keys.LeftShift, false))
                {
                    cameraMovement *= 20f;
                } else
                {
                    cameraMovement *= 10f;
                }
               
                Global.Camera.MoveCamera(cameraMovement);

                if (Input.IsScrolled(Orientation.Up))
                {
                    _editorCamera.AdjustZoom(0.05f);
                           
                }
                if (Input.IsScrolled(Orientation.Down))
                {
                    _editorCamera.AdjustZoom(-0.05f);
  
                }
                if (Input.IsKeyPressed(Keys.G, true)){
                    _drawGrid = !_drawGrid;
                }
                if (Input.IsKeyPressed(Keys.LeftControl, false) && Input.IsKeyPressed(Keys.S, true))
                {

                }

                var gridSize = 16 * _viewScale;
                var mousePos = Global.Camera.ScreenToWorld(new(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));
                int newX = (int)(mousePos.X - mousePos.X % gridSize);
                int newY = (int)(mousePos.Y - mousePos.Y % gridSize);

             
                if (mousePos.X < 0 || mousePos.Y < 0 || mousePos.X > Global.Room.Size.X * gridSize || mousePos.Y > Global.Room.Size.Y * gridSize)
                {
                    _selectionRect = new Rectangle(0, 0, 0, 0);
                }
                else
                {
                    _selectionRect = new Rectangle(newX, newY, (int)gridSize, (int)gridSize);
                }            
            }   
        }
        

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

        if (!IsEditing)
        {

        }
        if (IsEditing)
        {
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Global.Camera.TranslationMatrix);
            _spriteBatch.Draw(_tilesetOutside, new Rectangle(0, 0, 256 * (int)_viewScale, 256 * (int)_viewScale), Color.White);

            Primitives.DrawLine(_spriteBatch, new(0, 0), new(0, Global.Room.Size.Y * 16 * _viewScale), Color.DarkRed, 4);
            Primitives.DrawLine(_spriteBatch, new(0, 0), new(Global.Room.Size.X * 16 * _viewScale, 0), Color.DarkRed, 4);
            Primitives.DrawLine(_spriteBatch, new(0, Global.Room.Size.Y * 16 * _viewScale), new(Global.Room.Size.X * 16 * _viewScale, Global.Room.Size.Y * 16 * _viewScale), Color.DarkRed, 4);
            Primitives.DrawLine(_spriteBatch, new(Global.Room.Size.X * 16 * _viewScale, 0), new(Global.Room.Size.X * 16 * _viewScale, Global.Room.Size.Y * 16 * _viewScale), Color.DarkRed, 4);

            if (_drawGrid)
            {
               
                for (int i = 1; i < Global.Room.Size.X; i++)
                {
                    Primitives.DrawLine(_spriteBatch, new(i * 16 * _viewScale, 0), new(i * 16 * _viewScale, Global.Room.Size.Y * 16 * _viewScale), Color.White, 4);
                }
                for (int i = 1; i < Global.Room.Size.Y; i++)
                {
                    Primitives.DrawLine(_spriteBatch, new(0, i * 16 * _viewScale), new(Global.Room.Size.X * 16 * _viewScale, i * 16 * _viewScale), Color.White, 4);
                }

            }

            Primitives.DrawRect(_spriteBatch, _selectionRect, Color.Blue, 10);

            SpriteFontBase font18 = _fontSystem.GetFont(55);
            _spriteBatch.DrawString(font18, Global.Room.Name, new Vector2(0, -50), Color.White);

            _spriteBatch.End();
         
        }

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

    private void OnWindowResize(object sender, EventArgs e)
    {
        _editorCamera.ViewportWidth = Window.ClientBounds.Width;
        _editorCamera.ViewportHeight = Window.ClientBounds.Height;
    }

    private void SwitchToTesting()
    {
        IsEditing = false;
        
        Global.Camera = _gameCamera;
    }

    private void SwitchToEditor()
    {
        IsEditing = true;
        _editorCamera.Position = new(0, 0);
        _editorCamera.Zoom = 0.4f;
        Global.Camera = _editorCamera;
        
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
                string[] rooms = System.IO.Directory.GetFiles("Rooms", "*.room");
                foreach (string room in rooms)
                {
                    string roomName = room.Replace("Rooms/", "").Replace("Rooms\\", "").Replace(".room", "");
                    if (ImGui.Selectable(roomName))
                    {
                        Global.Room = Room.Load(roomName);
                        _roomName = roomName;
                        _roomSize = new System.Numerics.Vector2(Global.Room.Size.X, Global.Room.Size.Y);
                        _roomColor = new System.Numerics.Vector3(Global.Room.BackgroundColor.R/255f, Global.Room.BackgroundColor.G/255f, Global.Room.BackgroundColor.B/255f);
                    }
                }
                ImGui.EndCombo();
            }
        }
        ImGui.End();
    }
    private Tileset selectedTileset;
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
        ImGui.Separator();
        // Selection of tileset
        ImGui.BeginCombo("Tileset", selectedTileset.ToString());
        var count = Enum.GetNames(typeof(Tileset)).Length;
        for (int i = 0; i < count; i++)
        {
            var tileset = (Tileset)i;
            if (ImGui.Selectable(tileset.ToString()))
            {
                selectedTileset = tileset;
            }
        }
        ImGui.EndCombo();
      
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
        ImGui.SameLine();
        ImGui.Text($"x{Global.Camera.Zoom:0.0##}");
        ImGui.SameLine();
        ImGui.Checkbox("Grid", ref _drawGrid);

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

