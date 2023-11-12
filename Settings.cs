using System.Net.NetworkInformation;

namespace Workshop1._1
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }
        private void CheckDBConnection(string ip_address)
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(ip_address, 2000);
                if (reply != null)
                {
                    MessageBox.Show("Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address);
                    //Console.WriteLine(reply.ToString());

                }
            }
            catch
            {
                MessageBox.Show("ERROR: You have Some TIMEOUT issue");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Odaberite bazu!", "Greška");
                return;
            }
            else if (comboBox1.Text != "Lokalna" && comboBox1.Text != "Online")
            {
                MessageBox.Show("Ta baza ne postoji!", "Greška");
                return;
            }
            switch (comboBox1.Text)
            {
                case "Lokalna":
                    {
                        CheckDBConnection("192.168.0.14");
                    }
                    break;
                case "Online":
                    {
                        CheckDBConnection("78.153.44.150");
                    }
                    break;
                default:
                    MessageBox.Show("Takva baza na postoji u sistemu! Proverimo: lokalnu ");
                    comboBox1.SelectedItem = "Lokalna";
                    break;
            }
        }
        // Send database name to another layers. 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.selectedBase = comboBox1.SelectedItem.ToString();
            Main.selectedBase = comboBox1.SelectedItem.ToString();

        }
    }
}
