using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Firebird.Checkers
{
    public class FirebirdDatabaseServiceViewChecker : FirebirdDatabaseServiceCheckerBase, IDatabaseServiceViewChecker
    {
        public FirebirdDatabaseServiceViewChecker(IDatabaseService databaseService)
            : base(databaseService)
        {
        }

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[3];

            restrictions[2] = name;

            return RetrieveSchemaInformationAndReturnTrueIfRowExists(@"Views", restrictions);

        }
    }
}