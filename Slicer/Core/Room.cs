using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Core
{
    internal class Room
    {
        public string Name { get; set; }
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
