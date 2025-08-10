using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.tracker
{
    public enum changeType
    {
        modified,
        added,
        removed
    }
    public abstract class Tracker
    {
        protected string folderToTrack;
        public abstract Dictionary<string, changeType> GetChanges();

        public Tracker(string folderToTrack)
        {
            this.folderToTrack = folderToTrack;
        }
    }
}
