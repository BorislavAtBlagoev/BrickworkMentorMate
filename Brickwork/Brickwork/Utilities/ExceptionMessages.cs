namespace Brickwork.Utilities
{
    public class ExceptionMessages
    {
        //Constants for exceptions handling
        public const string INVALID_AREA_ROWS_MESSAGE = "Rows cannot be odd, less than 0 or more than 100";
        public const string INVALID_AREA_COLUMNS_MESSAGE = "Columns must be even, more than 0 and less than 100";
        public const string INVALID_SIZE_OF_LINE = "The size of the line must be the same as the size of the columns";
        public const string INVALID_BRICK = "The brick cannot spanning more than 2 rows or columns";
        public const string INVALID_CONSTRUCTED_BRICK = "The brick was incorrect constructed";
    }
}
