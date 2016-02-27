using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroProcessor34
{
    static class Program
    {
        #region Поля для скрытия/открытия консоли

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            if (args.Length == 0)
            {
                ShowWindow(handle, SW_HIDE);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
                try
                {
                    ConsoleProgram program = new ConsoleProgram(args);
                    Console.WriteLine(program.getProgGuide());
                    string ch = "";
                    while ((ch = Console.ReadLine().ToUpper().Trim()) != "0")
                    {
                        switch (ch)
                        {
                            case "1":
                                if (!program.isEnd)
                                {
                                    if (program.firstEnd == true)
                                    {
                                        Console.WriteLine("\n Первый проход уже выполнен, перезапустите программу.");
                                        break;
                                    }
                                    program.step = true;
                                    Console.WriteLine("\nОчередной шаг выполнен!\n");
                                    program.nextStep();
                                }
                                else
                                {
                                    Console.WriteLine("\nПрограмма завершила свои действия. Запустите ее заново.\n");
                                }
                                break;
                            case "2":
                                if (!program.isEnd)
                                {
                                    program.firstRun();
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.WriteLine("\nПрограмма завершила свои действия. Запустите ее заново.\n");
                                }
                                break;
                            case "3":
                                Console.WriteLine("\nИсходный код\n");
                                foreach (string str in program.sourceStrings)
                                {
                                    Console.WriteLine(str);
                                }
                                Console.WriteLine();
                                break;
                            case "4":
                                Console.WriteLine("\nАссемблерный код\n");
                                program.sourceCode.printAsm();
                                Console.WriteLine();
                                break;
                            case "5":
                                Console.WriteLine("\nТаблица глобальных переменных\n");
                                Global.printGlobal();
                                Console.WriteLine();
                                break;
                            case "6":
                                Console.WriteLine("\nТМО\n");
                                TMO.printTMO();
                                Console.WriteLine();
                                break;
                            case "8":
                                Console.WriteLine("\nОбновлено все\n");
                                TMO.refresh();
                                Global.refresh();
                                program = new ConsoleProgram(args);
                                program.sourceCode.result = new List<SourceEntity>();
                                Console.WriteLine();
                                break;
                            case "7":
                                try
                                {
                                    StreamWriter sw = new StreamWriter(program.output_file);
                                    foreach (SourceEntity se in program.sourceCode.result)
                                    {
                                        sw.WriteLine(se.ToString());
                                    }
                                    sw.Close();
                                    Console.WriteLine("\nЗапись успешна\n");
                                    Process.Start("notepad.exe", program.output_file);
                                }
                                catch
                                {
                                    Console.WriteLine("\nЗапись не успешна, возможно не задан или не найден файл\n");
                                }
                                break;
                            default:
                                Console.WriteLine("\nОшибка! Введен неверный ключ!\n");
                                break;
                        }
                        Console.WriteLine(program.getProgGuide());
                    }
                }
                catch (ConsoleException ex)
                {
                    Console.WriteLine("\n\nОшибка " + ex.Message + "\n\n");
                    Console.WriteLine(ConsoleProgram.getUserGuide());
                    Console.WriteLine("\nПрограмма завершила свои действия. Запустите ее заново.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\nОшибка" + ex.Message + "\n\n");
                    Console.WriteLine(ConsoleProgram.getUserGuide());
                    Console.WriteLine("\nПрограмма завершила свои действия. Запустите ее заново.\n");
                }
            }
        }
    }
}
