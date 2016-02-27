using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroProcessor34
{
    public class GUIProgram
    {
        public SourceCode sourceCode = null;
        public List<string> sourceStrings = null;
        // номер строки
        public int index = 0;

        /// <summary>
        /// Конструктор. Считывает исходники с файла
        /// </summary>
        public GUIProgram(TextBox tb)
        {
            TMO.refresh();
            Global.refresh();
            string[] temp = tb.Text.Split('\n');
            refresh(temp);
        }



        /// <summary>
        /// Шаг выполнения программы 1 просмотра
        /// </summary>
        public void nextFirstStep(TextBox tb)
        {
            try
            {
                this.sourceCode.firstRunStep(this.sourceCode.entities[index++], TMO.root);
                //this.sourceCode.printAsm(tb);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                this.index = 0;
                this.refresh(this.sourceStrings.ToArray());
                TMO.refresh();
                Global.refresh();
            }
            catch (SPException ex)
            {
                throw new SPException("Строка \"" + this.sourceCode.entities[index - 1].ToString() + "\": " + ex.Message + "\n");
            }
            catch (Exception e)
            {
                throw new SPException("Ошибка в строке \"" + this.sourceCode.entities[index - 1].ToString() + "\n");
            }
        }

        /// <summary>
        /// Обновляет результаты предыдущего прохода
        /// </summary>
        private void refresh(string[] temp)
        {
            this.sourceCode = new SourceCode(temp);
            this.sourceStrings = new List<string>(temp);
        }

        /// <summary>
        /// Печатает исходники sourceStrings в TextBox
        /// </summary>
        public void print(TextBox tb)
        {
            tb.Clear();
            foreach (string str in this.sourceStrings)
            {
                tb.AppendText(str + Environment.NewLine);
            }
        }
    }
}
