using DIDAWorker;

namespace ExtendedMetarecord
{
    class ExtendedMetarecord : DIDAMetaRecord
    {
        int versionNumber;

        public ExtendedMetarecord(int versionNumber)
        {
            this.versionNumber = versionNumber;
        }
    }
}