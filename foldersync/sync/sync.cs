using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.sync
{
    public class Synchronizer
    {
        private tracker.Tracker folderTracker;
        private tracker.Tracker fileTracker;
        private string sourceFolder;
        private string destinationFolder;
        public Synchronizer(tracker.Tracker folderTracker, tracker.Tracker fileTracker, string sourceFolder, string destinationFolder)
        {
            this.folderTracker = folderTracker;
            this.fileTracker = fileTracker;
            this.sourceFolder = sourceFolder;
            this.destinationFolder = destinationFolder;
            CopyDirectory(sourceFolder, destinationFolder);
        }

        private string getNewAbsolutePath(string oldBasePath, string absolutePath, string newBasePath)
        {
            var currentFolderRelative = Path.GetRelativePath(oldBasePath, absolutePath);
            var currentFolderDestinationAbsolute = Path.GetFullPath(currentFolderRelative, newBasePath);
            return currentFolderDestinationAbsolute;
        }

        private void CopyDirectory(string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            foreach(string currentFolder in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                var currentFolderDestinationAbsolute = getNewAbsolutePath(sourcePath, currentFolder, destinationPath);
                Directory.CreateDirectory(currentFolderDestinationAbsolute);
            }
            foreach (string currentFile in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                var currentFileDestinationAbsolute = getNewAbsolutePath(sourcePath, currentFile, destinationPath);
                File.Copy(currentFile, currentFileDestinationAbsolute, true);
            }
        }

        public void sync()
        {
            var folderChanges = folderTracker.GetChanges();
            var fileChanges = fileTracker.GetChanges();

            foreach (var currentFolderRelative in folderChanges)
            {
                var currentFolderAbsolte = Path.GetFullPath(currentFolderRelative.Key, destinationFolder);
                if (currentFolderRelative.Value == tracker.changeType.removed && Directory.Exists(currentFolderAbsolte))
                {
                    Directory.Delete(currentFolderAbsolte, true);
                }
                else if(currentFolderRelative.Value == tracker.changeType.added)
                {
                    Directory.CreateDirectory(currentFolderAbsolte);
                }
            }

            foreach(var currentFileRelative in fileChanges)
            {
                var currentFileDestinationAbsolute = Path.GetFullPath(currentFileRelative.Key, destinationFolder);
                var currentFileSourceAbsolute = Path.GetFullPath(currentFileRelative.Key, sourceFolder);
                switch (currentFileRelative.Value)
                {
                    case tracker.changeType.removed:
                        File.Delete(currentFileDestinationAbsolute);
                        break;
                    case tracker.changeType.added:
                    case tracker.changeType.modified:
                        File.Copy(currentFileSourceAbsolute, currentFileDestinationAbsolute, true);
                        break;
                }
            }

        }

    }
}
