using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.config
{

    public abstract class Config
    {
        protected string _sourcePath;
        protected string _replicaPath;
        protected int _syncInterval;
        protected string _logFile;

        public string sourcePath
        {
            get { return _sourcePath; }
        }
        public string replicaPath
        {
            get { return _replicaPath; }
        }

        public int syncInterval
        {
            get { return _syncInterval; }
        }

        public string logFile
        {
            get { return _logFile; }
        }
    }
}
