using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace decode_time
{
    class Program
    {
        static bool isBinary(string path)
        {
            var bytes = File.ReadAllBytes(path);
            foreach (var b in bytes)
                if (b == 0)
                    return true;
            return false;
        }

        static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        static string decode(string s)
        {
            var r = new Regex(@"\A\s*<time>(\d+)</time>\s*\z");
            var m = r.Match(s);
            if (!m.Success)
                return s;
            return FromUnixTime(long.Parse(m.Groups[1].ToString())).ToString("dddd, d MMMM yyyy HH:mm:ss");
        }

        static void Main(string[] args)
        {
            foreach (var arg in args)
                foreach (var path in Glob.Glob.Expand(arg))
                {
                    if (isBinary(path.ToString()))
                    {
                        Console.WriteLine("{0}: binary file", path);
                        continue;
                    }

                    var contents = File.ReadAllLines(path.ToString());
                    var changed = false;

                    for (int i = 0; i < contents.Length; i++)
                    {
                        var s = decode(contents[i]);
                        if (s != contents[i])
                        {
                            contents[i] = s;
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        Console.WriteLine(path);
                        File.WriteAllLines(path.ToString(), contents);
                    }
                }
        }
    }
}
