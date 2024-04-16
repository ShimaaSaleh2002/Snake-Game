using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    //it's not gonna draw anything on the screen..it will help us keep
    //track of X and Y value of each circle drawn on z screen.
    internal class Circle
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Circle()
        {
            //so each time creating an instance it needs to know
            //what to do with X & Y.
            X = 0;
            Y = 0;
        }

    }
}
