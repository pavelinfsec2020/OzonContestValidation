namespace OzonContest
{
    public class TaskValidator
    {
        private string[] _inputTestFiles;
        private string[] _outputTestFiles;
        private readonly ValidationMethod _validationMethod;
        public TaskValidator(string folderPath, ValidationMethod validationMethod)
        {
            _validationMethod = validationMethod;
            GetTestFilesNames(folderPath);

        }

        public delegate string[] ValidationMethod(string[] inputData);

        private void GetTestFilesNames(string folderPath)
        {
            var allFiles = Path.Exists(folderPath) ?
              Directory.GetFiles(folderPath) : new string[0];

            if (allFiles.Length == 0)
                return;

            _inputTestFiles = allFiles.Where(f => !f.EndsWith(".a")).OrderBy(f => f).ToArray();
            _outputTestFiles = allFiles.Where(f => f.EndsWith(".a")).OrderBy(f => f).ToArray();
        }

        public List<string> StartValidation()
        {
            var testResults = new List<string>();

            if (_inputTestFiles.Length != _outputTestFiles.Length)
                return new List<string>() { "Некорректные тестовые файлы, проверьте содержимое папки с тестами!"};

            for (int i = 0; i < _inputTestFiles.Length; i++)
            {
                var validationResult = ValidTest(_inputTestFiles[i], _outputTestFiles[i], Path.GetFileName(_inputTestFiles[i])).Result;
                testResults.Add(validationResult);
            }

            return testResults;
        }

        private async Task<string> ValidTest(string inputTestFileName, string expectedOutputFileName, string testNumber)
        {
            var inputTestData = new List<string>();
            var expectedOutputData = new List<string>();

            using (var reader = new StreamReader(inputTestFileName))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    inputTestData.Add(line);
                }
            }

            using (var reader = new StreamReader(expectedOutputFileName))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    expectedOutputData.Add(line);
                }
            }

            var actualResult = _validationMethod(inputTestData.ToArray());

            if (actualResult.Length != expectedOutputData.Count)
                return $"В тесте {testNumber} не соответствует количество строк ожидаемого и фактического результатов!";

            for (int i = 0; i < actualResult.Length; i++)
            { 
                if (actualResult[i].Length != expectedOutputData[i].Length)
                    return $"В тесте {testNumber} ошибка в строке {i}. Несоответствие длин строк \n " +
                       $"Ожидаемая строка {expectedOutputData[i]} \n" +
                       $"Фактическая строка {actualResult[i]}";

                for (int j = 0; j < actualResult[i].Length; j++)
                {
                    if (actualResult[i][j] != expectedOutputData[i][j])
                        return $"В тесте {testNumber} ошибка в строке: {i} позиция: {j} \n " +
                            $"Ожидаемая строка {expectedOutputData[i]} \n" +
                            $"Фактическая строка {actualResult[i]}";
                }
            }

            return $"Тест № {testNumber} пройден успешно!";
        }
    }
}
