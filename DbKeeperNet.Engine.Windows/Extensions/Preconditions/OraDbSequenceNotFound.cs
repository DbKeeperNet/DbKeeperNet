using System;
using System.Globalization;
using DbKeeperNet.Engine.Resources;
using DbKeeperNet.Engine.Windows.Extensions.DatabaseServices;

namespace DbKeeperNet.Engine.Windows.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that Oracle sequence with specified
    /// name doesn't exist in database.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>OraDbSequenceNotFound</c>.
    /// This is Oracle specific (see <see cref="OracleDatabaseService"/>).
    /// 
    /// It has one parameter which should contain tested database
    /// sequence name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Sequence SEQ_for_me not found" Precondition="OraDbSequenceNotFound">
    ///   <Param>SEQ_for_me</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class OraDbSequenceNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return "OraDbSequenceNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null) throw new ArgumentNullException(@"context");

            OracleDatabaseService service = context.DatabaseService as OracleDatabaseService;
            
            if (service == null)
                throw new NotSupportedException(String.Format(CultureInfo.CurrentCulture, PreconditionMessages.OnlyOracleSupport, Name));
            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.SequenceNameEmpty, Name));

            return !service.SequenceExists(param[0].Value);
        }

        #endregion
    }
}
