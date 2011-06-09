namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    public sealed class ForceExecution : IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"ForceExecution"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            return true;
        }

        #endregion
    }
}
