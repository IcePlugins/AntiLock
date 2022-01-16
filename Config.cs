using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AntiLock
{
    public class LockGroup
    {
        [XmlAttribute]
        public string Permission { get; set; }
        [XmlAttribute]
        public int MaxLocks { get; set; }
    }
    public class Config : IRocketPluginConfiguration
    {
        public int DefaultMaxLocks = 0;
        public bool 
            DisplayLoadMessage = true,
            IgnoreAdmins = true,
            DisplayMaxlocksNotice = true,
            DisplayLockNotice = true;
        public string
            AllowOtherPermission = "antilock.clearlocks.other";
        public List<LockGroup> LockGroups = new List<LockGroup>();

        public void LoadDefaults()
        {
            LockGroups = new List<LockGroup>
            {
                new LockGroup
                {
                    MaxLocks = 5,
                    Permission = "antilock.lock"
                },
                new LockGroup
                {
                    MaxLocks = 10,
                    Permission = "antilock.biglock"
                }
            };
        }
    }
}
