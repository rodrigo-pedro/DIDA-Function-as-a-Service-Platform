using System.Collections.Generic;

namespace DIDAWorker
{
    class ExtendedMetarecord : DIDAMetaRecord
    {
        public int VersionNumber;
        public int ReplicaId;
        public List<int> deadStoragesHashes = new List<int>();
    }
}