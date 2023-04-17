using Files.Categorizer.Configuration;

namespace Files.Categorizer;
internal class Program
{
    const string plural = "arquivos";
    const string single = "arquivo";
    static readonly KeyValuePair<string, string[]> empty = new();

    static int success = 0;
    static void Main(string[] args)
    {
        Settings settings = Settings.Instance;

        Console.Write("Digite o diretório de entrada: ");
        string directory = Console.ReadLine() ?? string.Empty;

        while (!Directory.Exists(directory ?? string.Empty))
        {
            Console.Write("Esse diretório não existe! Tente outro: ");
            directory = Console.ReadLine() ?? string.Empty;
        }

        string[] files = Directory.GetFiles(directory);

        Console.WriteLine($"Categorizando {FilesText(files.Length)}.");
        Thread.Sleep(500);

        foreach (string file in files)
        {
            success++;
            string fileName = file.Split('\\').Last();
            string extension = fileName.Split('.').Last();

            if (settings.Excludes.Contains(extension))
            {
                File.Delete(file);
                Console.WriteLine($"O arquivo \"{file.Split('\\').Last()}\" foi excluido.");
                continue;
            }

            var path = settings.Includes.FirstOrDefault(fs => fs.Value.Contains(extension), empty);

            if (path.Equals(empty))
                continue;

            string workStation = Path.Combine(directory, path.Key);

            if (!Directory.Exists(workStation))
                Directory.CreateDirectory(workStation);

            workStation = Path.Combine(workStation, fileName);
            bool exist = File.Exists(workStation);

            if (exist && settings.ExcludeEquals)
            {
                FileInfo infoOriginal = new(workStation);
                FileInfo infoActual = new(file);

                if (infoActual.Length == infoOriginal.Length)
                    infoActual.Delete();

                if (infoActual.Length > infoOriginal.Length)
                {
                    infoOriginal.Delete();
                    infoActual.MoveTo(workStation);
                }

                continue;
            }
            else if (exist)
                continue;

            File.Move(file, workStation);

            Console.WriteLine($"{FilesText(success)} categorizados de {FilesText(files.Length)} totais.");
        }
    }

    private static string FilesText(int quantity)
        => $"{quantity} {(quantity != 1 ? plural : single)}";
}