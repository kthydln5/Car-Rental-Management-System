using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CarRentalSimpleApp
{
    public class MainForm : Form
    {
        private string apiUrl = "http://localhost:8000/api.php";
        private JavaScriptSerializer json = new JavaScriptSerializer();

        private DataGridView dgvVehicles = new DataGridView();
        private DataGridView dgvReservations = new DataGridView();

        private TextBox txtPlate = new TextBox();
        private TextBox txtBrand = new TextBox();
        private TextBox txtModel = new TextBox();
        private ComboBox cboType = new ComboBox();
        private NumericUpDown numRate = new NumericUpDown();

        private Button btnAddVehicle = new Button();
        private Button btnRefresh = new Button();
        private Button btnApprove = new Button();
        private Button btnCancel = new Button();

        private Label lblStatus = new Label();

        private Color ink = Color.FromArgb(15, 23, 42);
        private Color orange = Color.FromArgb(234, 88, 12);
        private Color blue = Color.FromArgb(37, 99, 235);
        private Color red = Color.FromArgb(185, 28, 28);

        public MainForm()
        {
            Text = "STARTER - Admin Car Rental Management";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 680);
            MinimumSize = new Size(980, 620);
            Font = new Font("Segoe UI", 9);
            BackColor = Color.FromArgb(241, 245, 249);

            BuildInterface();

            Load += delegate { RefreshAll(); };
        }

        private void BuildInterface()
        {
            Panel header = new Panel();
            header.Location = new Point(18, 15);
            header.Size = new Size(1045, 78);
            header.BackColor = ink;
            Controls.Add(header);

            Label title = new Label();
            title.Text = "STARTER ADMIN CAR RENTAL MANAGEMENT";
            title.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.AutoSize = true;
            title.Location = new Point(22, 13);
            header.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Early-stage C# admin app. Vehicle adding and approval buttons are TODO features.";
            subtitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            subtitle.ForeColor = Color.FromArgb(254, 215, 170);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(26, 47);
            header.Controls.Add(subtitle);

            Panel left = MakeCard(new Point(18, 112), new Size(360, 500), "TODO: Vehicle Admin Form");
            Controls.Add(left);

            AddLabel(left, "Plate Number", 18, 55, 110);
            txtPlate.Location = new Point(135, 52);
            txtPlate.Size = new Size(185, 26);
            txtPlate.Text = "NEW-1001";
            left.Controls.Add(txtPlate);

            AddLabel(left, "Brand", 18, 90, 110);
            txtBrand.Location = new Point(135, 87);
            txtBrand.Size = new Size(185, 26);
            txtBrand.Text = "Toyota";
            left.Controls.Add(txtBrand);

            AddLabel(left, "Model", 18, 125, 110);
            txtModel.Location = new Point(135, 122);
            txtModel.Size = new Size(185, 26);
            txtModel.Text = "Corolla";
            left.Controls.Add(txtModel);

            AddLabel(left, "Type", 18, 160, 110);
            cboType.Location = new Point(135, 157);
            cboType.Size = new Size(185, 26);
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboType.Items.Add("Sedan");
            cboType.Items.Add("SUV");
            cboType.Items.Add("MPV");
            cboType.Items.Add("Van");
            cboType.Items.Add("Pickup");
            cboType.SelectedIndex = 0;
            left.Controls.Add(cboType);

            AddLabel(left, "Daily Rate", 18, 195, 110);
            numRate.Location = new Point(135, 192);
            numRate.Size = new Size(185, 26);
            numRate.Maximum = 100000;
            numRate.Minimum = 1;
            numRate.Value = 1800;
            left.Controls.Add(numRate);

            btnAddVehicle.Text = "TODO: ADD VEHICLE";
            btnAddVehicle.Location = new Point(18, 245);
            btnAddVehicle.Size = new Size(302, 34);
            StyleButton(btnAddVehicle, orange);
            btnAddVehicle.Click += delegate { AddVehicleTodo(); };
            left.Controls.Add(btnAddVehicle);

            btnRefresh.Text = "REFRESH STARTER DATA";
            btnRefresh.Location = new Point(18, 292);
            btnRefresh.Size = new Size(302, 34);
            StyleButton(btnRefresh, ink);
            btnRefresh.Click += delegate { RefreshAll(); };
            left.Controls.Add(btnRefresh);

            Label todo = new Label();
            todo.Text = "TODO commits should connect these admin features step by step:\n\n- add vehicle\n- update vehicle status\n- approve requests\n- activate rental\n- mark returned\n- cancel request";
            todo.Location = new Point(18, 350);
            todo.Size = new Size(310, 120);
            todo.ForeColor = Color.FromArgb(71, 85, 105);
            todo.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            left.Controls.Add(todo);

            Panel right = MakeCard(new Point(400, 112), new Size(663, 500), "Starter Data Monitor");
            Controls.Add(right);

            Label vtitle = new Label();
            vtitle.Text = "Vehicles loaded from API";
            vtitle.Location = new Point(18, 52);
            vtitle.Size = new Size(280, 24);
            vtitle.ForeColor = orange;
            vtitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            right.Controls.Add(vtitle);

            dgvVehicles.Location = new Point(18, 80);
            dgvVehicles.Size = new Size(625, 165);
            ConfigureGrid(dgvVehicles);
            right.Controls.Add(dgvVehicles);

            Label rtitle = new Label();
            rtitle.Text = "Reservations will appear after create_reservation is implemented";
            rtitle.Location = new Point(18, 260);
            rtitle.Size = new Size(520, 24);
            rtitle.ForeColor = orange;
            rtitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            right.Controls.Add(rtitle);

            dgvReservations.Location = new Point(18, 288);
            dgvReservations.Size = new Size(625, 120);
            ConfigureGrid(dgvReservations);
            right.Controls.Add(dgvReservations);

            btnApprove.Text = "TODO: APPROVE";
            btnApprove.Location = new Point(18, 430);
            btnApprove.Size = new Size(130, 34);
            StyleButton(btnApprove, blue);
            btnApprove.Click += delegate { MessageBox.Show("TODO: Implement update_reservation_status = Approved."); };
            right.Controls.Add(btnApprove);

            btnCancel.Text = "TODO: CANCEL";
            btnCancel.Location = new Point(160, 430);
            btnCancel.Size = new Size(130, 34);
            StyleButton(btnCancel, red);
            btnCancel.Click += delegate { MessageBox.Show("TODO: Implement update_reservation_status = Cancelled."); };
            right.Controls.Add(btnCancel);

            lblStatus.Location = new Point(18, 628);
            lblStatus.Size = new Size(1045, 28);
            lblStatus.BackColor = Color.White;
            lblStatus.ForeColor = ink;
            lblStatus.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Padding = new Padding(12, 0, 0, 0);
            lblStatus.Text = "Ready.";
            Controls.Add(lblStatus);
        }

        private Panel MakeCard(Point location, Size size, string titleText)
        {
            Panel p = new Panel();
            p.Location = location;
            p.Size = size;
            p.BackColor = Color.White;

            Label title = new Label();
            title.Text = titleText;
            title.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            title.ForeColor = orange;
            title.Location = new Point(18, 16);
            title.Size = new Size(size.Width - 36, 28);
            p.Controls.Add(title);

            return p;
        }

        private void AddLabel(Control parent, string text, int x, int y, int w)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(w, 23);
            label.ForeColor = ink;
            label.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            parent.Controls.Add(label);
        }

        private void StyleButton(Button button, Color color)
        {
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 8, FontStyle.Bold);
        }

        private void ConfigureGrid(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ink;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(254, 215, 170);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            dgv.DefaultCellStyle.SelectionBackColor = orange;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            dgv.RowTemplate.Height = 24;
        }

        private Dictionary<string, object> GetApi(string query)
        {
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(apiUrl + "?" + query);
                return json.Deserialize<Dictionary<string, object>>(response);
            }
        }

        private void RefreshAll()
        {
            try
            {
                LoadVehicles();
                LoadReservations();
                lblStatus.Text = "Starter admin connected to " + apiUrl;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Connection failed. Start PHP first: php -S localhost:8000";
                MessageBox.Show("Cannot connect to PHP API.\n\nRun PHP first inside php-api folder:\nphp -S localhost:8000\n\nDetails:\n" + ex.Message);
            }
        }

        private void LoadVehicles()
        {
            Dictionary<string, object> result = GetApi("action=list_vehicles");
            ArrayList rows = result["vehicles"] as ArrayList;

            dgvVehicles.Columns.Clear();
            dgvVehicles.Rows.Clear();

            dgvVehicles.Columns.Add("id", "ID");
            dgvVehicles.Columns.Add("plate", "Plate");
            dgvVehicles.Columns.Add("vehicle", "Vehicle");
            dgvVehicles.Columns.Add("type", "Type");
            dgvVehicles.Columns.Add("rate", "Rate");
            dgvVehicles.Columns.Add("status", "Status");

            if (rows != null)
            {
                foreach (object obj in rows)
                {
                    Dictionary<string, object> v = obj as Dictionary<string, object>;
                    if (v == null) continue;

                    dgvVehicles.Rows.Add(v["id"], v["plate_number"], v["brand"] + " " + v["model"], v["vehicle_type"], v["daily_rate"], v["status"]);
                }
            }
        }

        private void LoadReservations()
        {
            Dictionary<string, object> result = GetApi("action=list_reservations");
            ArrayList rows = result["reservations"] as ArrayList;

            dgvReservations.Columns.Clear();
            dgvReservations.Rows.Clear();

            dgvReservations.Columns.Add("id", "ID");
            dgvReservations.Columns.Add("customer", "Customer");
            dgvReservations.Columns.Add("vehicle", "Vehicle");
            dgvReservations.Columns.Add("status", "Status");

            if (rows != null)
            {
                foreach (object obj in rows)
                {
                    Dictionary<string, object> r = obj as Dictionary<string, object>;
                    if (r == null) continue;

                    dgvReservations.Rows.Add(r["id"], r["customer_name"], r["plate_number"] + " " + r["model"], r["reservation_status"]);
                }
            }
        }

        private void AddVehicleTodo()
        {
            MessageBox.Show("TODO: Connect this button to action=add_vehicle in a later commit.");
        }
    }
}
