using System.Text.Json;
using System.Text;
using OzonContest;

public class Program
{

    public static void Main(string[] args)
    {
        //Указываем путь к папке с тестами и главный метод
        var validator = new TaskValidator(@"C:\Users\infsec\Desktop\task3", GetResult);
        var testResults = validator.StartValidation();

        foreach (var result in testResults)
            Console.WriteLine(result);
    }

    //Ваш метод, соответствующий делегату (на вход массив строк, на выход - массив строк)
    public static string[] GetResult(string[] input)
    {
        var result = new List<string>();
        int n = 0;
        for (int i = 1; i < input.Length; i += n + 1)
        {
            n = int.Parse(input[i]);

            var jsonBuilder = new StringBuilder();

            for (int j = i + 1; j < n + i + 1; j++)
            {
                jsonBuilder.Append(input[j]);
            }

            var catalog = JsonSerializer.Deserialize<Catalog>(jsonBuilder.ToString(), new JsonSerializerOptions { MaxDepth = 2000000 });
            var virusCount = GetVirusisCount(catalog, false);

            result.Add(virusCount.ToString());
        }

        return result.ToArray();
    }

    private static int GetVirusisCount(Catalog catalog, bool isHacked)
    {
        var count = 0;

        if (isHacked)
        {
            if (catalog != null && catalog.files != null)
                count += catalog.files.Count;
        }
        else

        {

            if (catalog != null)
            {

                if (catalog.files != null && catalog.files.Any())
                    foreach (var file in catalog.files)
                    {

                        if (file != null)
                        {

                            if (file.EndsWith(".hack"))
                            {
                                if (catalog != null && catalog.files != null)
                                {
                                    count += catalog.files.Count;

                                    isHacked = true;
                                    break;
                                }
                            }
                        }

                    }
            }

        }

        if (catalog != null && catalog.folders != null)
        {

            foreach (var folder in catalog.folders)
            {
                count += GetVirusisCount(folder, isHacked);
            }

        }

        return count;
    }
}

public class Catalog
{
    public string dir { get; set; }
    public List<string> files { get; set; }
    public List<Catalog> folders { get; set; }
}