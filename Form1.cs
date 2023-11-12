using MySql.Data.MySqlClient;

namespace Workshop1._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetDatabaseOnStartup();
            if (CheckUserAgreement() == true)
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;
            }
        }

        internal static string? selectedBase;
        string connString = "";

        //----- Send login parameters to sql server and confirm that we have access to db -----//
        public void SendLoginDataAndLogin()
        {
            string connStr1 = $"server=78.153.44.150;user={username_box.Text};database=WorkshopRanaDB;port=52079;password={password_box.Text}";
            string connStr2 = $"server=192.168.0.14;user={username_box.Text};database=WorkshopRanaDB;port=3306;password={password_box.Text}";

            if (selectedBase == "Lokalna")
            {
                connString = connStr2;

            }
            else if (selectedBase == "Online")
            {
                connString = connStr1;
            }


            MySqlConnection con = new MySqlConnection(connString);
            try
            {
                con.Open();
                MessageBox.Show("Uspešna prijava!");
                Main main = new Main();
                main.Show();
                this.Hide();

            }
            catch (Exception)
            {
                MessageBox.Show("Greška pri povezivanju na bazu!");
                //  MessageBox.Show(ex.ToString());
            }
            con.Close();


        }

        //----- Login button -----//
        private void login_button_Click(object sender, EventArgs e)
        {
            SendLoginDataAndLogin();
        }
        public bool CheckUserAgreement()
        {
            if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\Rights.txt"))
            {
                // Read file using StreamReader. Reads file line by line
                using (StreamReader file = new StreamReader(System.Windows.Forms.Application.StartupPath + "\\Rights.txt"))
                {
                    string? ln;
                    string? licenceStatus = null;

                    while ((ln = file.ReadLine()) != null)
                    {
                        if (ln == "Licence valid:true")
                        {
                            file.Close();
                            button1.Enabled = true;
                            return true;
                        }
                        licenceStatus = "NOT VALID";
                        button1.Enabled = false;
                        button3.Enabled = false;
                    }
                    file.Close();
                    MessageBox.Show("Please contact your administrator to update licence!", $"Licence: {licenceStatus}.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;
                MessageBox.Show("Please contact your administrator to update licence!", $"Licence: NO LICENCE FOUND.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        //----- Info button -----//
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(System.IO.File.ReadAllText(System.Windows.Forms.Application.StartupPath + "\\EULA.txt"), "Confirm Eula", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DateTime dt1 = DateTime.Parse("11/11/2023 05:00:00");
                DateTime dt2 = DateTime.Parse("12/11/2024 19:00:00");
                DateTime dt3 = DateTime.Now;
                string? licenceValid;
                List<string> parameters = new List<string>();

                parameters.Add("Program created by: Dušan Stevanoviæ");
                parameters.Add("Workshop1.1 ver 1.0");
                parameters.Add("Licenced by Dušan Stevanoviæ");
                parameters.Add("ALURWORKSHOP1.1");
                parameters.Add($"Licenced date: {dt1}");
                parameters.Add($"Licenced until: {dt2}");

                if (dt3 > dt2)
                {
                    licenceValid = "Licence valid:false";
                }
                else licenceValid = "Licence valid:true";

                parameters.Add(licenceValid);

                File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Rights.txt", parameters);

                if (CheckUserAgreement() == true)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    button3.Enabled = false;
                }
            }
            else
            {
                // user disagreed
                System.Windows.Forms.Application.Exit();
            }
        }

        //----- Settings button -----//
        private void button3_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }
        private void SetDatabaseOnStartup()
        {
            Form1.selectedBase = "Lokalna";
            Main.selectedBase = "Lokalna";
        }
    }
}