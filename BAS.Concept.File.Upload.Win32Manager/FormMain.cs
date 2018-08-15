﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAS.Concept.File.Upload.Client;
using System.IO;

namespace BAS.Concept.File.Upload.Win32Manager
{
    public partial class FormMain : Form
    {
        private readonly BasConceptFileUploadClient _client;

        public FormMain()
        {
            _client = new BasConceptFileUploadClient(
#if DEBUG
                new ClientOptions
                {
                    Url = "http://localhost:3000"
                }
#endif
            );

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

                var response = _client.GetAllFiles();

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

                var response = _client.GetFileInfo(fileId);

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

        private void btnEnviarArquivo_Click(object sender, EventArgs e)
        {
            EnviarArquivo();
        }

        private void EnviarArquivo()
        {
            var dResult = openFileDialog1.ShowDialog(this);

            if (dResult == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;
                var fileName = Path.GetFileName(filePath);

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var webResult = _client.SendFile(fileStream, fileName);
                }

                Atualizalista();
            }
        }
    }
}
