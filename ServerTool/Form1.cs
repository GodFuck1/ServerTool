using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ServerTool
{
    public partial class Form1 : MaterialForm
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn (int nLeftRect, int nTopRect,int nRightRect,int nBottomRect,int nWidthEllipse,int nHeightEllipse);
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            materialLabel7.Text = "";//garage
            materialLabel8.Text = "";//discord
        }

    
        private void Form1_Load(object sender, EventArgs e)
        {
            materialLabel6.Text = Licenses.CheckLic();
            if (materialLabel6.Text == "Лицензия не приобреталась") materialTabControl1.TabPages.Remove(tabPage2); //.Enabled = false;
            else  materialTabControl1.TabPages.Remove(tabPage2);
        }

        private void WebBtn_MouseEnter(object sender, EventArgs e)
        {
        }

        
        void ResetColors()
        {
    /*        WebBtn.BackColor = Color.FromArgb(74, 74, 74);//  74; 74; 74
            button2.BackColor = Color.FromArgb(74, 74, 74);//  48; 101; 231
            button3.BackColor = Color.FromArgb(74, 74, 74);//  48; 101; 231
            button4.BackColor = Color.FromArgb(74, 74, 74);//  48; 101; 231
            button1.BackColor = Color.FromArgb(74, 74, 74);//  48; 101; 231*/
        }
        private void WebBtn_Click(object sender, EventArgs e)
        {
            ResetColors();
          //  WebBtn.BackColor = Color.FromArgb(48, 101, 231);//  48; 101; 231
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetColors();
          //  button1.BackColor = Color.FromArgb(48, 101, 231);//  48; 101; 231
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetColors();
          //  button2.BackColor = Color.FromArgb(48, 101, 231);//  48; 101; 231
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetColors();
           // button3.BackColor = Color.FromArgb(48, 101, 231);//  48; 101; 231
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetColors();
          //  button4.BackColor = Color.FromArgb(48, 101, 231);//  48; 101; 231
        }
    }

}
