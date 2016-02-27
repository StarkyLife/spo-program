using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProcessor34
{
    public class TMOEntity
    {
        public string name { get; set; }
        public List<SourceEntity> body { get; set; }
        public List<String> parameters { get; set; }
        // локальные переменные (Метки внутри макроса - да)
        public Dictionary<string, int?> local { get; set; }
        // для уникальных меток (Метки внутри макроса - да)
        public int count = 0;
        // список меток, используемых при AGO
        //public List<string> agoLabels { get; set; }
        // если будет 10 000 000 итераций - считаем это бесконечным циклом:)
        private int counter { get; set; }

        public TMOEntity()
        {
            this.local = new Dictionary<string, int?>();
            this.body = new List<SourceEntity>();
            this.parameters = new List<String>();
            //this.agoLabels = new List<string>();
            this.counter = 0;
        }


        public List<SourceEntity> invoke(string[] prms)
        {
            // Подставим значения параметров
            List<SourceEntity> newBody = CheckMacros.InvokeMacroParams(this, prms);

            SourceCode sc = new SourceCode(newBody);
            SourceEntity current = null;

            // проверки
            CheckMacros.CheckMacroLabels(this);

            //заменяем метки на "крутые" уникальные метки
            this.local = new Dictionary<string, int?>();
            foreach (SourceEntity se in sc.entities)
            {
                current = se.Clone();
                if (!String.IsNullOrEmpty(current.label))
                {
                    if (!Utils.isLabel(current.label))
                    {
                        throw new SPException("Некорректное задание метки в макросе: " + se.sourceString);
                    }
                    Utils.checkNames(current.label);
                    if (this.local.Keys.Contains(current.label))
                    {
                        throw new SPException("Повторное задание метки в макросе: " + se.sourceString);
                    }
                    this.local.Add(current.label, null);
                }
            }

            // исполнять ли команду дальше
            Stack<bool> runStack = new Stack<bool>();
            runStack.Push(true);
            // стек комманд, появлявшихся ранее
            Stack<Command> commandStack = new Stack<Command>();
            // стек строк, куда надо вернуться при while
            Stack<int> whileStack = new Stack<int>();
            //
            bool elseFlag;
            for (int i = 0; i < sc.entities.Count; i++)
            {
                this.counter++;
                if (this.counter == 1000000)
                {
                    throw new SPException("Обнаружен бесконечный цикл");
                }

                current = newBody[i].Clone();
                CheckMacros.checkInner(current, commandStack);

                #region IF
                //if (current.operation == "IF")
                //{
                //    CheckMacros.checkIF(this);
                //    commandStack.Push(Command.if_);
                //    runStack.Push(runStack.Peek() && Utils.compare(current.operands[0]));
                //    continue;
                //}
                //if (current.operation == "ELSE")
                //{
                //    CheckMacros.checkIF(this);
                //    commandStack.Pop();
                //    commandStack.Push(Command.else_);
                //    elseFlag = runStack.Pop();
                //    runStack.Push(runStack.Peek() && !elseFlag);
                //    continue;
                //}
                //if (current.operation == "ENDIF")
                //{
                //    CheckMacros.checkIF(this);
                //    commandStack.Pop();
                //    runStack.Pop();
                //    continue;
                //}
                #endregion

                if (current.operation == "WHILE")
                {
                    CheckMacros.checkWhileEndw(this);
                    commandStack.Push(Command.while_);
                    runStack.Push(runStack.Peek() && Utils.compare(current.operands[0]));
                    whileStack.Push(i);
                    continue;
                }
                if (current.operation == "ENDW")
                {
                    CheckMacros.checkWhileEndw(this);
                    commandStack.Pop();
                    int newI = whileStack.Pop() - 1;
                    if (runStack.Pop())
                    {
                        i = newI;
                    }
                    continue;
                }

                #region AIF AGO

                //if (current.operation == "AIF" && Utils.compare(current.operands[0]) || current.operation == "AGO")
                //{
                //    CheckMacros.checkAIF(this);
                //    if (runStack.Peek())
                //    {
                //        string label = current.operation == "AIF" ? current.operands[1] : current.operands[0];
                //        // находим метку, чтобы туда прыгнуть
                //        bool ready = false;
                //        Stack<bool> agoStack = new Stack<bool>();
                //        // вверх
                //        for (int j = i; j >= 0; j--)
                //        {
                //            if (this.body[j].operation == "IF" || this.body[j].operation == "WHILE")
                //            {
                //                if (agoStack.Count > 0)
                //                {
                //                    agoStack.Pop();
                //                }
                //            }
                //            if (this.body[j].operation == "ELSE")
                //            {
                //                if (agoStack.Count > 0)
                //                {
                //                    agoStack.Pop();
                //                }
                //                agoStack.Push(false);
                //            }
                //            if (this.body[j].operation == "ENDIF" || this.body[j].operation == "ENDW")
                //            {
                //                agoStack.Push(false);
                //            }
                //            if (this.body[j].label == label && (agoStack.Count == 0 || agoStack.Peek()))
                //            {
                //                i = j - 1;
                //                ready = true;
                //                break;
                //            }
                //        }

                //        // вниз
                //        if (!ready)
                //        {
                //            for (int j = i; j < this.body.Count; j++)
                //            {
                //                if (this.body[j].operation == "IF" || this.body[j].operation == "WHILE")
                //                {
                //                    agoStack.Push(false);
                //                }
                //                if (this.body[j].operation == "ELSE")
                //                {
                //                    if (agoStack.Count > 0)
                //                    {
                //                        agoStack.Pop();
                //                    }
                //                    agoStack.Push(false);
                //                }
                //                if (this.body[j].operation == "ENDIF" || this.body[j].operation == "ENDW")
                //                {
                //                    if (agoStack.Count > 0)
                //                    {
                //                        agoStack.Pop();
                //                    }
                //                }
                //                if (this.body[j].label == label && (agoStack.Count == 0 || agoStack.Peek()))
                //                {
                //                    i = j - 1;
                //                    ready = true;
                //                    break;
                //                }
                //            }
                //        }
                //        if (!ready)
                //        {
                //            throw new SPException("Метка " + label + " при директиве " + current.operation + " находится вне зоны видимости или не описана");
                //        }
                //    }
                //    continue;
                //}

                #endregion

                if (runStack.Peek())
                {
                    if (!String.IsNullOrEmpty(current.label))
                    {
                        current.label = current.label + "_" + this.name + "_" + this.count;
                    }
                    for (int j = 0; j < current.operands.Count; j++)
                    {
                        if (this.local.Keys.Contains(current.operands[j]))
                        {
                            current.operands[j] = current.operands[j] + "_" + this.name + "_" + this.count;
                        }
                    }

                    sc.firstRunStep(current, this);
                }
            }
            this.count++;

            //foreach (SourceEntity se in sc.result)
            //{
                //se.label = null;
            //}
            return sc.result;
        }

    }

    public enum Command
    {
        if_,
        else_,
        while_,
        ago_,
        empty_
    }

    public static class CheckMacros
    {
        /// <summary>
        /// Подстановка значений вместо параметров макроса
        /// </summary>
        /// <param name="te"></param>
        /// <param name="prms"></param>
        /// <returns></returns>
        public static List<SourceEntity> InvokeMacroParams(TMOEntity te, string[] prms)
        {
            // формируем локальную область видимости (параметры в виде key-value)
            Dictionary<string, int> dict = new Dictionary<string, int>();
            if (prms.Length != te.parameters.Count)
            {
                throw new SPException("Количество параметров вызова некорректно, необходимо " + te.parameters.Count + " параметров.");
            }
            string[] vals = null;
            int temp;
            foreach (string prm in prms)
            {
                vals = prm.Split('=');
                if (vals.Length != 2)
                {
                    throw new SPException("Параметр '" + prm + "'. Параметры определены некорректно, разделители между '=', названием и значением параметра недопустимы.");
                }
                if (!te.parameters.Contains(vals[0]))
                {
                    throw new SPException("Параметра '" + vals[0] + "' не существует в макроопределении.");
                }
                if (!Global.isInGlobal(vals[1]) && !Int32.TryParse(vals[1], out temp))
                {
                    throw new SPException("Параметр '" + prm + "' имеет некорректное значение.");
                }
                if (Global.isInGlobal(vals[1]) && Global.searchInGlobal(vals[1]).value == null)
                {
                    throw new SPException("Параметр '" + prm + "' - неинициализованная глобальная переменная!");
                }
                if (dict.Keys.Contains(vals[0]))
                {
                    throw new SPException("Параметр '" + vals[0] + "' повторяется.");
                }

                dict.Add(vals[0], Global.isInGlobal(vals[1]) ? (int)Global.searchInGlobal(vals[1]).value : Int32.Parse(vals[1]));
            }

            List<SourceEntity> result = new List<SourceEntity>();
            foreach (SourceEntity se in te.body)
            {
                // Проверка на использование меток внутри макроса
                //if (!String.IsNullOrEmpty(se.label) && se.operation != "MACRO")
                //{
                //    throw new SPException("Метки внутри макроса " + te.name + " запрещены");
                //}
                result.Add(se.Clone());
            }

            // замена параметров в макросе на числа
            foreach (SourceEntity se in result)
            {
                if (se.operation == "WHILE")
                {
                    foreach (string sign in Utils.signs)
                    {
                        string[] t = se.operands[0].Split(new string[] { sign }, StringSplitOptions.None);
                        if (t.Length == 2)
                        {
                            if (te.parameters.Contains(t[0].Trim()))
                            {
                                t[0] = dict[t[0].Trim()].ToString();
                            }
                            if (te.parameters.Contains(t[1].Trim()))
                            {
                                t[1] = dict[t[1].Trim()].ToString();
                            }
                            se.operands[0] = t[0] + sign + t[1];
                            break;
                        }
                    }
                }
                else if (se.operation == "IF")
                {
                    foreach (string sign in Utils.signs)
                    {
                        string[] t = se.operands[0].Split(new string[] { sign }, StringSplitOptions.None);
                        if (t.Length == 2)
                        {
                            if (te.parameters.Contains(t[0].Trim()))
                            {
                                t[0] = dict[t[0].Trim()].ToString();
                            }
                            if (te.parameters.Contains(t[1].Trim()))
                            {
                                t[1] = dict[t[1].Trim()].ToString();
                            }
                            se.operands[0] = t[0] + sign + t[1];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < se.operands.Count; i++)
                    {
                        if (dict.Keys.Contains(se.operands[i]))
                        {
                            se.operands[i] = dict[se.operands[i]].ToString();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Проверяет макрос на наличие меток
        /// </summary>
        public static void CheckMacroLabels(TMOEntity te)
        {
            List<SourceEntity> result = new List<SourceEntity>();
            foreach (SourceEntity se in te.body)
            {
                result.Add(se.Clone());
            }
            //// Определяем метки, используемые при AGO
            //foreach (SourceEntity se in result)
            //{
            //    if (se.operation == "AGO" && se.operands.Count > 0 && !te.agoLabels.Contains(se.operands[0]))
            //    {
            //        te.agoLabels.Add(se.operands[0]);
            //    }
            //    if (se.operation == "AIF" && se.operands.Count > 1 && !te.agoLabels.Contains(se.operands[1]))
            //    {
            //        te.agoLabels.Add(se.operands[1]);
            //    }
            //}
            //формируется локальная область видимости
            // Список меток, которые являютс ячастью AGO, и уже найдены
            //List<string> markedLabels = new List<string>();
            //foreach (SourceEntity se in result)
            //{
                //if (!String.IsNullOrEmpty(se.label))
                //{
                    //if (!Utils.isLabel(se.label))
                    //{
                        //throw new SPException("Метки внутри макроса " + te.name + " запрещены");
                    //}
                    //if (!te.agoLabels.Contains(se.label))
                    //{
                    //    throw new SPException("Метки внутри макроса " + te.name + " запрещены");
                    //}
                    //else
                    //{
                    //    if (markedLabels.Contains(se.label))
                    //    {
                    //        throw new SPException("Повторное описание метки " + se.label + " в макросе " + te.name);
                    //    }
                    //    markedLabels.Add(se.label);
                    //}
            //    }
            //}
            //if (te.agoLabels.Count != markedLabels.Count)
            //{
            //    throw new SPException("Ошибка использования директивы AGO или AIF. Метка в пределах макроса " + te.name + " не найдена.");
            //}
        }

        /// <summary>
        /// Проверка макроса на WHILE-ENDW
        /// </summary>
        public static void checkWhileEndw(TMOEntity te)
        {
            List<SourceEntity> body = te.body;
            int whileCount = 0;
            //проверка корректности WHILE-ENDW
            try
            {
                foreach (SourceEntity str in body)
                {
                    if (str.operation == "WHILE")
                    {
                        if (str.operands.Count != 1)
                        {
                            throw new SPException("Некорректное количество операндов директивы WHILE: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве WHILE метки быть не должно: " + str.sourceString);
                        }
                        whileCount++;
                    }
                    else if (str.operation == "ENDW")
                    {
                        if (str.operands.Count != 0)
                        {
                            throw new SPException("Некорректное количество операндов директивы ENDW: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве ENDW метки быть не должно: " + str.sourceString);
                        }
                        whileCount--;
                        if (whileCount < 0)
                        {
                            throw new SPException("Некорректное использование директив WHILE и ENDW");
                        }
                    }
                    else if ((str.operation == "MACRO" || str.operation == "MEND") && whileCount > 0)
                    {
                        throw new SPException("Объявление макросов в цикле запрещено");
                    }
                    else if (str.operation == "GLOBAL" && whileCount > 0)
                    {
                        throw new SPException("Объявление глобальных переменных в цикле запрещено");
                    }
                }

                if (whileCount != 0)
                {
                    throw new SPException("Некорректное использование директив WHILE и ENDW");
                }
            }
            catch (SPException ex)
            {
                throw new SPException(ex.Message, ex.errorString);
            }
            catch (Exception)
            {
                throw new SPException("Некорректное использование директив WHILE и ENDW");
            }
        }

        /// <summary>
        /// Проверка макроса на IF-ELSE-ENDIF
        /// </summary>
        /// <param name="body"></param>
        public static void checkIF(TMOEntity te)
        {
            List<SourceEntity> body = te.body;
            Stack<bool> stackIfHasElse = new Stack<bool>();
            //проверка корректности IF-ELSE-ENDIF
            try
            {
                foreach (SourceEntity str in body)
                {
                    if (str.operation == "IF")
                    {
                        if (str.operands.Count != 1)
                        {
                            throw new SPException("Некорректное использование директивы IF: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве IF метки быть не должно: " + str.sourceString);
                        }
                        stackIfHasElse.Push(false);
                    }
                    if (str.operation == "ELSE")
                    {
                        if (str.operands.Count != 0)
                        {
                            throw new SPException("Некорректное использование директивы ELSE: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве ELSE метки быть не должно: " + str.sourceString);
                        }
                        if (stackIfHasElse.Peek() == true)
                        {
                            throw new SPException("Лишняя ветка ELSE: " + str.sourceString);
                        }
                        else
                        {
                            stackIfHasElse.Pop();
                            stackIfHasElse.Push(true);
                        }
                    }
                    if (str.operation == "ENDIF")
                    {
                        if (str.operands.Count != 0)
                        {
                            throw new SPException("Некорректное использование директивы ENDIF: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве ENDIF метки быть не должно: " + str.sourceString);
                        }
                        stackIfHasElse.Pop();
                    }
                }

                if (stackIfHasElse.Count > 0)
                {
                    throw new SPException("Отсутствует директива ENDIF");
                }
            }
            catch (SPException ex)
            {
                throw new SPException(ex.Message, ex.errorString);
            }
            catch (Exception)
            {
                throw new SPException("Некорректное использование директив IF - ENDIF");
            }
        }

        /// <summary>
        /// Проверка макроса на IF-ELSE-ENDIF
        /// </summary>
        /// <param name="body"></param>
        public static void checkAIF(TMOEntity te)
        {
            List<SourceEntity> body = te.body;
            try
            {
                foreach (SourceEntity str in body)
                {
                    if (str.operation == "AIF")
                    {
                        if (str.operands.Count != 2)
                        {
                            throw new SPException("Некорректное использование директивы AIF: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве AIF метки быть не должно: " + str.sourceString);
                        }
                        if (!Utils.isLabel(str.operands[1]))
                        {
                            throw new SPException("При директиве AIF отсутствует метка для ссылки: " + str.sourceString);
                        }
                    }
                    if (str.operation == "AGO")
                    {
                        if (str.operands.Count != 1)
                        {
                            throw new SPException("Некорректное использование директивы AGO: " + str.sourceString);
                        }
                        if (!String.IsNullOrEmpty(str.label))
                        {
                            throw new SPException("При директиве AGO метки быть не должно: " + str.sourceString);
                        }
                        if (!Utils.isLabel(str.operands[0]))
                        {
                            throw new SPException("При директиве AGO отсутствует метка для ссылки: " + str.sourceString);
                        }
                    }
                }
            }
            catch (SPException ex)
            {
                throw new SPException(ex.Message, ex.errorString);
            }
            catch (Exception)
            {
                throw new SPException("Некорректное использование директив AIF-AGO");
            }
        }

        /// <summary>
        /// Проверка вложенностей
        /// </summary>
        /// <param name="current"></param>
        /// <param name="stack"></param>
        public static void checkInner(SourceEntity current, Stack<Command> stack)
        {
            if (current.operation == "IF")
            {
                return;
            }
            if (current.operation == "ELSE")
            {
                if (stack.Count > 0 && stack.Peek() != Command.if_)
                {
                    throw new SPException("Некорректное использование директивы ELSE");
                }
                return;
            }
            if (current.operation == "ENDIF")
            {
                if (stack.Count > 0 && stack.Peek() != Command.if_ && stack.Peek() != Command.else_)
                {
                    throw new SPException("Некорректное использование директивы ENDIF");
                }
                return;
            }
            if (current.operation == "WHILE")
            {
                return;
            }
            if (current.operation == "ENDW")
            {
                if (stack.Count > 0 && stack.Peek() != Command.while_)
                {
                    throw new SPException("Некорректное использование директивы ENDW");
                }
                return;
            }
        }
    }
}
