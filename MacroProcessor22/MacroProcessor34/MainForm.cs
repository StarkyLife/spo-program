using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroProcessor34
{
    public partial class MainForm : Form
    {
        GUIProgram program;
        private static string currentDirectory = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
        public string input_file = currentDirectory + "\\source.txt";
        public string output_file = currentDirectory + "\\result.txt";

        public MainForm()
        {
            InitializeComponent();
            this.Show();
            this.Activate();
            this.program = new GUIProgram(tb_source_code);
            btn_next_step.Enabled = false;
        }

        /// <summary>
        /// 2 проход
        /// </summary>
        private void btn_next_step_Click(object sender, EventArgs e)
        {            
            program.sourceCode.printAsm(tb_result);
            btn_next_step.Enabled = false;
        }

        /// <summary>
        /// Следующий шаг выполнения проги
        /// </summary>
        private void btn_step()
        {
            try
            {
                this.btn_first_run.Enabled = false;
                this.btn_refresh_all.Enabled = true;

                // если исходный текст пуст
                if (program.sourceCode.entities.Count == 0)
                {
                    throw new SPException("Исходный текст должен содержать хотя бы одну строку");
                }
                if (program.index == 0)
                {
                    tb_error.Clear();
                }
                // если это последняя строка
                if ((program.index + 1) == program.sourceCode.entities.Count)
                {
                    //this.btn_next_step.Enabled = false;
                    CheckSourceEntity.checkEND(new SourceEntity(), false, "");
                }

                this.program.nextFirstStep(this.tb_result);
                TMO.printTMO(this.dgv_tmo);
                Global.printGlobal(this.dgv_global);
            }
            catch (SPException ex)
            {
                this.tb_error.Text = ex.Message;
                this.disableButtons();
                this.btn_refresh_all.Enabled = true;
            }
            catch (Exception)
            {
                this.tb_error.Text = "Ошибка!";
                this.disableButtons();
                this.btn_refresh_all.Enabled = true;
            }
        }

        /// <summary>
        /// Первый проход
        /// </summary>
        private void btn_first_run_Click(object sender, EventArgs e)
        {
            this.btn_first_run.Enabled = false;
            this.btn_next_step.Enabled = true;
            this.btn_refresh_all.Enabled = true;

            while (true)
            {
                // если ошибка или конец текста - не продолжаем
                if (!String.IsNullOrEmpty(tb_error.Text) || (this.program.index) == this.program.sourceCode.entities.Count)
                {
                    break;
                }
                // иначе выполняем шаг
                //this.btn_next_step_Click(sender, e);
                this.btn_step();
            }
        }

        #region Хрень

        /// <summary>
        /// Обновить все данные, заново начать прогу
        /// </summary>
        private void btn_refresh_all_Click(object sender, EventArgs e)
        {
            this.program = new GUIProgram(this.tb_source_code);
            this.tb_error.Clear();
            this.tb_result.Clear();
            TMO.printTMO(this.dgv_tmo);
            Global.printGlobal(this.dgv_global);

            this.enableButtons();
        }

        /// <summary>
        /// Загрузить исходники из выбранного файла в TextBox
        /// </summary>
        private void btn_load_file_Click(object sender, EventArgs e)
        {
            try
            {
                this.loadFile();
                enableButtons();
                this.tb_error.Text = String.Empty;
            }
            catch (Exception ex)
            {
                this.tb_error.Text = ex.Message;
                disableButtons();
            }
        }

        /// <summary>
        /// Записать результат в файл
        /// </summary>
        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (.txt)|*.txt";
            sfd.InitialDirectory = currentDirectory;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.tb_input_file.Text = sfd.FileName;
                List<String> temp = tb_result.Text.Split('\n').ToList<String>();
                StreamWriter sw = new StreamWriter(sfd.FileName);
                foreach (string str in temp)
                {
                    sw.WriteLine(str);
                }
                sw.Close();
            }
        }

        /// <summary>
        /// Заполняет TеxtBox исходных данных, считав их из выбранного файла в OpenFileDialog
        /// </summary>
        public void loadFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (.txt)|*.txt";
            ofd.InitialDirectory = currentDirectory;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.tb_source_code.Clear();
                this.tb_input_file.Text = ofd.FileName;
                try
                {
                    fillSourceTextBoxFromFile(ofd.FileName, this.tb_source_code);
                }
                catch (Exception e)
                {
                    throw new SPException(e.Message);
                }
            }
        }

        /// <summary>
        /// Заполнить TеxtBox исходных данных, считав их из заданного файла
        /// </summary>
        /// <param name="file">Имя файла с исходниками</param>
        /// <param name="tb">TеxtBox исходных данных</param>
        public void fillSourceTextBoxFromFile(string file, TextBox tb)
        {
            try
            {
                String temp = String.Empty;
                StreamReader sr = new StreamReader(file);
                while ((temp = sr.ReadLine()) != null)
                {
                    tb.AppendText(temp + Environment.NewLine);
                }
                sr.Close();
            }
            catch (Exception e)
            {
                throw new SPException("Не удалось загрузить данные с файла. Возможно путь к файлу указан неверно");
            }
        }

        /// <summary>
        /// При загрузке формы заплняем TB исходниками, если можно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fm_main_Load(object sender, EventArgs e)
        {
            this.tb_input_file.Text = this.input_file;
            this.tb_output_file.Text = this.output_file;

            disableButtons();

            if (!String.IsNullOrEmpty(this.tb_input_file.Text))
            {
                try
                {
                    fillSourceTextBoxFromFile(this.tb_input_file.Text, this.tb_source_code);
                    enableButtons();
                    this.tb_error.Text = String.Empty;
                }
                catch (Exception ex)
                {
                    this.tb_error.Text = ex.Message;
                    disableButtons();
                }
            }
        }

        /// <summary>
        /// При изменении исходного кода все очищаем
        /// </summary>
        private void tb_source_code_TextChanged(object sender, EventArgs e)
        {
            this.btn_refresh_all_Click(sender, e);
        }

        private void enableButtons()
        {
            this.btn_next_step.Enabled = true;
            this.btn_refresh_all.Enabled = true;
            this.btn_first_run.Enabled = true;
        }

        private void disableButtons()
        {
            this.btn_next_step.Enabled = false;
            this.btn_refresh_all.Enabled = false;
            this.btn_first_run.Enabled = false;
        }

        #endregion


    }
}
