using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace formulariosimples
{
    public partial class frmFormularioSimples : Form
    {
        MySqlConnection Conexao;
        string data_source = "datasource=localhost; username=root; password=; database=formulario";

        

        public frmFormularioSimples()
        {
            InitializeComponent();

            lstCliente.View = View.Details;             
            lstCliente.LabelEdit = true;                
            lstCliente.AllowColumnReorder = true;       
            lstCliente.FullRowSelect = true;            
            lstCliente.GridLines = true;

            lstCliente.Columns.Add("Numero Cadastro", 100, HorizontalAlignment.Left);
            lstCliente.Columns.Add("Nome", 100, HorizontalAlignment.Left);
            lstCliente.Columns.Add("Cidade", 100, HorizontalAlignment.Left);


            //Carrega os dados do cliente na interface
            carregar_clientes();
        }




        private void carregar_clientes_com_query(string query)
        {
            try
            {
                //Cria a conexão com o banco de dados
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                //Executa a consulta SQL fornecida
                MySqlCommand cmd = new MySqlCommand(query, Conexao);


                //Se a consulta contém o parâmetro @q, adiciona o valor da caixa de pesquisa
                if (query.Contains("@q"))
                {
                    cmd.Parameters.AddWithValue("@q", "%" + txtBuscar.Text + "%");
                }

                //Executa o comando e obtém os resultados 
                MySqlDataReader reader = cmd.ExecuteReader();

                //Limpa os itens existentes no ListView antes de adicionar novos
                lstCliente.Items.Clear();


                //Preenche o ListView com os dados dos clientes
                while (reader.Read())
                {
                    //Cria uma linha para cada cliente com os dados retornados da consulta
                    string[] row =
                    {
                        Convert.ToString(reader.GetInt32(0)), //Número Cadastro
                        reader.GetString(1),
                        reader.GetString(3),
                        
                    };


                    //Adiciona a linha ao ListView         
                    lstCliente.Items.Add(new ListViewItem(row));

                }
            }


            catch (MySqlException ex)
            {
                //Tratar erros relacionados ao MySQL
                MessageBox.Show("Erro " + ex.Number + "ocorreu: " + ex.Message,
                      "Erro",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);

            }

            catch (Exception ex)
            {

                //Trata outros tipos de erro
                MessageBox.Show("Ocorreu: " + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


            }
            finally
            {
                //Garante que a conexão com o banco será fechada, mesmo se ocorrer erro
                if (Conexao != null && Conexao.State == ConnectionState.Open)
                {
                    Conexao.Close();


                }


            }

        }


        private void carregar_clientes()
        {
            string query = "SELECT * FROM cadastro_form ORDER BY numerocadastro DESC";
            carregar_clientes_com_query(query);
        }


        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            int numeroCadastro;
            string nomeUsuario;
            DateTime dataNascimento;
            string cidade;
            bool generoF;
            bool generoM;
            bool generoNB;

            //Validação de campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtNumeroCadastro.Text)) 
            {
                MessageBox.Show("Por favor, preencha o número de cadastro.");
                return; //Interrompe a execuçao do código caso o campo esteja vazio
            }

            if (string.IsNullOrWhiteSpace(txtNomeCompleto.Text)) 
            {
                MessageBox.Show("Por favor, preencha o nome completo.");
                return;
            }

            // Validação da data de nascimento usando DateTimePicker
            dataNascimento = dateTimePicker1.Value.Date;

            //Verificar se a data é posterior ou igual á data atual
            if (dataNascimento >= DateTime.Now.Date) //Compara com a data atual sem hora
            {
                MessageBox.Show("Verifique novamente a sua data de nascimento.");
                return;
            }

            if (comboBoxCidade.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione a idade.");
                return;
            }

            if (!rbFeminino.Checked && !rbMasculino.Checked && !rbNaoBinario.Checked)
            {
                MessageBox.Show("Por favor. selecione o genero");
                return;
            }


            //Agora, caso todos os campos esteja prenchido, a validaçaõ prossegue
            numeroCadastro = Convert.ToInt32(txtNumeroCadastro.Text);
            nomeUsuario = txtNomeCompleto.Text;
            cidade = comboBoxCidade.Text;
            generoF = rbFeminino.Checked;
            generoM = rbMasculino.Checked;
            generoNB = rbNaoBinario.Checked;


            //Formatar a data para exibir apenas a data (sem hora)
            string dataFormatada = dataNascimento.ToString("dd/MM/yyyy");

            //Determinar o gênero selecionado

            string generoselecionado = "Não informado"; //Caso nenhum gênero seja selecionado
            if (generoF)
                generoselecionado = "Feminino";
            else if (generoM)
                generoselecionado = "Masculino";
            else if (generoNB)
                generoselecionado = "Não Binário";

            //Exibir as informações em MessageBox
            //MessageBox.Show("Número Cadastro:" + numeroCadastro);
           // MessageBox.Show("Nome:" + nomeUsuario);
           // MessageBox.Show("Data Nascimento:" + dataFormatada);
          //  MessageBox.Show("Cidade:" + cidade);
           // MessageBox.Show("Genero: " + generoselecionado);

            try
            { 
                //Criando a conexão com o banco
                 Conexao = new MySqlConnection(data_source);
                 Conexao.Open();

                 MessageBox.Show("Conexão aberta com sucesso!");


                 //Comando SQL para cadastrar um novo cliente no Banco
                  MySqlCommand cmd = new MySqlCommand
                      {
                        Connection = Conexao
                      };

                 cmd.Prepare();


                string generoSelecionado = "";

                if (rbFeminino.Checked)
                {
                    generoSelecionado = "Feminino";
                }
                else if (rbMasculino.Checked)
                {
                    generoSelecionado = "Masculino";
                }
                else if (rbNaoBinario.Checked)
                {
                    generoSelecionado = "Não Binário";
                }



                int? numCadastro = null;
                if (numCadastro == null)
                {

                    cmd.CommandText = "INSERT INTO cadastro_form(numerocadastro,nomeusuario,datanasc,cidade,generoselecionado) " +
                        "VALUES (@numerocadastro,@nomeusuario,@datanasc,@cidade,@generoselecionado)";


                    cmd.Parameters.AddWithValue("@numerocadastro", txtNumeroCadastro.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomeusuario", txtNomeCompleto.Text.Trim());
                    cmd.Parameters.AddWithValue("@datanasc", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@cidade", comboBoxCidade.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@generoselecionado", generoSelecionado);

                    cmd.ExecuteNonQuery();


                    //Mensagem de sucesso
                    MessageBox.Show("Cliente cadastrado com sucesso! ",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }


            catch (MySqlException ex)
            {
                //Tratar erros relacionados ao MySQL
                MessageBox.Show("Erro " + ex.Number + "ocorreu: " + ex.Message,
                      "Erro",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);

            }

            catch (Exception ex)
            {

                //Trata outros tipos de erro
                MessageBox.Show("Ocorreu: " + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);


            }
            finally
            {
                //Garante que a conexão com o banco será fechada, mesmo se ocorrer erro
                if (Conexao != null && Conexao.State == ConnectionState.Open)
                {
                    Conexao.Close();


                }


            }

        }

        private void txtNumeroCadastro_Click(object sender, EventArgs e)
        {
            //Limpa o conteúdo do textBox quando o usuário clicar nele
           if (txtNumeroCadastro.Text == "Número Cadastro")
            {
                txtNumeroCadastro.Text = "";
            }
            
         
                
        }

        private void txtNomeCompleto_Click(object sender, EventArgs e)
        {
            //Limpa o conteúdo do TextBox quando o usuário  clicar nele
            if (txtNomeCompleto.Text == "insira o seu nome completo")
            {
                txtNomeCompleto.Text = "";
            }
        }

        private void lstCliente_Click(object sender, EventArgs e)
        {
         
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM cadastro_form WHERE nomeusuario LIKE @q OR cidade LIKE @q ORDER BY numerocadastro DESC";
            carregar_clientes_com_query(query);
        }
    }
}
