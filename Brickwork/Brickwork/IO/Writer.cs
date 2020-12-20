namespace Brickwork.IO
{
    using System;

    using Brickwork.IO.Contracts;

    //Implementation of IWriter interface
    public class Writer : IWriter
    {
        public void Write(string content) => Console.Write(content);

        public void WriteLine(string content) => Console.WriteLine(content);
    }
}
