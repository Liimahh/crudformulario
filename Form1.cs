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
            lstCliente.Columns.Add("Data de Nascimento", 120, HorizontalAlignment.Left);
            lstCliente.Columns.Add("Cidade", 100, HorizontalAlignment.Left);
            lstCliente.Columns.Add("Gênero", 100, HorizontalAlignment.Left);


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


                //Teste para cadastrar a data sem quebrar
                DateTime? dataNasc = null;
                // Preenche o ListView com os dados dos clientes
                while (reader.Read())
                {
                    // Inicializa a variável de data com "Data inválida"
                    string dataFormatada = "Data inválida";

                    // Se a data não for nula, obtém e formata a data de nascimento
                    if (!reader.IsDBNull(2))
                    {
                        DateTime dataNascimento = reader.GetDateTime(2);
                        dataFormatada = dataNascimento.ToString("dd/MM/yyyy"); // Formata a data
                    }

                    // Monta a linha a ser exibida no ListView
                    string[] row =
                    {
            Convert.ToString(reader.GetInt32(0)),  // Número Cadastro
            reader.GetString(1),                   // Nome
            dataFormatada,                         // Data de nascimento
            reader.GetString(3),                   // Cidade
            reader.GetString(4)                    // Gênero
        };

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

            if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text))
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
            nomeUsuario = txtNomeUsuario.Text;
            cidade = comboBoxCidade.Text;
            generoF = rbFeminino.Checked;
            generoM = rbMasculino.Checked;
            generoNB = rbNaoBinario.Checked;



            //Determinar o gênero selecionado

                  //VARIÁVEIS DESNECESSÁRIAS(chatgpt)
                 //string generoselecionado = "Não informado"; //Caso nenhum gênero seja selecionado
                 //if (generoF)
                 //generoselecionado = "Feminino";
                 //else if (generoM)
                 //generoselecionado = "Masculino";
                 //else if (generoNB)
                 //generoselecionado = "Não Binário";


            // Preenche o campo de gênero
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


                // Verifica se o registro já existe
                MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM cadastro_form WHERE numerocadastro = @numerocadastro", Conexao);
                checkCmd.Parameters.AddWithValue("@numerocadastro", numeroCadastro);
                int recordExists = Convert.ToInt32(checkCmd.ExecuteScalar());


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



                if (recordExists == 0)
                {
                    // INSERIR
                    cmd.CommandText = "INSERT INTO cadastro_form(numerocadastro,nomeusuario,datanasc,cidade,generoselecionado) " +
                        "VALUES (@numerocadastro,@nomeusuario,@datanasc,@cidade,@generoselecionado)";

                    cmd.Parameters.AddWithValue("@numerocadastro", numeroCadastro);
                    cmd.Parameters.AddWithValue("@nomeusuario", nomeUsuario);
                    cmd.Parameters.AddWithValue("@datanasc", dataNascimento);
                    cmd.Parameters.AddWithValue("@cidade", cidade);
                    cmd.Parameters.AddWithValue("@generoselecionado", generoSelecionado);

                    cmd.ExecuteNonQuery();


                    //Mensagem de sucesso
                    MessageBox.Show("Cliente cadastrado com sucesso! ",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    // UPDATE
                    cmd.CommandText = "UPDATE cadastro_form SET " +
                         "nomeusuario = @nomeusuario, " +
                         "datanasc = @datanasc, " +
                         "cidade = @cidade, " +
                         "generoselecionado = @generoselecionado " +
                         "WHERE numerocadastro = @numerocadastro";

                    cmd.Parameters.AddWithValue("@numerocadastro", numeroCadastro);
                    cmd.Parameters.AddWithValue("@nomeusuario", nomeUsuario);
                    cmd.Parameters.AddWithValue("@datanasc", dataNascimento);
                    cmd.Parameters.AddWithValue("@cidade", cidade);
                    cmd.Parameters.AddWithValue("@generoselecionado", generoSelecionado);



                    //executa comando 

                    cmd.ExecuteNonQuery();


                    //MENSAGEM DE SUCESSO PARA DADOS ATUALIZADOS

                    MessageBox.Show($"Os dados do cliente {numeroCadastro} foram alterados com sucesso!",
                                    "Sucesso",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                }

                

                // Limpa os campos
                txtNomeUsuario.Text = string.Empty;
                txtNumeroCadastro.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                comboBoxCidade.SelectedItem = null;
                rbFeminino.Checked = false;
                rbMasculino.Checked = false;
                rbNaoBinario.Checked = false;



                //Recarrega os clientes na ListView
                carregar_clientes();

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
            if (txtNomeUsuario.Text == "insira o seu nome completo")
            {
                txtNomeUsuario.Text = "";
            }
        }


        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM cadastro_form WHERE nomeusuario LIKE @q OR cidade LIKE @q ORDER BY numerocadastro DESC";
            carregar_clientes_com_query(query);
        }


        // Evento para capturar o item selecionado na ListView

        private void lstCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstCliente.SelectedItems.Count > 0)
            {
                // Captura o item selecionado
                ListViewItem selectedItem = lstCliente.SelectedItems[0];

                // Preenche os campos do formulário com os dados do item selecionado
                txtNumeroCadastro.Text = selectedItem.SubItems[0].Text;
                txtNomeUsuario.Text = selectedItem.SubItems[1].Text;


                if (selectedItem.SubItems.Count > 3)
                {
                    comboBoxCidade.SelectedItem = selectedItem.SubItems[3].Text;
                }


                if (selectedItem.SubItems.Count > 4)
                {

                    string genero = selectedItem.SubItems[4].Text;
                    if (genero == "Feminino")
                    {
                        rbFeminino.Checked = true;
                    }
                    else if (genero == "Masculino")
                    {
                        rbMasculino.Checked = true;
                    }
                    else if (genero == "Não Binário")
                    {
                        rbNaoBinario.Checked = true;
                    }
                }
                
            }
        }

        private void excluir_cliente(string numerocadastro) 
        {
            try
            {
                DialogResult opcaoDigitada = MessageBox.Show(
                    "Tem certeza que deseja Excluir os dados desse cliente?",
                    "Excluir dados",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (opcaoDigitada == DialogResult.Yes)
                {
                    using (MySqlConnection conexao = new MySqlConnection(data_source))
                    {
                        conexao.Open();

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conexao;
                        cmd.CommandText = "DELETE FROM cadastro_form WHERE numerocadastro = @numerocadastro";
                        cmd.Parameters.AddWithValue("@numerocadastro", numerocadastro);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Os dados do cliente foram EXCLUÍDOS!",
                                            "Sucesso",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            carregar_clientes(); // Recarrega a ListView
                            limpar_campos();
                        }
                    }
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

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (lstCliente.SelectedItems.Count == 0)
            {
                MessageBox.Show("Por favor, selecione um cliente.");
                return;
            }

            string numerocadastroSelecionado = lstCliente.SelectedItems[0].SubItems[0].Text;

            excluir_cliente(numerocadastroSelecionado);
        }

       private void limpar_campos()
        {
            txtNumeroCadastro.Text = "";
            txtNomeUsuario.Text = "";
            comboBoxCidade.SelectedItem = null;
            rbFeminino.Checked = false;
            rbMasculino.Checked = false;
            rbNaoBinario.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
        }
    }
   
}



