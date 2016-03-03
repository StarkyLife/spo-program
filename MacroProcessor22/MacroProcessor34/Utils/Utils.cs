using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProcessor34
{
    public static class Utils
    {
        public static string[] symbols = { "#", "$", "%", "!", "@", "^", "&", "*", "-", "[", "\"", "*", "(", ")", "\\", "/", "?", "№", ";", ":", "_", "+", "=", "[", "]", "{", "}", "|", "<", ">", "`", "~", ".", ",", "'", " " };
        public static string[] dirs = { "BYTE", "RESB", "RESW", "WORD" };
        public static string[] keyWords = { "START", "END", "MACRO", "MEND", "WHILE", "ENDW", /*"IF", "ELSE", "ENDIF",*/ "GLOBAL", "SET", "INC", "ADD", "SAVER1", "SAVER2", "LOADR1", "JMP" };
        public static string rus = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        public static string[] signs = { "==", ">=", "<=", "!=", ">", "<" };

        /// <summary>
        /// Проверка на директивы препроцессора
        /// </summary>
        public static bool isDirective(string operation)
        {
            for (int i = 0; i < Utils.dirs.Length; i++)
            {
                if (Utils.dirs[i] == operation)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на метку
        /// </summary>
        public static bool isLabel(string label)
        {
            if (!Utils.isOperation(label))
            {
                return false;
            }
            foreach (string e in Utils.keyWords)
            {
                if (label == e)
                {
                    return false;
                }
            }
            if (!isNotRussian(label))
            {
                return false;
            }
            if (TMO.isInTMO(label))
            {
                return false;
            }
            if (Global.isInGlobal(label))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// проверка на операцию
        /// </summary>
        public static bool isOperation(string label)
        {
            if (String.IsNullOrEmpty(label)) return false;

            for (int i = 0; i < 10; i++)
            {
                if (i.ToString() == label[0].ToString())
                    return false;
            }
            int j = 0;
            while (j < label.Length)
            {
                for (int i = 0; i < Utils.symbols.Length; i++)
                {
                    if (Utils.symbols[i] == label[j].ToString())
                        return false;
                }
                j++;
            }
            if (isRegistr(label)) return false;
            if (isDirective(label)) return false;
            return true;
        }

        /// <summary>
        /// Проверка на регистр
        /// </summary>
        public static bool isRegistr(string reg)
        {
            for (int i = 0; i < 16; i++)
            {
                if ("R" + i.ToString() == reg.Trim().ToUpper())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на присутствие русских символов
        /// </summary>
        public static bool isNotRussian(string word)
        {
            for (int j = 0; j < word.Length; j++)
                for (int i = 0; i < Utils.rus.Length; i++)
                {
                    if (Utils.rus[i].ToString() == word[j].ToString().ToUpper())
                        return false;
                }
            return true;
        }



        public static void parseCondition(string str, out int first, out int second, out string sign)
        {
            string[] arr;
            first = 0;
            second = 0;
            sign = "";
            int temp;
            foreach (string sgn in Utils.signs)
            {
                if ((arr = str.Split(new string[] { sgn }, StringSplitOptions.None)).Length > 1)
                {
                    if (Global.isInGlobal(arr[0]))
                    {
                        if (Global.searchInGlobal(arr[0]).value == null)
                        {
                            throw new SPException("Не инициализированная глобальная переменная '" + arr[0] + "' является частью условия");
                        }
                        else
                        {
                            first = (int)Global.searchInGlobal(arr[0]).value;
                        }
                    }
                    else if (Int32.TryParse(arr[0], out temp) == false)
                    {
                        throw new SPException("Часть условия '" + arr[0] + "' не глобальная переменная и не число");
                    }
                    else
                    {
                        first = Int32.Parse(arr[0]);
                    }

                    if (Global.isInGlobal(arr[1]))
                    {
                        if (Global.searchInGlobal(arr[1]).value == null)
                        {
                            throw new SPException("Не инициализированная глобальная переменная '" + arr[1] + "' является частью условия");
                        }
                        else
                        {
                            second = (int)Global.searchInGlobal(arr[1]).value;
                        }
                    }
                    else if (Int32.TryParse(arr[1], out temp) == false)
                    {
                        throw new SPException("Часть условия '" + arr[1] + "' не глобальная переменная и не число");
                    }
                    else
                    {
                        second = Int32.Parse(arr[1]);
                    }

                    sign = sgn;
                    return;
                }
            }
            throw new SPException("Неопознан знак сравнения");
        }

        /// <summary>
        /// Сравнение
        /// </summary>
        /// <param name="str">строка со сравнением</param>
        /// <returns>результат сравнения</returns>
        public static bool compare(string str)
        {
            int first;
            int second;
            string sign;
            parseCondition(str, out first, out second, out sign);
            switch (sign)
            {
                case ">":
                    return first > second;
                case "<":
                    return first < second;
                case ">=":
                    return first >= second;
                case "<=":
                    return first <= second;
                case "==":
                    return first == second;
                case "!=":
                    return first != second;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Проверка на совпадение имен
        /// </summary>
        /// <param name="name">имя для проверки</param>
        public static void checkNames(string name)
        {
            List<string> list = new List<string>();
            foreach (GlobalEntity glob in Global.entities)
            {
                list.Add(glob.name);
            }
            foreach (TMOEntity te in TMO.entities)
            {
                list.Add(te.name);
                list.AddRange(te.parameters);
            }
            if (list.Contains(name))
            {
                throw new SPException("Имя " + name + " уже используется в качестве глобальной переменной или имени макроса");
            }
        }

        public static SourceEntity print(SourceEntity str)
        {
            SourceEntity newStr = str.Clone();
            for (int j = 0; j < newStr.operands.Count; j++)
            {
                if (Global.isInGlobal(newStr.operands[j]))
                {
                    if (Global.searchInGlobal(newStr.operands[j]).value.HasValue)
                        newStr.operands[j] = Global.searchInGlobal(newStr.operands[j]).value.Value.ToString();
                    else
                        throw new SPException("Глобальная переменная " + newStr.operands[j] + " не инициализирована.");
                }
            }
            return newStr;
        }

    }
}
