using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Core
{
    internal class Actor
    {
        public Vector2 Position;
        public Vector2 Size;
        private float xRemainder = 0;
        private float yRemainder = 0;
        private List<Solid> solids;
        
        public void MoveX(float amount, Action onCollide)
        {
            xRemainder += amount;
            int move = (int)Math.Round(xRemainder);
            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);
                while (move != 0)
                {
                    if (!collideAt(solids, Position + new Vector2(sign, 0)))
                    {
                        //There is no Solid immediately beside us 
                        Position.X += sign;
                        move -= sign;
                    }
                    else
                    {
                        //Hit a solid!
                        if (onCollide != null)
                            onCollide();
                        break;
                    }
                }
            }
        }

        public void MoveY(float amount, Action onCollide)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder);
            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign(move);
                while (move != 0)
                {
                    if (!collideAt(solids, Position + new Vector2(0, sign)))
                    {
                        //There is no Solid immediately beside us 
                        Position.Y += sign;
                        move -= sign;
                    }
                    else
                    {
                        //Hit a solid!
                        if (onCollide != null)
                            onCollide();
                        break;
                    }
                }
            }
        }

        private bool collideAt(List<Solid> solids, Vector2 position)
        {
            foreach (Solid solid in solids)
            {
                if (solid.Collide(position, Size))
                    return true;
            }
            return false;
        }
       
    }
}
