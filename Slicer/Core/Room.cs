using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Slicer.Core
{
    internal class Room
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Room size in terms of tiles. So a 20x10 room is 20*16 x 10*16 in size;
        /// </summary>
        public Vector2 Size { get; set; }
       

        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<Solid> Solids { get; set; } = new List<Solid>();

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
    }
}
