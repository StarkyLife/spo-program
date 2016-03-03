using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroProcessor34
{
    public class SourceCode
    {
        #region Поля класса
        // флаг макроописания и имя текущего макроса
        public bool macroFlag = false;
        public string macroName = null;
        // Список строк кода
        public List<SourceEntity> entities;
        // результаты первого прохода
        public List<SourceEntity> result;
        // список строк, подозрительных на макровызов
        public List<SourceEntity> mbMacroCall;

        public SourceCode(string[] strs)
        {
            //парсим строки в объектное представление
            entities = CodeParser.parse(strs);
            mbMacroCall = new List<SourceEntity>();
            result = new List<SourceEntity>();
            //назначаем родителя для исходных строк
            foreach (SourceEntity se in this.entities)
            {
                se.sources = this;
            }
        }

        public SourceCode(List<SourceEntity> strs)
        {
            //парсим строки в объектное представление
            entities = strs;
            mbMacroCall = new List<SourceEntity>();
            result = new List<SourceEntity>();
            //назначаем родителя для исходных строк
            foreach (SourceEntity se in this.entities)
            {
                se.sources = this;
            }
        }

        #endregion

        /// <summary>
        /// Шаг первого прохода
        /// </summary>
        public void firstRunStep(SourceEntity se, TMOEntity te)
        {
            String operation = se.operation;
            String label = se.label;
            List<String> operands = se.operands;
            try
            {
                CheckSourceEntity.checkLabel(se);
                if (operation == "END")
                {
                    CheckSourceEntity.checkEND(se, this.macroFlag, this.macroName);
                    result.Add(Utils.print(se));
                }
                else if (operation == "GLOBAL" && this.macroFlag == false)
                {
                    CheckSourceEntity.checkGLOBAL(se);
                    if (operands.Count == 1)
                        Global.entities.Add(new GlobalEntity(operands[0], null));
                    else
                        Global.entities.Add(new GlobalEntity(operands[0], Int32.Parse(operands[1])));
                }
                else if (operation == "SET" && this.macroFlag == false)
                {
                    CheckSourceEntity.checkSET(se, te);
                    Global.searchInGlobal(se.operands[0]).value = Int32.Parse(se.operands[1]);
                }
                else if (operation == "INC" && this.macroFlag == false)
                {
                    CheckSourceEntity.checkINC(se, te);
                    Global.searchInGlobal(operands[0]).value++;
                }
                else if (operation == "MACRO")
                {
                    if (te != TMO.root)
                    {
                        throw new SPException("Макроопределения внутри макросов запрещены");
                    }
                    CheckSourceEntity.checkMACRO(se, macroFlag);
                    
                    TMO.entities.Add(new TMOEntity() { name = label, parameters = se.operands });
                    this.macroFlag = true;
                    this.macroName = label;
                }
                else if (operation == "MEND")
                {
                    if (te != TMO.root)
                    {
                        throw new SPException("Макроопределения внутри макросов запрещены");
                    }
                    CheckSourceEntity.checkMEND(se, macroFlag);

                    foreach (SourceEntity mc in mbMacroCall)
                    {
                        if (mc.operation == this.macroName)
                        {
                            TMOEntity currentTe = TMO.searchInTMO(this.macroName);
                            CheckSourceEntity.checkMacroSubstitution(mc, currentTe);
                            List<SourceEntity> res = CheckBody.checkMacroBody(currentTe, operands);
                            // результат макроподстановки
                            List<SourceEntity> macroSubs = new List<SourceEntity>();
                            foreach (SourceEntity str in res)
                            {
                                macroSubs.Add(Utils.print(str));
                            }
                            // Заменяем в результате макровызов на результат макроподстановки
                            for (int i = 0; i < this.result.Count; i++)
                            {
                                if (this.result[i].operation == mc.operation && this.result[i].isRemove == "true")
                                {
                                    this.result.Remove(this.result[i]);
                                    this.result.InsertRange(i, macroSubs);
                                    i += macroSubs.Count - 1;
                                }
                            }
                        }
                    }

                    TMOEntity curTe = TMO.searchInTMO(this.macroName);
                    curTe.IsFinished = true;

                    var prevTe = TMO.GetPrevNotFinishedMacro();
                    if (prevTe != null)
                    {
                        this.macroFlag = true;
                        this.macroName = prevTe.name;
                    }
                    else
                    {
                        this.macroFlag = false;
                        this.macroName = null;
                    }
                }
                else
                {
                    if (this.macroFlag == true)
                    {
                        TMO.searchInTMO(this.macroName).body.Add(se);
                    }
                    else
                    {
                        if (te == TMO.root && (operation == "WHILE" || operation == "ENDW"))
                        {
                            throw new SPException("Использование директивы " + operation + " возможно только в теле макроса: " + se.sourceString);
                        }
                        // макровызов
                        if (TMO.isInTMO(operation))
                        {

                            TMOEntity currentTe = TMO.searchInTMO(operation);
                            CheckSourceEntity.checkMacroSubstitution(se, currentTe);
                            CheckBody.checkMacroRun(se, te, currentTe);

                            List<SourceEntity> res = CheckBody.checkMacroBody(currentTe, operands);
                            foreach (SourceEntity str in res)
                            {
                                result.Add(Utils.print(str));
                            }

                            //if (te == TMO.root)
                            //{
                                
                            //}
                            //else
                            //{
                            //    throw new SPException("Макровызовы внутри макроса запрещены");
                            //}
                        }
                        else
                        {
                            // Добавляем строку в список подозрительных на макровызов и в результат
                            se = Utils.print(se);
                            if (te == TMO.root && macroFlag == false)
                            {
                                se.isRemove = "true";
                                mbMacroCall.Add(se);
                            }
                            result.Add(se);
                        }
                    }
                }
            }
            catch (SPException ex)
            {
                throw new SPException(ex.Message);
            }
        }


        #region Распечатка

        /// <summary>
        /// Распечатывает полностью ассемблерный код без макросов в таблицу
        /// </summary>
        public void printAsm(TextBox tb)
        {
            tb.Clear();
            foreach (SourceEntity se in this.result)
            {
                tb.AppendText(se.ToString() + "\n");
            }
        }

        /// <summary>
        /// Распечатывает полностью ассемблерный код без макросов в консоль
        /// </summary>
        public void printAsm()
        {
            foreach (SourceEntity se in this.result)
            {
                Console.WriteLine(se.ToString());
            }
        }

        #endregion
    }


    public static class CheckSourceEntity
    {
        /// <summary>
        /// Проверка на метку (может быть пустая или много двоеточий)
        /// </summary>
        /// <param name="se">строка с операцией меткой</param>
        public static void checkLabel(SourceEntity se)
        {
            if (se.sourceString.Split(':').Length > 2 && se.operation != "BYTE")
            {
                throw new SPException("Слишком много двоеточий в строке: " + se.sourceString);
            }
            if (se.sourceString.Split(':').Length > 1 && String.IsNullOrEmpty(se.sourceString.Split(':')[0]))
            {
                throw new SPException("Слишком много двоеточий в строке: " + se.sourceString);
            }
        }

        /// <summary>
        /// Проверка строки с операцией MACRO
        /// </summary>
        /// <param name="se">строка с операцией MACRO</param>
        public static void checkMACRO(SourceEntity se, bool macroFlag)
        {
            if (se.sourceString.Contains(":"))
            {
                throw new SPException("При объявлении макроса не должно быть меток: " + se.sourceString);
            }
            if (String.IsNullOrEmpty(se.label) || !Utils.isLabel(se.label))
            {
                throw new SPException("Имя макроса некорректно: " + se.sourceString);
            }
            if (TMO.isInTMO(se.label))
            {
                throw new SPException("Макрос " + se.label + " уже описан: " + se.sourceString);
            }
            //if (macroFlag == true)
            //{
            //    throw new SPException("Макроопределения внутри макроса запрещены: " + se.sourceString);
            //}
            //if (se.operands.Count != 0)
            //{
            //    throw new SPException("У макроса не должно быть параметров: " + se.sourceString);
            //}

            foreach (string o in se.operands)
            {
                if (se.operands.Count(p => p == o) > 1)
                {
                    throw new SPException("Имя параметра \"" + o + "\" повторяется: " + se.sourceString);
                }
                if (!Utils.isLabel(o))
                {
                    throw new SPException("Имя параметра \"" + o + "\" некорректно: " + se.sourceString);
                }
                foreach (TMOEntity te in TMO.entities)
                {
                    foreach (string par in te.parameters)
                    {
                        if (par == o)
                        {
                            throw new SPException("Имя параметра " + o + " уже используется в другом макросе: " + se.sourceString);
                        }
                    }
                }
                Utils.checkNames(o);
                if (o == se.label)
                {
                    throw new SPException("Имя параметра " + o + " некорректно (совпадает с названием макроса): " + se.sourceString);
                }
            }

            Utils.checkNames(se.label);
        }

        /// <summary>
        /// Проверка строки с операцией MEND
        /// </summary>
        /// <param name="se">строка с операцией MEND</param>
        public static void checkMEND(SourceEntity se, bool macroFlag)
        {
            if (se.operands.Count != 0)
            {
                throw new SPException("У директивы MEND не должно быть параметров: " + se.sourceString);
            }
            if (!String.IsNullOrEmpty(se.label))
            {
                throw new SPException("У директивы MEND не должно быть метки: " + se.sourceString);
            }
            if (macroFlag == false)
            {
                throw new SPException("Лишняя директива MEND: " + se.sourceString);
            }
        }

        /// <summary>
        /// Проверка строки с операцией END
        /// </summary>
        /// <param name="se">строка с операцией END</param>
        public static void checkEND(SourceEntity se, bool macroFlag, string macroName)
        {
            if (macroFlag == true)
            {
                throw new SPException("Макрос " + macroName + " не описан полностью: " + se.sourceString);
            }
            //foreach (TMOEntity te in TMO.entities)
            //{
            //    foreach (SourceEntity sse in te.body)
            //    {
            //        if (TMO.isInTMO(sse.operation))
            //        {
            //            throw new SPException("Макровызовы внутри макроса запрещены");
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Проверка макроподстановки
        /// </summary>
        public static void checkMacroSubstitution(SourceEntity se, TMOEntity te)
        {
            if (se.operands.Count != te.parameters.Count)
            {
                throw new SPException("Некорректное количество параметров. Введено: " + se.operands.Count + ". Ожидается: " + te.parameters.Count);
            }
            //if (se.operands.Count != 0)
            //{
            //    throw new SPException("Вызов макроса не должен содержать параметров: " + se.sourceString);
            //}
            if (!String.IsNullOrEmpty(se.label))
            {
                throw new SPException("При макровызове макроса не должно быть меток: " + se.sourceString);
            }

            int temp; string[] vals = null;
            foreach (string prm in se.operands)
            {
                vals = prm.Split('=');
                if (vals.Length != 2)
                {
                    throw new SPException("Параметр '" + prm + "'. Параметры определены некорректно, разделители между '=', названием и значением параметра недопустимы.");
                }

                if (!Global.isInGlobal(vals[1]) && !Int32.TryParse(vals[1], out temp))
                {
                    throw new SPException("Параметр '" + vals[0] + "' имеет некорректное значение (" + vals[1] + ")");
                }
                if (Global.isInGlobal(vals[1]) && Global.searchInGlobal(vals[1]).value == null)
                {
                    throw new SPException("Параметр '" + vals[0] + "' - неинициализованная глобальная переменная");
                }
            }
            //if (!String.IsNullOrEmpty(se.label))
            //{
            //    throw new SPException("При макровызове макрса не должно быть меток: " + se.sourceString);
            //}
        }

        /// <summary>
        /// Проверка строки с операцией GLOBAL
        /// </summary>
        /// <param name="se">строка с операцией GLOBAL</param>
        public static void checkGLOBAL(SourceEntity se)
        {
            //if (macroFlag == true)
            //{
            //    throw new SPException("Глобальные переменные нельзя объявлять в макросе");
            //}
            if (se.operands.Count > 0 && Global.isInGlobal(se.operands[0]))
            {
                throw new SPException("Повторное задание глобальной переменной: " + se.sourceString);
            }
            if (!String.IsNullOrEmpty(se.label))
            {
                throw new SPException("В описании глобальной переменной метки не нужны: " + se.sourceString);
            }
            if (se.operands.Count == 2)
            {
                if (!Utils.isLabel(se.operands[0]))
                {
                    throw new SPException("Некорректное имя глобальной переменной: " + se.sourceString);
                }
                int temp;
                if (Int32.TryParse(se.operands[1], out temp) == false)
                {
                    throw new SPException("Некорректное значение глобальной переменной: " + se.sourceString);
                }
            }
            else if (se.operands.Count == 1)
            {
                if (!Utils.isLabel(se.operands[0]))
                {
                    throw new SPException("Некорректное имя глобальной переменной: " + se.sourceString);
                }
            }
            else
            {
                throw new SPException("Некорректное количество операндов в директиве GLOBAL: " + se.sourceString);
            }
            Utils.checkNames(se.operands[0]);
        }

        /// <summary>
        /// Проверка строки с операцией SET
        /// </summary>
        /// <param name="se">строка с операцией SET</param>
        public static void checkSET(SourceEntity se, TMOEntity te)
        {
            if (!String.IsNullOrEmpty(se.label))
            {
                throw new SPException("В директиве SET метки не должно быть: " + se.sourceString);
            }
            if (se.operands.Count == 2)
            {
                if (!Global.isInGlobal(se.operands[0]))
                {
                    throw new SPException("Некорректное имя глобальной переменной: " + se.sourceString);
                }
                int temp;
                if (Int32.TryParse(se.operands[1], out temp) == false)
                {
                    throw new SPException("Некорректное значение глобальной переменной: " + se.sourceString);
                }
                foreach (Dictionary<List<string>, TMOEntity> dict in Global.whileVar)
                {
                    if (dict.Keys.First().Contains(se.operands[0]) && dict.Values.First() != te)
                    {
                        throw new SPException("Глобальная переменная " + se.operands[0] + " используется как счетчик в цикле: " + se.sourceString);
                    }
                }
            }
            else
            {
                throw new SPException("Некорректное количество операндов в директиве SET: " + se.sourceString);
            }
        }

        /// <summary>
        /// Проверка строки с операцией INC
        /// </summary>
        /// <param name="se">строка с операцией INC</param>
        public static void checkINC(SourceEntity se, TMOEntity te)
        {
            if (!String.IsNullOrEmpty(se.label))
            {
                throw new SPException("В директиве INC метки не должно быть: " + se.sourceString);
            }
            if (se.operands.Count == 1)
            {
                if (!Global.isInGlobal(se.operands[0]))
                {
                    throw new SPException("Некорректное имя глобальной переменной: " + se.sourceString);
                }
                if (Global.searchInGlobal(se.operands[0]).value == null)
                {
                    throw new SPException("Глобальной переменной " + se.operands[0] + " не присвоено значение.");
                }
                foreach (Dictionary<List<string>, TMOEntity> dict in Global.whileVar)
                {
                    if (dict.Keys.First().Contains(se.operands[0]) && dict.Values.First() != te)
                    {
                        throw new SPException("Глобальная переменная " + se.operands[0] + " используется как счетчик в цикле: " + se.sourceString);
                    }
                }
            }
            else
            {
                throw new SPException("Некорректное количество операндов в директиве INC: " + se.sourceString);
            }
        }

    }

    public static class CheckBody
    {
        /// <summary>
        /// Проверка body макроса на GLOBAL, SET и формирование выходного body
        /// </summary>
        /// <param name="se">макрос TMOEntity</param>
        public static List<SourceEntity> checkMacroBody(TMOEntity te, List<String> operands)
        {
            //foreach (SourceEntity se in te.body)
            //{
            //    if (TMO.isInTMO(se.operation))
            //    {
            //        throw new SPException("Макровызовы внутри макроса запрещены: " + se.sourceString);
            //    }
            //}
            List<SourceEntity> result = te.invoke(operands.ToArray());
            return result;
        }

        /// <summary>
        /// Проверка макроподстановки
        /// </summary>
        public static void checkMacroRun(SourceEntity se, TMOEntity parent, TMOEntity child)
        {
            //TMOEntity current = parent;
            //List<TMOEntity> list = new List<TMOEntity>();
            //while (current.prev != null)
            //{
            //    if (list.Contains(current))
            //    {
            //        throw new SPException("Перекрестные ссылки и рекурсия запрещены.");
            //    }
            //    list.Add(current);
            //    current = current.prev;
            //}
            if (TMO.isInTMO(child.name) && parent.name == child.name)
            {
                throw new SPException("Макрос \"" + child.name + "\" не может быть вызван из себя (Рекурсия запрещена).");
            }
            //if (TMO.isInTMO(child.name) && !parent.localTMO.Contains(child))
            //{
            //    throw new SPException("Макрос " + child.name + " не входит в область видимости " +
            //        (parent.name == "root" ? "основной программы" : "тела макроса " + parent.name) + ".");
            //}
        }
    }
}
