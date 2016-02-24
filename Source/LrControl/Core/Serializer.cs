using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using log4net;

namespace micdah.LrControl.Core
{
    public class Serializer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Serializer));

        public static bool Save<T>(string relativeFilename, T instance) where T : class
        {
            var path = ResolveRelativeFilename(relativeFilename);

            try
            {
                var serializer = new XmlSerializer(typeof (T));
                using (var writer = new StreamWriter(path, false))
                {
                    serializer.Serialize(writer, instance);
                }

                Log.Debug($"Successfully saved instance of {typeof (T).Name} to '{path}'");
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Was unable to save instance of {typeof (T).Name} to '{path}'", e);
                return false;
            }
        }

        public static bool Load<T>(string relativeFilename, out T instance) where T : class
        {
            var path = ResolveRelativeFilename(relativeFilename);
            if (File.Exists(path))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof (T));
                    using (var reader = new StreamReader(path))
                    {
                        instance = serializer.Deserialize(reader) as T;
                        return instance != null;
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Was unable to load instance of {typeof (T).Name} from '{path}'", e);
                    instance = null;
                    return false;
                }
            }

            Log.Debug($"Cannot load instance of {typeof (T).Name} from '{path}', file does not exist");
            instance = null;
            return false;
        }

        private static string ResolveRelativeFilename(string relativeFilename)
        {
            var exePath = Assembly.GetExecutingAssembly().Location;
            var exeDir = Path.GetDirectoryName(exePath);
            return exeDir != null
                ? Path.Combine(exeDir, relativeFilename)
                : Path.GetFullPath(relativeFilename);
        }
    }
}