using CsvHelper;
using CsvHelper.Configuration;
using IMDbCrawler.Models;
using System.Globalization;

namespace IMDbCrawler.Utils;

public static class CsvExporter
{
    public static void ExportToFile(IEnumerable<Movie> movies, string fileName)
    {
        // Obtém o caminho do diretório Downloads do usuário
        string downloadsFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );

        // Garante que o diretório existe
        if (!Directory.Exists(downloadsFolder))
        {
            Directory.CreateDirectory(downloadsFolder);
        }

        // Cria o caminho completo do arquivo
        string filePath = Path.Combine(downloadsFolder, fileName);

        // Escreve o CSV no diretório de Downloads
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        csv.Context.RegisterClassMap<MovieMap>();
        csv.WriteRecords(movies);
    }

    private sealed class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.Year).Name("Year");
            Map(m => m.Director).Name("Director");
            Map(m => m.Rating).Name("Rating").TypeConverterOption.Format("0.0");
            Map(m => m.NumberOfReviews).Name("Reviews");
        }
    }
}
