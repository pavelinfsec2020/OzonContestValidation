using System.Diagnostics;
using System.Text;
using OzonContest;

public class Program
{

    public static void Main(string[] args)
    {
        //Указываем путь к папке с тестами и главный метод
        var validator = new TaskValidator(@"C:\Users\infsec\Desktop\task4", GetResult);
        var testResults = validator.StartValidation();

        foreach (var result in testResults)
            Console.WriteLine(result);
    }

    //Ваш метод, соответствующий делегату (на вход массив строк, на выход - массив строк)
    public static string[] GetResult(string[] input)
    {
        var resultOut = new List<string>();
        int n = 0;

        for (int i = 1; i < input.Length; i += n)
        {
            var productsCount = int.Parse(input[i]);
            var products = input[i + 1].Split(' ');
            var productsArray = new Product[productsCount];

            for (int j = 0; j < productsCount; j++)
            {
                productsArray[j] = new Product() { Arrive = int.Parse(products[j]), Index = j };
            }

            var trucksCount = int.Parse(input[i + 2]);
            var trucks = new Truck[trucksCount];

            for (int j = i + 3; j < trucksCount + i + 3; j++)
            {
                var infoTruck = input[j].Split(' ');
                trucks[j - (i + 3)] = new Truck()
                {
                    Index = j + 1 - (i + 3),
                    Start = int.Parse(infoTruck[0]),
                    End = int.Parse(infoTruck[1]),
                    Capacity = int.Parse(infoTruck[2])
                };

            }

            var result = GetTruckIndexes(productsCount, productsArray, trucksCount, trucks);
            var builder = new StringBuilder();
          

            for (int j = 0; j < productsCount; j++)
            {
                builder.Append(result[j]);
                    builder.Append(' ');
            }

            resultOut.Add(builder.ToString());

            n = trucksCount + 3;
        }

        return resultOut.ToArray();
    }

    private static int[] GetTruckIndexes(int productsCount, Product[] products, int trucksCount, Truck[] trucks)
    {
        var result = new int[productsCount];
        var dictionary = new Dictionary<Product, Truck>();
        // Array.Sort(products);
        products = products.OrderBy(x => x.Arrive).ToArray();
        trucks = trucks.OrderBy(t => t.Start).ToArray();
        var productIndex = 0;

        for (int i = 0; i < trucks.Length; i++)
        {
            if (productIndex >= products.Length)
                break;



            while (productIndex < products.Length && products[productIndex].Arrive < trucks[i].Start)
            {
                result[products[productIndex].Index] = -1;
                productIndex++;
            }

            while (productIndex < products.Length && products[productIndex].Arrive <= trucks[i].End && trucks[i].Capacity > 0)
            {
                dictionary.Add(products[productIndex], trucks[i]);
                trucks[i].Capacity--;
                productIndex++;
            }

        }

        for (int i = 0; i < productsCount; i++)
        {
            Truck truck;

            if (dictionary.TryGetValue(products[i], out truck))
                result[products[i].Index] = truck.Index;
            else result[products[i].Index] = -1;
        }
        return result;
    }
}

public class Truck
{
    public int Index { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Capacity { get; set; }
}

public class Product
{
    public int Arrive { get; set; }
    public int Index { get; set; }

}