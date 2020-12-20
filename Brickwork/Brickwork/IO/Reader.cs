namespace Brickwork.IO
{
    using System;

    using Brickwork.IO.Contracts;

    //Implementation of IReader interface
    public class Reader : IReader
    {
        public string ReadLine() => Console.ReadLine();
    }
}
