using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Core.Solids
{
    internal class Tile : Solid
    {
        public Texture2D Texture { get; set; }
        public string TilesetName;
        public Vector2 Tilepos;
    }
}
