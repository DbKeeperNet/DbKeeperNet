using System;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.Firebird.Checkers
{
    public class FirebirdDatabaseServiceTableChecker : FirebirdDatabaseServiceCheckerBase, IDatabaseServiceTableChecker
    {
        public FirebirdDatabaseServiceTableChecker(IDatabaseService databaseService)
            : base(databaseService)
        {
        }

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string[] restrictions = new string[4];

            restrictions[2] = name;
            restrictions[3] = "TABLE";

            return RetrieveSchemaInformationAndReturnTrueIfRowExists(@"Tables", restrictions);
        }
    }
}