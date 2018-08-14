using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAS.Concept.File.Upload.Client;

namespace BAS.Concept.File.Upload.Win32Manager
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblFileDetailId.Text = "";
            lblFileDetailName.Text = "";
            lblFileDetailMime.Text = "";
        }

        private void HabilitaDesabilitaTela(bool habilita = true)
        {
            btnListarArquivos.Enabled = habilita;
            dataGridView1.Enabled = habilita;
            groupBox1.Enabled = habilita;

            Cursor = habilita
                ? Cursors.Default
                : Cursors.WaitCursor;
        }

        private void Atualizalista()
        {
            try
            {
                HabilitaDesabilitaTela(false);

                var response = new BasConceptFileUploadClient().GetAllFiles();

                bindingSource1.Clear();

                foreach (var fileInfo in response.Data)
                {
                    bindingSource1.Add(fileInfo);
                }
            }
            catch (Exception e)
            {
                bindingSource1.Clear();

                MessageBox.Show(this, e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HabilitaDesabilitaTela();
            }
        }

        private void btnListarArquivos_Click(object sender, EventArgs e)
        {
            Atualizalista();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                HabilitaDesabilitaTela(false);

                if (dataGridView1.SelectedCells.Count < 1)
                    return;

                var fileId = dataGridView1.SelectedCells[0].Value.ToString();

                var response = new BasConceptFileUploadClient().GetFileInfo(fileId);

                lblFileDetailId.Text = response.Data.Id;
                lblFileDetailName.Text = response.Data.Name;
                lblFileDetailMime.Text = response.Data.MimeType;
            }
            catch (Exception ex)
            {
                lblFileDetailId.Text = "";
                lblFileDetailName.Text = "";
                lblFileDetailMime.Text = "";

                MessageBox.Show(this, ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HabilitaDesabilitaTela();
            }
        }
    }
}
