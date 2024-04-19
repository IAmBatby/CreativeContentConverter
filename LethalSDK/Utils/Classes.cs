using System;

namespace LethalSDK.Utils
{
    [Serializable]
    public class SerializableVersion
    {
        public int Major = 1;
        public int Minor = 0;
        public int Build = 0;
        public int Revision = 0;

        public SerializableVersion(int major, int minor, int build, int revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }
        public Version ToVersion()
        {
            return new Version(Major, Minor, Build, Revision);
        }
        public override String ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);
        }
    }
}
