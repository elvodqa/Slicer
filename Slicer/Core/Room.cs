using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Slicer.Core
{
    internal class Room
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Room size in terms of tiles. So a 20x10 room is 20*16 x 10*16 in pixel size;
        /// </summary>
        public Vector2 Size { get; set; }
       

        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<Solid> Solids { get; set; } = new List<Solid>();
        public List<Action> Actions { get; set; } = new List<Action>();

        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            if (Actors.Contains(actor))
            {
                Actors.Remove(actor);
            }
        }

        public void Save()
        {
            using (var stream = File.Open(@"Rooms/" + Name + ".room", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                
                
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(Name);
                    writer.WriteLine(BackgroundColor.R + "," + BackgroundColor.G + "," + BackgroundColor.B);
                    writer.WriteLine(Size.X + "," + Size.Y);
                    writer.WriteLine(Actors.Count);
                    foreach (var actor in Actors)
                    {
                        writer.WriteLine($"{actor.GetType().Name}|{actor.Position.X},{actor.Position.Y}");
                    }
                    writer.WriteLine(Solids.Count);
                    foreach (var solid in Solids)
                    {
                        writer.WriteLine($"{solid.GetType().Name}|{solid.Position.X},{solid.Position.Y}");
                    }
                }
            }
        }

        public static Room Load(string name)
        {
            string[] lines = File.ReadAllLines(@"Rooms/" + name + ".room");
            var room = new Room();
            room.Name = lines[0];
            var color = lines[1].Split(',');
            room.BackgroundColor = new Color(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]));
            var size = lines[2].Split(',');
            room.Size = new Vector2(int.Parse(size[0]), int.Parse(size[1]));
            var actorCount = int.Parse(lines[3]);
            for (int i = 0; i < actorCount; i++)
            {
                var actor = lines[4 + i].Split('|');
                var type = Type.GetType("Slicer.Core." + actor[0]);
                var instance = (Actor)Activator.CreateInstance(type);
                var position = actor[1].Split(',');
                instance.Position = new Vector2(int.Parse(position[0]), int.Parse(position[1]));
                room.AddActor(instance);
            }
            var solidCount = int.Parse(lines[4 + actorCount]);
            for (int i = 0; i < solidCount; i++)
            {
                var solid = lines[5 + actorCount + i].Split('|');
                var type = Type.GetType("Slicer.Core." + solid[0]);
                var instance = (Solid)Activator.CreateInstance(type);
                var position = solid[1].Split(',');
                instance.Position = new Vector2(int.Parse(position[0]), int.Parse(position[1]));
                room.Solids.Add(instance);
            }
            Console.WriteLine(room.BackgroundColor);
            return room;
        }
    }
}
