using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.tracker
{ 
    /// <summary>
    /// Uses SHA256 hashes of files in a directory to track changes
    /// </summary>
    public class ShaTracker : Tracker
    {
        private Dictionary<string, byte[]> hashedFile = new Dictionary<string, byte[]>();
        public ShaTracker(string folderToTrack) : base(folderToTrack) 
        { 
            UpdateFileDictionary();
        }

        private void UpdateFileDictionary()
        {
            hashedFile = Directory
                .EnumerateFiles(folderToTrack, "*", SearchOption.AllDirectories)
                .ToDictionary(file => Path.GetRelativePath(folderToTrack, file), file => CryptoWrappers.ComputeHash(file));
        }
        /// <summary>
        /// Returns a dictionary of changes in the directory tracked
        /// </summary>
        /// <returns>Dictionary where the key is relative path file and value the type of change</returns>
        public override Dictionary<string, changeType> GetChanges()
        {
            // Assume all files removed initially 
            var listOfChanges = hashedFile.ToDictionary(KeyValuePair => KeyValuePair.Key, KeyValuePair => changeType.removed);
            var updatedFileDictionary = new Dictionary<string, byte[]>();

            foreach (string currentFileAbsolute in Directory.EnumerateFiles(folderToTrack, "*", SearchOption.AllDirectories))
            {
                var currentFileHash = CryptoWrappers.ComputeHash(currentFileAbsolute);
                var currentFileRelative = Path.GetRelativePath(folderToTrack, currentFileAbsolute);

                if (!hashedFile.ContainsKey(currentFileRelative)) listOfChanges[currentFileRelative] = changeType.added;
                else if (!currentFileHash.SequenceEqual(hashedFile[currentFileRelative])) listOfChanges[currentFileRelative] = changeType.modified;
                else listOfChanges.Remove(currentFileRelative);

                updatedFileDictionary[currentFileRelative] = currentFileHash;
            }

            hashedFile = updatedFileDictionary;

            return listOfChanges;
        }
    }
}
