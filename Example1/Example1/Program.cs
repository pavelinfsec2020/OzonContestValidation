using OzonContest;

public class Program
{

    public static void Main(string[] args)
    {
        //Указываем путь к папке с тестами и главный метод
        var validator = new TaskValidator(@"C:\Users\infsec\Desktop\task1", GetResult);
        var testResults = validator.StartValidation();

        foreach (var result in testResults)
            Console.WriteLine(result);
    }

    //Ваш метод, соответствующий делегату (на вход массив строк, на выход - массив строк)
    public static string[] GetResult(string[] input)
    {
        var result = new List<string>();
        for (int i = 1; i < input.Length; i++)
        {
            result.Add(GetMaxSalary(input[i]));
        }

        return result.ToArray();
    }

    private static string GetMaxSalary(string input)
    {
        if (input.Length == 1)
            return "0";

        for (int i = 0; i < input.Length - 1; i++)
        {
            if (int.Parse(input[i].ToString()) < int.Parse(input[i + 1].ToString()))
            {
                return input.Remove(i, 1);
            }
        }

        return input.Remove(input.Length - 1, 1);
    }
}
