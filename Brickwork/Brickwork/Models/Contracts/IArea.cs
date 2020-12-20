namespace Brickwork.Models.Contracts
{
    //Иnterface that allows you to work with any area 
    public interface IArea
    {
        //Determine the size of the area
        void InitializeArea(int[] input);

        //Insert values of that area
        void AddLines();

        //Create second layer of brickwork
        void CreateSecondLayer();

        //Print second layer
        string Print();
    }
}
