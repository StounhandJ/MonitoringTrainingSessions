using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace MonitoringTrainingSessions.Lib;

static class ManagmentSave
{
    private static readonly string savePath = Path.GetTempPath() + "\\MonitoringTrainingSessions.stoun";

    private static readonly string key = "F";

    private static readonly string filename = "secret.bin";

    private static void write(string path, string data)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var demoFile = archive.CreateEntry(filename);

                using (var entryStream = demoFile.Open())
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write(data);
                }
            }

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
            }
        }
    }

    private static string? read(string path)
    {
        if (File.Exists(path))
        {
            using (ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name == filename)
                    {
                        using (StreamReader fs = new StreamReader(entry.Open()))
                        {
                            return fs.ReadToEnd();
                        }
                    }
                }
        }

        return null;
    }

    public static async Task saveId(int id)
    {
        write(savePath, Encryption.encoding(
            id.ToString(),
            ManagmentSave.key));
    }

    public static int loadId()
    {
        string? text = read(savePath);
        if (text != null)
        {
            string json = Encryption.dencoding(text, ManagmentSave.key);
            int id;
            if (!int.TryParse(json, out id))
            {
                return -1;
            }

            return id;
        }

        return -1;
    }
}