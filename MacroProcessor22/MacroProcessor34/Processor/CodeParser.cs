using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProcessor34
{
    public static class CodeParser
    {
        /// <summary>
        /// Парсит массив строк в масссив SourceEntity, но только до появления первого END в качестве операции
        /// </summary>
        public static List<SourceEntity> parse(string[] strs)
        {
            List<SourceEntity> result = new List<SourceEntity>();
            foreach (string s in strs)
            {
                // пропускаем пустую строку
                if (String.IsNullOrEmpty(s.Trim()))
                    continue;
                string currentString = s.ToUpper().Trim();
                SourceEntity se = new SourceEntity() { sourceString = currentString };

                //разборка метки
                if (currentString.Contains(':') && (!currentString.Contains("BYTE") || currentString.IndexOf(':') < currentString.IndexOf("C'")))
                {
                    se.label = currentString.Split(':')[0].Trim();
                    currentString = currentString.Remove(0, currentString.Split(':')[0].Length + 1).Trim();
                }

                if (currentString.Split(null as char[], StringSplitOptions.RemoveEmptyEntries).Length > 0)
                {
                    se.operation = currentString.Split(null as char[], StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    currentString = currentString.Remove(0, currentString.Split(null as char[], StringSplitOptions.RemoveEmptyEntries)[0].Length).Trim();
                }

                if (se.operation == "BYTE")
                {
                    se.operands.Add(currentString.Trim());
                }
                else
                {
                    for (int i = 0; i < currentString.Split(null as char[], StringSplitOptions.RemoveEmptyEntries).Length; i++)
                    {
                        se.operands.Add(currentString.Split(null as char[], StringSplitOptions.RemoveEmptyEntries)[i].Trim());
                    }
                }

                //название проги или макроса - в поле метки
                if (se.operands.Count > 0 && se.operands[0] == "MACRO")
                {
                    se.label = se.operation;
                    se.operation = se.operands[0];
                    for (int i = 1; i < se.operands.Count; i++)
                    {
                        se.operands[i - 1] = se.operands[i];
                    }
                    se.operands.RemoveAt(se.operands.Count - 1);
                }
                result.Add(se);

                //Читаем только до энда
                if (se.operation == "END")
                {
                    return result;
                }
            }

            return result;
        }
    }
}
