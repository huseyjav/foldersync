using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldersync.config
{
    public class ConsoleConfig : Config
    {
        public ConsoleConfig()
        {
            Console.WriteLine("Source path:");
            _sourcePath = Console.ReadLine();
            Console.WriteLine("Replica path:");
            _replicaPath = Console.ReadLine();
            Console.WriteLine("Sync Interval (in ms):");
            _syncInterval = int.Parse(Console.ReadLine());
            Console.WriteLine("Log file path:");
            _logFile = Console.ReadLine();
        }
    }
}
