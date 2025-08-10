using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.tracker
{
    public class FolderTracker : Tracker
    {
        List<string> foldersRelative;
        public FolderTracker(string folderToTrack) : base(folderToTrack)
        {
            updateFoldersList();
        }

        private void updateFoldersList()
        {
            foldersRelative = Directory.EnumerateDirectories(folderToTrack, "*", SearchOption.AllDirectories)
                .Select(folderAbsolute => Path.GetRelativePath(folderToTrack, folderAbsolute))
                .ToList();
        }

        public override Dictionary<string, changeType> GetChanges()
        {
            // Assume all folders removed initially 
            var listOfChanges = foldersRelative.ToDictionary(folderRelative => folderRelative, KeyValuePair => changeType.removed);

            foreach (string currentFolderAbsolute in Directory.EnumerateDirectories(folderToTrack, "*", SearchOption.AllDirectories))
            {
                var currentFolderRelative = Path.GetRelativePath(folderToTrack, currentFolderAbsolute);

                if (!foldersRelative.Contains(currentFolderRelative)) listOfChanges[currentFolderRelative] = changeType.added;
                else listOfChanges.Remove(currentFolderRelative);

            }
            updateFoldersList();

            return listOfChanges;
        }
    }
}
