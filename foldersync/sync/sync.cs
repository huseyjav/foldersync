using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace foldersync.sync
{
    public class Synchronizer
    {
        private tracker.Tracker _folderTracker;
        private tracker.Tracker _fileTracker;
        private string _sourceFolder;
        private string _destinationFolder;
        private ILogger _eventLogger;
        public Synchronizer(tracker.Tracker folderTracker, tracker.Tracker fileTracker, string sourceFolder, string destinationFolder, ILogger eventLogger)
        {
            this._folderTracker = folderTracker;
            this._fileTracker = fileTracker;
            this._sourceFolder = sourceFolder;
            this._destinationFolder = destinationFolder;
            this._eventLogger = eventLogger;
            CopyDirectory(sourceFolder, destinationFolder);
        }

        private string GetNewAbsolutePath(string oldBasePath, string absolutePath, string newBasePath)
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
                var currentFolderDestinationAbsolute = GetNewAbsolutePath(sourcePath, currentFolder, destinationPath);
                Directory.CreateDirectory(currentFolderDestinationAbsolute);
            }
            foreach (string currentFile in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                var currentFileDestinationAbsolute = GetNewAbsolutePath(sourcePath, currentFile, destinationPath);
                File.Copy(currentFile, currentFileDestinationAbsolute, true);
                LogEvent(tracker.ChangeType.added, Path.GetRelativePath(sourcePath, currentFile));
            }
        }

        public void Sync()
        {
            SyncFolders();
            SyncFiles();
        }
        private void SyncFolders()
        {
            var folderChanges = _folderTracker.GetChanges();

            foreach (var currentFolderRelative in folderChanges)
            {
                var currentFolderAbsolte = Path.GetFullPath(currentFolderRelative.Key, _destinationFolder);
                if (currentFolderRelative.Value == tracker.ChangeType.removed && Directory.Exists(currentFolderAbsolte))
                {
                    Directory.Delete(currentFolderAbsolte, true);
                }
                else if (currentFolderRelative.Value == tracker.ChangeType.added)
                {
                    Directory.CreateDirectory(currentFolderAbsolte);
                }
            }
        }

        private void SyncFiles()
        {
            var fileChanges = _fileTracker.GetChanges();

            foreach (var currentFileRelative in fileChanges)
            {
                var currentFileDestinationAbsolute = Path.GetFullPath(currentFileRelative.Key, _destinationFolder);
                var currentFileSourceAbsolute = Path.GetFullPath(currentFileRelative.Key, _sourceFolder);
                switch (currentFileRelative.Value)
                {
                    case tracker.ChangeType.removed:
                        if (!File.Exists(currentFileDestinationAbsolute)) break;
                        File.Delete(currentFileDestinationAbsolute);
                        break;
                    case tracker.ChangeType.added:
                    case tracker.ChangeType.modified:
                        File.Copy(currentFileSourceAbsolute, currentFileDestinationAbsolute, true);
                        break;
                }

                LogEvent(currentFileRelative.Value, currentFileRelative.Key);
            }
        }

        private void LogEvent(tracker.ChangeType change, string fileName)
        {
            switch (change)
            {
                case tracker.ChangeType.removed:
                    _eventLogger.LogInformation("file {filepath} removed", fileName);
                    break;
                case tracker.ChangeType.added:
                case tracker.ChangeType.modified:
                    _eventLogger.LogInformation("file {filepath} copied into replica", fileName);
                    break;
            }
        }
        

    }
}
