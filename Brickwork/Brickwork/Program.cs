namespace Brickwork
{
    using Brickwork.Core;
    using Brickwork.Core.Contracts;
    using Brickwork.IO;
    using Brickwork.IO.Contracts;
    using Brickwork.Models;
    using Brickwork.Models.Contracts;

    public class Program
    {
        public static void Main()
        {
            //Initialize concrete  object
            //Dependency inversion principle
            IReader reader = new Reader();
            IWriter writer = new Writer();
            IArea brickwork = new Area(reader);

            //Initialize object from Engine class and call Run method to start the console app
            IEngine engine = new Engine(reader, writer, brickwork);
            engine.Run();
        }
    }
}
