using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.tracker
{
    public enum ChangeType
    {
        modified,
        added,
        removed
    }
    public abstract class Tracker
    {
        protected string _folderToTrack;
        public abstract Dictionary<string, ChangeType> GetChanges();

        public Tracker(string folderToTrack)
        {
            this._folderToTrack = folderToTrack;
        }
    }
}
