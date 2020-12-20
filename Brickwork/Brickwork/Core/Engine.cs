namespace Brickwork.Core
{
    using System;

    using Brickwork.IO.Contracts;
    using Brickwork.Core.Contracts;
    using Brickwork.Models.Contracts;
    using System.Linq;

    //Implementation of IEngine interface
    public class Engine : IEngine
    {
        //Dependency inversion principle
        //Define interfaces to work with them not with concrete classes
        private readonly IReader _reader;
        private readonly IWriter _writer;
        private readonly IArea _brickwork;

        //Dependency inversion principle
        //Working with abstractions not with concrete classes
        public Engine(IReader reader, IWriter writer, IArea brickwork)
        {
            this._reader = reader;
            this._writer = writer;
            this._brickwork = brickwork;
        }

        //Implementation of Run method
        public void Run()
        {
            //Use try catch block to handle the exceptions
            try
            {
                //If there is no exceptions this code will be executed
                var input = this._reader.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                //Call public methods of Area class
                this._brickwork.InitializeArea(input);
                this._brickwork.AddLines();
                this._brickwork.CreateSecondLayer();
                this._writer.WriteLine(this._brickwork.Print());
            }
            catch (Exception ex)
            {
                //Otherwise throw exception message
                this._writer.WriteLine(ex.Message);
            }
        }
    }
}
