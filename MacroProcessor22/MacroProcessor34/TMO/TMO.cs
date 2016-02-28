using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroProcessor34
{
    public static class TMO
    {
        public static List<TMOEntity> entities = new List<TMOEntity>();
        public static TMOEntity root = new TMOEntity() { name = "root" };

        /// <summary>
        /// Обновить ТМО (сделать список ТМО пустым)
        /// </summary>
        public static void refresh()
        {
            TMO.entities = new List<TMOEntity>();
        }

        /// <summary>
        /// Поиск макроса в ТМО по имени
        /// </summary>
        public static TMOEntity searchInTMO(string name)
        {
            TMOEntity result = (from TMOEntity te in TMO.entities
                                where te.name == name
                                select te).SingleOrDefault<TMOEntity>();
            return result;
        }

        /// <summary>
        /// Есть ли макрос в ТМО
        /// </summary>
        public static bool isInTMO(string name)
        {
            return searchInTMO(name) != null;
        }

        /// <summary>
        /// Достать предыдущий макрос (Вложенные макроопределния - да)
        /// </summary>
        /// <returns></returns>
        public static TMOEntity GetPrevNotFinishedMacro()
        {
            TMOEntity result = (from te in TMO.entities where !te.IsFinished select te).LastOrDefault();
            return result;
        }

        /// <summary>
        /// Распечатать ТМО в таблицу
        /// </summary>
        public static void printTMO(DataGridView dgv)
        {
            dgv.Rows.Clear();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows.Remove(dgv.Rows[i]);
            }
            foreach (TMOEntity e in TMO.entities)
            {
                dgv.Rows.Add(e.name, e.body.Count > 0 ? e.body[0].ToString() : "");
                for (int i = 1; i < e.body.Count; i++)
                {
                    dgv.Rows.Add(null, e.body[i].ToString());
                }
            }
        }

        /// <summary>
        /// Распечатать ТМО в консоль
        /// </summary>
        public static void printTMO()
        {
            foreach (TMOEntity e in TMO.entities)
            {
                Console.WriteLine("Макрос    " + e.name + ":");
                for (int i = 0; i < e.body.Count; i++)
                {
                    Console.WriteLine(e.body[i].ToString());
                }
            }
        }
    }
}
