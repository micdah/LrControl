using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Serilog;

namespace LrControl.Core.Util
{
    internal static class Serializer
    {
        private static readonly ILogger Log = Serilog.Log.ForContext(typeof(Serializer));

        public static void Save<T>(string relativeFilename, T instance) where T : class
        {
            Log.Debug("Saving instance of {Name} to relative file {Filename}", typeof(T).Name, relativeFilename);

            var path = ResolveRelativeFilename(relativeFilename);
            EnsurePathExists(path);

            try
            {
                var serializer = new XmlSerializer(typeof (T));
                using (var writer = new StreamWriter(path, false))
                {
                    serializer.Serialize(writer, instance);
                }

                Log.Debug("Successfully saved instance of {Name} to '{Path}'", typeof(T).Name, path);
            }
            catch (Exception e)
            {
                Log.Error(e, "Was unable to save instance of {Name} to '{Path}'", typeof(T).Name, path);
            }
        }

        private static void EnsurePathExists(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir != null)
            {
                Log.Debug("Directory {Directory} doesn't exist, creating it", dir);
                Directory.CreateDirectory(dir);
            }
        }

        public static bool Load<T>(string relativeFilename, out T instance) where T : class
        {
            Log.Debug("Loading instance of {Name} from relative file {Filename}", typeof(T).Name, relativeFilename);

            var path = ResolveRelativeFilename(relativeFilename);
            if (File.Exists(path))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    using (var reader = new StreamReader(path))
                    {
                        instance = serializer.Deserialize(reader) as T;
                        return instance != null;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "Was unable to load instance of {Name} from '{path}'", typeof(T).Name, path);
                    instance = null;
                    return false;
                }
            }
            
            Log.Warning("Cannot load instance of {Name} from '{Path}', file does not exist", typeof(T).Name, path);
            instance = null;
            return false;
        }

        public static string ResolveRelativeFilename(string relativeFilename)
        {
            var exePath = Assembly.GetExecutingAssembly().Location;
            var exeDir = Path.GetDirectoryName(exePath);
            var absoluteFilename = exeDir != null
                ? Path.GetFullPath(Path.Combine(exeDir, relativeFilename))
                : Path.GetFullPath(relativeFilename);

            Log.Debug("Resolved relative path {RelativePath} to absolute path {AbsolutePath}", relativeFilename,
                absoluteFilename);

            return absoluteFilename;
        }
    }
}