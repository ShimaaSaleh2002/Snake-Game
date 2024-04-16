using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    //we don't need to create an instance of static vars 
    //once they're called from the main class they're accessable throughout z program
    internal class Settings
    {

        public static int Width { get; set; }//for each item
        public static int Height { get; set; }
        public static string directions;
        public Settings()
        {
            Width = 16;
            Height = 16;
            directions = "left";
        }
    }
}
