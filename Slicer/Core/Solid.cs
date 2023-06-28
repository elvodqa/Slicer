using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Slicer.Core
{
    internal class Solid
    {
        public Vector2 Position;
        public Vector2 Size;
        private float xRemainder = 0;
        private float yRemainder = 0;
        
        public bool Collide(Vector2 position, Vector2 size)
        {
            return position.X < Position.X + Size.X &&
                position.X + size.X > Position.X &&
                position.Y < Position.Y + Size.Y &&
                position.Y + size.Y > Position.Y;
        }

        /*
        public void Move(float x, float y)
        {
            xRemainder += x;
            yRemainder += y;
            int moveX = (int)Math.Round(xRemainder);
            int moveY = (int)Math.Round(yRemainder);
            if (moveX != 0 || moveY != 0)
            {
                //Loop through every Actor in the Level, add it to 
                //a list if actor.IsRiding(this) is true 
                List<Actor> riding = GetAllRidingActors();
                //Make this Solid non-collidable for Actors, 
                //so that Actors moved by it do not get stuck on it 
                Collidable = false;
                if (moveX != 0)
                {
                    xRemainder -= moveX;
                    Position.X += moveX;
                    if (moveX > 0)
                    {
                        foreach (Actor actor in Level.AllActors)
                        {
                            if (overlapCheck(actor))
                            {
                                //Push right 
                                actor.MoveX(this.Right — actor.Left, actor.Squish);
                            }
                            else if (riding.Contains(actor))
                            {
                                //Carry right 
                                actor.MoveX(moveX, null);
                            }
                        }
                    }
                    else
                    {
                        foreach (Actor actor in Level.AllActors)
                        {
                            if (overlapCheck(actor))
                            {
                                //Push left 
                                actor.MoveX(this.Left — actor.Right, actor.Squish);
                            }
                            else if (riding.Contains(actor))
                            {
                                //Carry left 
                                actor.MoveX(moveX, null);
                            }
                        }
                    }
                }
                if (moveY != 0)
                { 
                  //Do y-axis movement 
                }
                //Re-enable collisions for this Solid 
                Collidable = true;
            }
        } */
    }
}
