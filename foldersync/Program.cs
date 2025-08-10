using Microsoft.Extensions.Logging;
using foldersync.config;
class Program
{
    static void Main()
    {
        CConfig config1 = new CConsoleConfig();
        //var folderTracker = new foldersync.tracker.ShaTracker(config1.sourcePath);
        var folderTracker = new foldersync.tracker.FolderTracker(config1.sourcePath);
        var fileTracker = new foldersync.tracker.ShaTracker(config1.sourcePath);
        var syncer = new foldersync.sync.Synchronizer(folderTracker, fileTracker, config1.sourcePath, config1.replicaPath);
        while (true)
        {
            syncer.sync();
            Thread.Sleep(config1.syncInterval);
        }
    }
}


/*
ITracker -- interface for tracking changes
getChanges() -- returns a list of changes -- data type not yet decided on
 
CShaTracker -- tracking changes using SHA hash
on constructor, take config and make map of all the hashes
implement getChanges()


CFolderSync 


*/