namespace Brickwork.IO.Contracts
{
    //Interface to write text on console
    //I made it like that just because it's not good idea classes to know about console
    //Single responsibility principle
    public interface IWriter
    {
        //Write text on console
        void Write(string content);

        //Write text on console than add new line
        void WriteLine(string content);
    }
}
