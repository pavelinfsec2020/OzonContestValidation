using OzonContest;

public class Program
{

    public static void Main(string[] args)
    {
        //Указываем путь к папке с тестами и главный метод
        var validator = new TaskValidator(@"C:\Users\infsec\Desktop\task2", GetResult);
        var testResults = validator.StartValidation();

        foreach (var result in testResults)
            Console.WriteLine(result);
    }

    //Ваш метод, соответствующий делегату (на вход массив строк, на выход - массив строк)
    public static string[] GetResult(string[] input)
    {
        var count = int.Parse(input[0]);


        var result = new List<string>();
        for (int i = 1; i < input.Length; i+=3)
        {
            int  n = int.Parse(input[i]);

            var validResult = ValidateArrayString(n, input[i + 1], input[i + 2]) ? "yes" : "no";
            result.Add(validResult);
        }

        return result.ToArray();
    }

    private static bool ValidateArrayString(int n, string inputArray, string resultForValid)
    {
        if (resultForValid[0] == ' ' || resultForValid[resultForValid.Length - 1] == ' ')
            return false;
        var arrayInput = new int[n]; var arrayFromString = inputArray.Split(' ');
        for (int i = 0; i < n; i++)
        {
            if (!int.TryParse(arrayFromString[i], out arrayInput[i]))
                return false;
        }
        var arrayToValid = resultForValid.Split(' ');
        if (n != arrayToValid.Length)
            return false;
        var arrayOutput = new int[n];
        for (int i = 0; i < n; i++)
        {
            if (arrayToValid[i].Length > 0 && arrayToValid[i][0] == '0') return false;
            if (arrayToValid[i].Length > 1)
            {
                if (arrayToValid[i][0] == '-' && arrayToValid[i][1] == '0')
                    return false;
            }
            if (!int.TryParse(arrayToValid[i], out arrayOutput[i]))
                return false;
        }
        Array.Sort(arrayInput);
        for (int i = 0; i < n; i++)
        {
            if (arrayInput[i] != arrayOutput[i])
                return false;
        }
        return true;
    }
}