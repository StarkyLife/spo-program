using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroProcessor34
{
    public static class Global
    {
        public static List<GlobalEntity> entities = new List<GlobalEntity>();
        public static Stack<Dictionary<List<string>, TMOEntity>> whileVar = new Stack<Dictionary<List<string>, TMOEntity>>();

        /// <summary>
        /// Обновить Global (сделать список Global пустым)
        /// </summary>
        public static void refresh()
        {
            Global.entities = new List<GlobalEntity>();
            Global.whileVar = new Stack<Dictionary<List<string>, TMOEntity>>();
        }

        /// <summary>
        /// Поиск макроса в Global по имени
        /// </summary>
        public static GlobalEntity searchInGlobal(string name)
        {
            GlobalEntity result = (from GlobalEntity ge in Global.entities
                                   where ge.name == name
                                   select ge).SingleOrDefault<GlobalEntity>();
            return result;
        }

        /// <summary>
        /// Есть ли глобальная переменная в Global
        /// </summary>
        public static bool isInGlobal(string name)
        {
            return !(searchInGlobal(name) == null);
        }

        /// <summary>
        /// Распечатать Global в таблицу
        /// </summary>
        public static void printGlobal(DataGridView dgv)
        {
            dgv.Rows.Clear();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows.Remove(dgv.Rows[i]);
            }
            foreach (GlobalEntity e in Global.entities)
            {
                dgv.Rows.Add(e.name, e.value != null ? e.value.ToString() : "");
            }
        }

        /// <summary>
        /// Распечатать Global в консоль
        /// </summary>
        public static void printGlobal()
        {
            foreach (GlobalEntity e in Global.entities)
            {
                Console.WriteLine(e.name + " = " + (e.value != null ? e.value.ToString() : ""));
            }
        }

    }


    /// <summary>
    /// Элемент - глобальная переменная
    /// </summary>
    public class GlobalEntity
    {
        public string name { get; set; }
        public int? value { get; set; }

        public GlobalEntity(string name, int? value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
