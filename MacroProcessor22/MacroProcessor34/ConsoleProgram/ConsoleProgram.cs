using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProcessor34
{
    public class ConsoleProgram
    {
        public static string currentDirectory = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
        public string input_file = currentDirectory + "\\source.txt";
        public string output_file = currentDirectory + "\\result.txt";

        public SourceCode sourceCode = null;
        public List<string> sourceStrings = null;
        // номер строки
        public int index = 0;
        // false - 1 проход
        public bool mode = false;

        public bool firstEnd = false;
        public bool secondEnd = false;
        public bool step = false;
        public bool isEnd = false;

        /// <summary>
        /// Конструктор. Считывает исходники с файла
        /// </summary>
        public ConsoleProgram(string[] args)
        {
            #region Разбор аргyментов командной строки

            switch (args.Length)
            {
                case 1:
                    if (args[0].ToUpper() == "-HELP")
                    {
                        Console.WriteLine(ConsoleProgram.getUserGuide());
                    }
                    else
                    {
                        throw new ConsoleException("Некорректное использование аргументов командной строки");
                    }
                    break;
                case 2:
                    if (args[0].ToUpper() == "-INPUT_FILE")
                    {
                        this.input_file = args[1];
                    }
                    else if (args[0].ToUpper() == "-OUTPUT_FILE")
                    {
                        this.output_file = args[1];
                    }
                    else
                    {
                        throw new ConsoleException("Некорректное использование аргументов командной строки");
                    }
                    break;
                case 4:
                    if (args[0].ToUpper() == "-INPUT_FILE")
                    {
                        this.input_file = args[1];
                        if (args[2].ToUpper() == "-OUTPUT_FILE")
                        {
                            this.output_file = args[3];
                        }
                        else
                        {
                            throw new ConsoleException("Недопустимый ключ! Должен быть -OUTPUT_FILE");
                        }
                    }
                    else if (args[0].ToUpper() == "-OUTPUT_FILE")
                    {
                        this.output_file = args[1];
                        if (args[2].ToUpper() == "-INPUT_FILE")
                        {
                            this.input_file = args[3];
                        }
                        else
                        {
                            throw new ConsoleException("Недопустимый ключ! Должен быть -INPUT_FILE");
                        }
                    }
                    else
                    {
                        throw new ConsoleException("Некорректное использование аргументов командной строки");
                    }
                    break;
                default:
                    throw new ConsoleException("Неверное количество аргументов");
            }

            #endregion
            string[] temp = null;
            try
            {
                temp = System.IO.File.ReadAllLines(this.input_file);
            }
            catch (Exception e)
            {
                throw new ConsoleException("Не удалось загрузить данные с файла. Возможно путь к файлу указан неверно");
            }
            this.sourceCode = new SourceCode(temp);
            this.sourceStrings = new List<string>(temp);
        }

        /// <summary>
        /// Следующий шаг выполнения проги
        /// </summary>
        public void nextStep()
        {
            try
            {
                if (!isEnd) this.sourceCode.firstRunStep(this.sourceCode.entities[index++], TMO.root);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                isEnd = true;
            }
            catch (SPException ex)
            {
                isEnd = true;
                Console.WriteLine("\nСтрока \"" + this.sourceCode.entities[index - 1].ToString() + "\": " + ex.Message + "\n");
            }
            if (this.index == this.sourceCode.entities.Count)
            {
                Console.WriteLine("\nПервый проход выполнен\n");
                //this.isEnd = true;
                return;
            }
        }

        /// <summary>
        /// Первый проход
        /// </summary>
        public void firstRun()
        {
            if (step == true)
            {
                Console.WriteLine("\nРаз уж начали выполнять прогу по шагам, а не по частям, так и нечего сюда лезть.");
                return;
            }
            if (firstEnd == true)
            {
                Console.WriteLine("\n1 проход уже завершен.");
                return;
            }
            this.index = 0;
            try
            {
                while (true)
                {
                    this.nextStep();
                    if (this.index == this.sourceCode.entities.Count)
                    {
                        this.firstEnd = true;
                        //isEnd = true;
                        return;
                    }
                    if (isEnd) return;
                }
            }
            catch (SPException ex)
            {
                isEnd = true;
                Console.WriteLine("\nПроизошла ошибка :" + ex.Message);
            }
        }

        /// <summary>
        /// Возвращает строку со справкой по программе
        /// </summary>
        /// <returns></returns>
        public static string getUserGuide()
        {
            return "Справка по данной программе.\r\n  Доступные ключи: [-input_file] [-output_file] [-help]\r\n\t" +
                        "-input_file\tКлюч для указания пути к файлу с исходнм текстом\r\n\t" +
                        "-output_file\tКлюч для указания пути сохранения результирующего ассемблерного кода\r\n\t" +
                        "-help\t\tВызов данной справки.\r\n" +
                        "\r\n\tЕсли путь к файлу имеет пробелы - следует заключить его в кавычки.";
        }

        /// <summary>
        /// Менюшка
        /// </summary>
        public string getProgGuide()
        {
            return "1 - выполнить 1 проход\n" +
                    "2 - выполнить 2 проход\n" +
                    "3 - распечатать Исходный код\n" +
                    "4 - распечатать Ассемблерный код\n" +
                    "5 - распечатать таблицу глобальных переменных\n" +
                    "6 - распечатать ТМО\n" +
                    "7 - распечатать Ассемблерный код в файл\n" +
                    "8 - начать заново\n" +
                    "0 - выход\n";
        }

    }


    public class ConsoleException : Exception
    {
        public ConsoleException(string message)
            : base(message)
        {
        }
    }
}
