using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.tracker
{
    public class FolderTracker : Tracker
    {
        private List<string> _foldersRelative;
        public FolderTracker(string folderToTrack) : base(folderToTrack)
        {
            UpdateFoldersList();
        }

        private void UpdateFoldersList()
        {
            _foldersRelative = Directory.EnumerateDirectories(_folderToTrack, "*", SearchOption.AllDirectories)
                .Select(folderAbsolute => Path.GetRelativePath(_folderToTrack, folderAbsolute))
                .ToList();
        }
        /// <summary>
        /// Tracks changes done in subdirectories of a directory
        /// </summary>
        /// <returns>Dictionary where key is relative folder path and value whether it was added or removed</returns>
        public override Dictionary<string, ChangeType> GetChanges()
        {
            // Assume all folders removed initially 
            var listOfChanges = _foldersRelative.ToDictionary(folderRelative => folderRelative, KeyValuePair => ChangeType.removed);

            foreach (string currentFolderAbsolute in Directory.EnumerateDirectories(_folderToTrack, "*", SearchOption.AllDirectories))
            {
                var currentFolderRelative = Path.GetRelativePath(_folderToTrack, currentFolderAbsolute);

                if (!_foldersRelative.Contains(currentFolderRelative)) listOfChanges[currentFolderRelative] = ChangeType.added;
                else listOfChanges.Remove(currentFolderRelative);

            }
            UpdateFoldersList();

            return listOfChanges;
        }
    }
}
