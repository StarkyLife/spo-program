using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroProcessor34
{
    public class SourceEntity
    {
        // строка как string, как в исходниках
        public string sourceString { get; set; }
        public string label { get; set; }
        public string operation { get; set; }
        public List<string> operands { get; set; }
        public string isRemove { get; set; }
        // родитель для исходных строк
        public SourceCode sources { get; set; }

        public SourceEntity()
        {
            this.operands = new List<string>();
        }

        /// <summary>
        /// Обычное представление строки
        /// </summary>
        public override string ToString()
        {
            string temp = "";
            if (!String.IsNullOrEmpty(this.label))
            {
                temp += this.label + ": ";
            }
            temp += this.operation;

            foreach (string op in this.operands)
            {
                temp += " " + op;
            }
            return temp;
        }

        /// <summary>
        /// клонирует строку кода
        /// </summary>
        public SourceEntity Clone()
        {
            return new SourceEntity()
            {
                label = this.label,
                operation = this.operation,
                operands = new List<string>(this.operands),
                sources = this.sources,
                sourceString = this.sourceString
            };
        }
    }
}
