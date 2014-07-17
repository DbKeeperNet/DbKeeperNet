namespace DbKeeperNet.Engine
{
    /// <summary>
    /// An interface marking up a database upgrade step
    /// as accepting a visitor
    /// </summary>
    public interface IVisitableUpdateStep
    {
        /// <summary>
        /// Accept the visitor
        /// </summary>
        /// <param name="visitor">Visitor instance</param>
        void Accept(IUpdateStepVisitor visitor);
    }
}