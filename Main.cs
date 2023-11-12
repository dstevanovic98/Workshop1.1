using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Configuration;

namespace Workshop1._1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            CheckSelectedDatabase();
        }

        public readonly DateTime dateTime = DateTime.Now;
        public readonly TimeOnly timeOnly = TimeOnly.FromDateTime(DateTime.Now);
        public readonly DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
        internal static string? selectedBase;
        private string connString = "";
        private string? uniqueID;

        //------------------------------------------ START MENU PANEL -------------------------------//

        private void button1_Click(object sender, EventArgs e)
        {
            prodaja_panel.Visible = true;
            pretraga_panel.Visible = false;
            racuni_panel.Visible = false;
            zakazivanje_panel.Visible = false;
            Task task = FillUserlistViewAutomatically();
            Task task1 = FillServicesAutomatically();

        }

        private void pretraga_button_Click(object sender, EventArgs e)
        {
            prodaja_panel.Visible = false;
            racuni_panel.Visible = false;
            zakazivanje_panel.Visible = false;
            pretraga_panel.Visible = true;
            CreateLabelsForAdvices();
        }

        private void racuni_button_Click(object sender, EventArgs e)
        {
            prodaja_panel.Visible = false;
            pretraga_panel.Visible = false;
            zakazivanje_panel.Visible = false;
            racuni_panel.Visible = true;

        }

        private void zakazivanje_button_Click(object sender, EventArgs e)
        {
            prodaja_panel.Visible = false;
            pretraga_panel.Visible = false;
            racuni_panel.Visible = false;
            zakazivanje_panel.Visible = true;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            prodaja_panel.Visible = true;
            pretraga_panel.Visible = false;
            racuni_panel.Visible = false;
            zakazivanje_panel.Visible = false;
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Zatvorimo aplikaciju?", "Izlaz iz aplikacije", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                this.Activate();
            }
        }

        //------------------------------------------ END MENU PANEL ---------------------------------//
        /// <summary>
        /// Provera odabrane baze iz prozora podešavanja. Ukoliko nema izabrane, automatski je lokalna baza izabrana. 
        /// </summary>
        private void CheckSelectedDatabase()
        {
            if (selectedBase == "Lokalna")
            {
                connString = "WorkshopRanaDBHome";

            }
            else if (selectedBase == "Online")
            {
                connString = "WorkshopRanaDB";
            }
        }

        //------------------------------------------ START PRODAJA PANEL ----------------------------//

        // ----- Search users part! -------//

        public readonly List<User> users = new List<User>();
        public async Task FillUserlistViewAutomatically()
        {
            listView2.Items.Clear();
            await GetUsers("users");
            try
            {
                foreach (var user in users)
                {
                    ListViewItem showUser = new ListViewItem(user.User_id);
                    showUser.SubItems.Add(user.FirstName);
                    showUser.SubItems.Add(user.LastName);
                    showUser.SubItems.Add(user.Address);
                    showUser.SubItems.Add(user.Phone);

                    listView2.Items.Add(showUser);
                }
            }

            catch { }
        }
        public async Task GetUsers(string selectArray)
        {
            await Task.Run(() =>
            {
                users.Clear();
                string comm = "SELECT user_id, firstname, lastname, email, location, phone\r\nFROM WorkshopRanaDB.users";
                MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
                MySqlCommand cmd = new MySqlCommand(comm, con);
                try
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string User_id = reader.GetString("user_id");
                        string FirstName = reader.GetString("firstname");
                        string LastName = reader.GetString("lastname");
                        string Address = reader.GetString("location");
                        string Phone = reader.GetString("phone");

                        if (FirstName != "" && LastName != "" && User_id != "")
                        {
                            // ------------------- Create new user!  ------------------- //
                            User user = new User();
                            user.FirstName = FirstName;
                            user.LastName = LastName;
                            user.Address = Address;
                            user.User_id = User_id;
                            user.Phone = Phone;

                            if (selectArray == "users")
                            {
                                users.Add(user);
                            }
                            else users2.Add(user);

                        }

                    }
                }
                catch { }
                finally { con.Close(); }
            });
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            users.Clear();
            string comm = $"SELECT * FROM users where firstname='{comboBox1.Text}'";
            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(comm, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string User_id = reader.GetString("user_id");
                    string FirstName = reader.GetString("firstname");
                    string LastName = reader.GetString("lastname");
                    string Address = reader.GetString("location");
                    string Phone = reader.GetString("phone");

                    if (FirstName != "" && Address != "" && LastName != "")
                    {
                        // ------------------- Create new user!  ------------------- //
                        User user = new User();
                        user.FirstName = FirstName;
                        user.LastName = LastName;
                        user.Address = Address;
                        user.User_id = User_id;
                        user.Phone = Phone;

                        users.Add(user);
                    }

                }
            }
            catch { }
            finally
            {
                con.Close();
                listView2.Items.Clear();
                foreach (var user in users)
                {
                    ListViewItem showUser = new ListViewItem(user.User_id);
                    showUser.SubItems.Add(user.FirstName);
                    showUser.SubItems.Add(user.LastName);
                    showUser.SubItems.Add(user.Address);
                    showUser.SubItems.Add(user.Phone);

                    listView2.Items.Add(showUser);
                }

            }

        }


        // ----- Search service part! -------//
        public readonly List<Service> services = new List<Service>();

        public async Task GetServices(string selectArray)
        {
            await Task.Run(() =>
            {
                services.Clear();
                string comm = "SELECT name, service_id, description, value\r\nFROM WorkshopRanaDB.services";
                MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
                MySqlCommand cmd = new MySqlCommand(comm, con);
                try
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string Service_id = reader.GetString("service_id");
                        string Service_name = reader.GetString("name");
                        string Service_description = reader.GetString("description");
                        string Service_value = reader.GetString("value");

                        if (Service_id != "" && Service_name != "" && Service_name != "")
                        {
                            // ------------------- Create new service!  ------------------- //
                            Service service = new Service();
                            service.Name = Service_name;
                            service.Id = Service_id;
                            service.Description = Service_description;
                            service.Value = Service_value;

                            if (selectArray == "service")
                            {
                                services.Add(service);
                            }
                            else services2.Add(service);

                        }

                    }
                }
                catch { }
                finally
                {
                    con.Close();
                }

            });


        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            services.Clear();
            listView1.Items.Clear();
            string comm = $"SELECT * FROM services where name='{comboBox2.Text}'";
            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(comm, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string Service_id = reader.GetString("service_id");
                    string Service_name = reader.GetString("name");
                    string Service_description = reader.GetString("description");
                    string Service_value = reader.GetString("value");

                    if (Service_id != "" && Service_name != "" && Service_name != "")
                    {
                        // ------------------- Create new user!  ------------------- //
                        Service service = new Service();
                        service.Name = Service_name;
                        service.Id = Service_id;
                        service.Description = Service_description;
                        service.Value = Service_value;

                        services.Add(service);
                    }

                }
            }
            catch { }
            finally
            {
                con.Close();
                pdlistView.Items.Clear();
                foreach (var service in services)
                {
                    ListViewItem showService = new ListViewItem(service.Id);
                    showService.SubItems.Add(service.Name);
                    showService.SubItems.Add(service.Value);
                    showService.SubItems.Add(service.Description);

                    listView1.Items.Add(showService);
                }
            }
        }

        public async Task FillServicesAutomatically()
        {
            listView1.Items.Clear();
            services.Clear();
            await GetServices("service");

            try
            {
                foreach (var service in services)
                {
                    ListViewItem showService = new ListViewItem(service.Id);
                    showService.SubItems.Add(service.Name);
                    showService.SubItems.Add(service.Value);
                    showService.SubItems.Add(service.Description);

                    listView1.Items.Add(showService);
                }
            }
            catch { }
        }

        // Promenjiva za konačnu cenu. Prikazivanje cene u aplikaciji. 
        float final_price = 0;

        // Funkcija za kreiranje jedinstvenog broja računa. 
        private string GenerateBillUniqueKey()
        {
            int rows = 0;
            string sql = $"SELECT * FROM WorkshopRanaDB.bills WHERE date='{dateTime}'";
            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows) rows++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            string id = $"ALUR-{dateTime.Year}-{rows + 1}";


            return id;
        }

        // Funkcija za proveravanje validnosti računa. 
        // Treba proveravati da li račun već postoji. Ukoliko postoji, mora napisati da taj račun već postoji! 
        public bool CheckBillValidation()
        {
            int rows = 0;
            Bill bill1 = new Bill();
            bill1.Salesman_id = "rana";
            bill1.User_id = listView2.SelectedItems[0].Text;
            bill1.Value = final_price;
            bill1.Id = $"{listView2.SelectedItems[0].Text}-{dateOnly.ToString()}-{final_price.ToString()}";
            bill1.Date = dateTime.ToString("yyyy-MM-dd HH-mm-ss");
            bill1.Number = GenerateBillUniqueKey();

            uniqueID = bill1.Number;

            string sql = $"SELECT * FROM WorkshopRanaDB.bills WHERE (customer_id='{bill1.User_id}'AND salesman_id ='{bill1.Salesman_id}' AND total_price='{bill1.Value}' AND bill_id='{bill1.Id}' AND bill_number='{bill1.Number}')";

            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows) rows++;
                }

                if (rows > 0)
                {
                    MessageBox.Show("Greška!", "Takav račun već postoji!");
                    return false;
                }
                else
                {
                    MessageBox.Show("Uspešno!", "Ne postoji takav račun!!");
                    con.Close();
                    return true;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        // Funkcija za slanje računa u bazu.
        public void SendBillToDatabase()
        {
            if (CheckBillValidation() == true)
            {
                using (MySqlConnection con = new MySqlConnection(User.CnnVal(connString)))
                {
                    try
                    {
                        Bill bill1 = new Bill();
                        bill1.Salesman_id = "rana";
                        bill1.User_id = listView2.SelectedItems[0].Text;
                        bill1.Value = final_price;
                        bill1.Id = $"{listView2.SelectedItems[0].Text}-{dateOnly.ToString()}-{final_price.ToString()}";
                        bill1.Date = dateTime.ToString("yyyy-MM-dd HH-mm-ss");
                        bill1.Number = uniqueID;


                        string sql = $"INSERT INTO WorkshopRanaDB.bills VALUES('{bill1.Date}', '{bill1.User_id}', '{bill1.Salesman_id}', '{bill1.Value}', '{bill1.Number}', '{bill1.Id}')";

                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand(sql, con))
                        {
                            int rowsAdded = cmd.ExecuteNonQuery();

                            if (rowsAdded > 0)
                                MessageBox.Show("Račun uspešno poslan u bazu!!");
                            else
                                // Well this should never really happen
                                MessageBox.Show("Greška!!");
                        }
                        con.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

        }

        // Funkcija dodavanja artikla u korpu na klik artikla sa liste. 
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ukoliko imamo izabranog elementa, izabrani element dodamo u korpu (listView5). 
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem != null)
                {
                    MessageBox.Show(selectedItem.SubItems[1].Text, "Uspešno dodano u korpu!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Kreiramo novu listu za prikazivanje artikla/usluge.
                    ListViewItem billItems = new ListViewItem(selectedItem.SubItems[0].Text);
                    billItems.SubItems.Add(selectedItem.SubItems[0]);
                    billItems.SubItems.Add(selectedItem.SubItems[1]);
                    billItems.SubItems.Add(selectedItem.SubItems[2]);
                    billItems.SubItems.Add(selectedItem.SubItems[3]);
                    listView5.Items.Add(billItems);

                    // Cenu iz string vr. pretvorimo u int vrednost.
                    var current_item_price = Int32.Parse(selectedItem.SubItems[2].Text);

                    // Iznos usluge/artikla dodamo u ukupan iznos.
                    final_price = final_price + current_item_price;

                    // Prikaz iznosa sa valutom u aplikaciji. 
                    label7.Text = $"{final_price.ToString()} din";
                }
            }
            else
            {
                return;
            }

        }
        // Brisanje svih artikla/usluga iz korpe.
        private void button4_Click(object sender, EventArgs e)
        {
            listView5.Items.Clear();
            final_price = 0;
            label7.Text = string.Empty;
        }

        // Odstranjivanje zadnjeg elementa iz korpe.
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView5.Items.Count > 0)
            {
                listView5.Items.RemoveAt(listView5.Items.Count - 1);
                final_price = 0;
                foreach (ListViewItem item in listView5.Items)
                {
                    //---- MAKE FINAL PRICE ----//
                    var current_item_price = Int32.Parse(item.SubItems[3].Text);
                    final_price = final_price + current_item_price;
                }
                label7.Text = $"{final_price.ToString()} din";
            }
        }

        // Generiranje Excel fajla, namensko ostavljeno za predračune.
        private void button7_Click(object sender, EventArgs e)
        {
            if (listView5.Items.Count == 0)
            {
                MessageBox.Show("Na računu nemate dodatih usluga!", "Upozorenje!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (listView2.SelectedItems.Count < 1)
            {
                MessageBox.Show("Morate izabrati stranku! ", "Upozorenje! ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                //--- select user add to bill --//
                var selectedItem = listView2.SelectedItems[0];
                var selectedItemName = listView2.SelectedItems[0].Text;

                //-- Information about licence, code must containt that line! ---//
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                //---- Create info about file name ---// 
                string ExportFileName = $"račun'{dateTime.ToString("dd-MM-yyyy HH-mm-ss")}'{selectedItemName}'.xlsx";
                var newFile = new FileInfo(ExportFileName);

                //--- Start to create new file ---//
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //--- Add new sheet --//
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Račun");

                    //-- Add customer info into one string ---//
                    string CustomerInfo = $"{selectedItem.SubItems[1].Text} {selectedItem.SubItems[2].Text}, {selectedItem.SubItems[3].Text}";


                    //----- Add customer to Excel ----//
                    ws.Cells[1, 1].Value = selectedItem.SubItems[0].Text;
                    ws.Cells["H3"].Value = dateTime.ToString();
                    ws.Cells[1, 2].Value = CustomerInfo;
                    int scel = 12;


                    // -- Add basket items to Excel --//
                    for (int i = 0; i < listView5.Items.Count; ++i)
                    {
                        ws.Cells[scel, 1].Value = listView5.Items[i].SubItems[1].Text;
                        ws.Cells[scel, 3].Value = listView5.Items[i].SubItems[2].Text;
                        ws.Cells[scel, 5].Value = listView5.Items[i].SubItems[3].Text;
                        ws.Cells[scel, 6].Value = listView5.Items[i].SubItems[4].Text;
                        scel++;
                    }

                    //---- Design xlms---//
                    ws.Cells["A1:K1"].Style.Font.Italic = true;
                    ws.Cells["A12:F20"].Style.Font.Italic = true;
                    //  ws.Cells["A1:K1"].AutoFitColumns();
                    ws.Cells["A11:I11"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[$"A{scel + 2}:I{scel + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    //ws.Cells["A12:F20"].AutoFitColumns();
                    //ws.Column(1).Width = 15;
                    //ws.Column(2).Width = 20;
                    //ws.Column(3).Width = 10;
                    //ws.Column(4).Width = 40;
                    // using (ExcelRange range = ws.Cells["A12:D20"]) 
                    //  {
                    //     range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    //     range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    //     range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //     range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    //     range.Style.Font.Size = 10;
                    //     //range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    // }


                    //--- Add new sheet ---//
                    // ExcelWorksheet ws1 = xlPackage.Workbook.Worksheets.Add("Informacije");

                    // Save file
                    xlPackage.Save();
                    MessageBox.Show("Uspešno!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Fiskalno štampanje 
        private void button5_Click(object sender, EventArgs e)
        {
            // Pošaljemo račun 
            SendBillToDatabase();
            // Obrišemo dodate artikle iz korpe. Napravimo praznu korpu. 
            listView5.Clear();
        }

        //------------------------------------------ END PRODAJA PANEL ---------------------------------//

        //------------------------------------------ START SEARCH/ADD PANEL ---------------------------------//

        // ---------------------------------- Provera da li usluga ili stranka već postoje u bazi --------------------------------------------//
        public bool CheckIfUserExist()
        {
            int rows = 0;
            string sql = $"SELECT* FROM WorkshopRanaDB.users WHERE user_id='{id_textBox.Text}'";
            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows) rows++;
                }

                if (rows > 0)
                {
                    MessageBox.Show("Greška!", "Takva stranka već postoji!");
                    return false;
                }
                else
                {
                    MessageBox.Show("Uspešno!", "Ne postoji takva stranka!!");
                    con.Close();
                    return true;
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        // ----------------------------------Kreiraj novog korisnika/uslugu --------------------------------------------//
        // Kreiraj novog korisnika. 
        private void create_new_user_button_Click(object sender, EventArgs e)
        {
            // Proverimo validnost unosa
            if (name_textBox.Text != "" && surrname_textBox.Text != "" && id_textBox.Text != "")
            {
                if (CheckIfUserExist() == true)
                {
                    string sql = $"INSERT INTO WorkshopRanaDB.users\r\n(user_id, firstname, lastname, email, location, phone)\r\nVALUES('{id_textBox.Text.ToUpper()}', '{name_textBox.Text}', '{surrname_textBox.Text}', '{email_textBox.Text}', '{location_textBox.Text}', '{phone_textBox.Text}')";
                    MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
                    try
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(sql, con))
                        {
                            int rowsAdded = cmd.ExecuteNonQuery();

                            if (rowsAdded > 0)
                            {
                                MessageBox.Show("Stranka uspešno dodana u bazu!!");
                                name_textBox.Text = string.Empty;
                                surrname_textBox.Text = string.Empty;
                                id_textBox.Text = string.Empty;
                                email_textBox.Text = string.Empty;
                                location_textBox.Text = string.Empty;
                                phone_textBox.Text = string.Empty;
                            }
                            else
                                // Well this should never really happen
                                MessageBox.Show("Greška pri slanju u bazu!!");
                        }
                        con.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                MessageBox.Show("Morate uneti sve parametre!", "Greška!");
                return;
            }



        }

        // Kreiraj novu uslugu. 
        private void create_new_service_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nije implementirano!!");
        }

        // ---------------------------------- Pregled svih korisnika iz baze --------------------------------------------//
        private async void all_users_button_Click(object sender, EventArgs e)
        {
            users2.Clear();
            listView4.Items.Clear();
            await GetUsers("users2");
            try
            {
                foreach (var user in users2)
                {
                    ListViewItem showUser = new ListViewItem(user.User_id);
                    showUser.SubItems.Add(user.FirstName);
                    showUser.SubItems.Add(user.LastName);
                    showUser.SubItems.Add(user.Address);
                    showUser.SubItems.Add(user.Phone);

                    listView4.Items.Add(showUser);
                }
            }

            catch { }

        }
        // Napravimo listu korisnika koja će biti u funkciji na "Pretraga/Dodajanje" kartici.
        public readonly List<User> users2 = new List<User>();

        // Na unos imena se pretraži stranka u bazi. Koju dodamo u listu "users2". 
        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            listView4.Items.Clear();
            users2.Clear();
            string comm = $"SELECT * FROM users where firstname='{comboBox3.Text}'";
            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(comm, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string User_id = reader.GetString("user_id");
                    string FirstName = reader.GetString("firstname");
                    string LastName = reader.GetString("lastname");
                    string Address = reader.GetString("location");
                    string Phone = reader.GetString("phone");
                    string Email = reader.GetString("email");

                    if (FirstName != "" && Address != "" && LastName != "")
                    {
                        // ------------------- Create new user!  ------------------- //
                        User user = new User();
                        user.FirstName = FirstName;
                        user.LastName = LastName;
                        user.Address = Address;
                        user.User_id = User_id;
                        user.Phone = Phone;
                        user.Email = Email;
                        users2.Add(user);
                    }

                }
            }
            catch { }
            finally
            {
                con.Close();
                listView4.Items.Clear();

                // Iz liste stranki (iz baze) prikažemo u programu.
                foreach (var user in users2)
                {
                    ListViewItem showUser = new ListViewItem(user.User_id);
                    showUser.SubItems.Add(user.FirstName);
                    showUser.SubItems.Add(user.LastName);
                    showUser.SubItems.Add(user.Address);
                    showUser.SubItems.Add(user.Phone);
                    showUser.SubItems.Add(user.Email);
                    listView4.Items.Add(showUser);
                }

            }

        }



        // ---------------------------------- Pregled svih usluga iz baze --------------------------------------------//

        private async void all_services_button_Click(object sender, EventArgs e)
        {
            pdlistView.Items.Clear();
            services2.Clear();
            await GetServices("services2");
            try
            {
                foreach (var service in services2)
                {
                    ListViewItem showService = new ListViewItem(service.Id);
                    showService.SubItems.Add(service.Name);
                    showService.SubItems.Add(service.Value);
                    showService.SubItems.Add(service.Description);

                    pdlistView.Items.Add(showService);
                }
            }
            catch { }
        }

        public readonly List<Service> services2 = new List<Service>();
        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            services2.Clear();
            pdlistView.Items.Clear();
            string comm = $"SELECT * FROM services where name='{comboBox4.Text}'";
            MySqlConnection con = new MySqlConnection(User.CnnVal(connString));
            MySqlCommand cmd = new MySqlCommand(comm, con);
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string Service_id = reader.GetString("service_id");
                    string Service_name = reader.GetString("name");
                    string Service_description = reader.GetString("description");
                    string Service_value = reader.GetString("value");

                    if (Service_id != "" && Service_name != "" && Service_name != "")
                    {
                        // ------------------- Create new user!  ------------------- //
                        Service service = new Service();
                        service.Name = Service_name;
                        service.Id = Service_id;
                        service.Description = Service_description;
                        service.Value = Service_value;
                        services2.Add(service);
                    }

                }
            }
            catch { }
            finally
            {
                con.Close();
                pdlistView.Items.Clear();

                foreach (var service in services2)
                {
                    ListViewItem showService = new ListViewItem(service.Id);
                    showService.SubItems.Add(service.Name);
                    showService.SubItems.Add(service.Value);
                    showService.SubItems.Add(service.Description);
                    pdlistView.Items.Add(showService);
                }
            }
        }

        // Kreiramo savete pri kreiranju stranke/usluge.
        private void CreateLabelsForAdvices()
        {

            user_advice_label.Text = "Kratka uputstva: "
                + Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------"
                + Environment.NewLine +
                " * Pri dodajanju nove stranke morate imati ime, prezime i id korisnika za pravilan unos u bazu. Ostale parametre možete ostaviti prazne."
                + Environment.NewLine +
                " * Svaka stranka mora imati jedinstveni ID! Po tome ih razlikujemo. Primer: 'LOPNDS' "
                + Environment.NewLine +
                " * Jedinstven broj (ID) se sastoji od: "
                + Environment.NewLine +
                "      * LO oznake za lokaciju."
                + Environment.NewLine +
                "      * CU,PN skraćenice za grad. Kao i na reg. tablici."
                + Environment.NewLine +
                "      * MS,DS;DM inicijala (ImePrezime - IP).";

        }
        // "Svaki zapis se beleži u bazi, za popravak je potrebno kontaktirati administratora baze."
        //------------------------------------------ END SEARCH/ADD PANEL ---------------------------------//




    }
    public class User : Main
    {
        public string? User_id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public User() { }


    }
    public class Service
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }

        public Service() { }
    }
    public class Bill
    {
        public string? Id { get; set; }
        public string? Date { get; set; }
        public string? User_id { get; set; }
        public string? Salesman_id { get; set; }
        public string? Description { get; set; }
        public float Value { get; set; }
        public string? Number { get; set; }
        public Bill() { }
    }
}
