namespace Brickwork.IO.Contracts
{
    //Interface to read text from console
    //I made it like that just because it's not good idea classes to know about console
    //Single responsibility principle
    public interface IReader
    {
        //Method to read text from console
        string ReadLine();
    }
}
