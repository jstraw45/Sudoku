/************************************************************************************
 * Classification of type of Move
 * Jeff Straw | Northwestern Michigan College
 * 03/10/2015: Initial release
 ***********************************************************************************/
namespace Utility
{
    /// <summary>
    /// Classification of type of Move
    /// </summary>
    public enum MoveType
    {
        /// <summary>
        /// This Move solves a Cell
        /// </summary>
        Solution,

        /// <summary>
        /// This Move disqualifies a potential solution for a Cell
        /// </summary>
        Disqualification
    }
}