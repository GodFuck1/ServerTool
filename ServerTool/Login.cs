using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ServerTool
{
    public partial class Login : MaterialForm
    {
        private bool nowEnglish=false;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualProtect(byte[] lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallWindowProc(byte[] lpPrevWndFunc, int hWnd, int Msg, int wParam, int lParam);

        private const int NULL = 0;
        private const int PAGE_EXECUTE_READWRITE = 64;
        private static readonly byte[] buf_asm = { 85, 139, 236, 129, 236, 192, 0, 0, 0, 83, 86, 87, 141, 189, 64, 255, 255, 255, 185, 48, 0, 0, 0, 184, 204, 204, 204, 204, 243, 171, 184, 0, 0, 0, 0, 51, 210, 15, 162, 137, 85, 252, 137, 69, 248, 184, 1, 0, 0, 0, 51, 201, 51, 210, 15, 162, 137, 85, 244, 137, 69, 240, 139, 69, 252, 137, 69, 236, 139, 69, 248, 137, 69, 232, 139, 69, 244, 137, 69, 228, 139, 69, 240, 137, 69, 224, 141, 69, 236, 95, 94, 91, 139, 229, 93, 195 };
        public Login()
        {

            InitializeComponent();
            CenterToScreen();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
          
        }
        private static void VirtualProtect(byte[] address)
        {
            uint lpflOldProtect;
            VirtualProtect(address, address.Length, PAGE_EXECUTE_READWRITE, out lpflOldProtect);
        }
        public string getDays(TimeSpan time)
        {
            if (nowEnglish == true) { return ((time < TimeSpan.Zero) ? String.Format("License expired {0} days {1} hours {2} minuts before", Math.Abs(time.Days).ToString(), Math.Abs(time.Hours).ToString(), Math.Abs(time.Minutes).ToString()) : String.Format("License active. Left {0} days {1} hours {2} minutes", time.Days, time.Hours, time.Minutes)); }
            else return ((time < TimeSpan.Zero) ? String.Format("Лицензия истекла {0} дней {1} часов {2} минут назад", Math.Abs(time.Days).ToString(), Math.Abs(time.Hours).ToString(), Math.Abs(time.Minutes).ToString()) : String.Format("Лицензия активна. Осталось {0} дней {1} часов {2} минут", time.Days, time.Hours, time.Minutes));
        }

        public static string getSignedText(string[] response)
        {
            string returned = String.Empty;
            for (int i = 0; i < response.Length - 1; i++)
            {
                returned += response.GetValue(i) + "\r\n";
            }
            return returned;
        }
        public static string info(Random rn, Int32 token1, string preKey)
        {
            string tokenString = String.Format("token={0}&hwid={1}", DigitalSign.EncryptString(token1.ToString(), preKey), GetCPUID());
            return tokenString;
        }
        public static string GetRequest(string url, string post)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            byte[] buffer = Encoding.UTF8.GetBytes(post);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            request.Method = "POST";
            Stream newStream = request.GetRequestStream();
            newStream.Write(buffer, 0, post.Length);
            newStream.Close();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader strReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1251));
                string WorkingPage = strReader.ReadToEnd();
                response.Close();
                return WorkingPage;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return e.Message; }

        }


        public static string randomStringWithNumbers(int maxlength, Random rn)
        {
            StringBuilder sb = new StringBuilder();
            char[] allowedChars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < maxlength; i++)
            {
                int n = rn.Next(0, allowedChars.Length);
                if (char.IsLetter(allowedChars[n]))
                {
                    if (rn.Next(0, 2) == 0)
                    {
                        sb.Append(allowedChars[n].ToString().ToUpper());
                    }
                    else
                    {
                        sb.Append(allowedChars[n]);
                    }
                }
                else
                {
                    sb.Append(allowedChars[n]);
                }
            }
            return sb.ToString();
        }
        public void CheckLic()
        {
            string pubKeyNotXORed = "<RSAKeyValue><Modulus>qhA6WG54Aosn4RFNoAt1F/BX3lB1lXbNb3Pv7w30zzAYvHDLKD/+W/woggq6Y5bunz4GpsNl3z5NqQWsWTDkmpHwSTxMhabn/16G4TBB/RimVKJIn7fb7j3v3lYD5UtOGbJ5U/BXMpVPTIWjs73Zz9KNLmkGS1UQXEgpN4H4rQDOnPAqebuQZkTLKfIUwJgewcTpzip6OhInuWdobnV7oi71W/5SjgyRokgDy98ci/M6Jp41OOp7esVzMM3OU1JRGQmeRjFY26Op58vA5BeWy9zex8O7EOxOZegNRc1g5UlYx8bEShYs4WmAKBtoZW5mTDuXExpzDSSqCg8vb4UEdQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            Random curRandom = new Random();
            string preKey = randomStringWithNumbers(curRandom.Next(15, 21), curRandom);
            int XORkey = curRandom.Next(1, int.MaxValue);
            string urlToScript = DigitalSign.XOR("https://connect.dayz-tool.online/projects/rsa_generator/base.php", XORkey);
            string pubKey = DigitalSign.XOR(pubKeyNotXORed, XORkey);

            int token = curRandom.Next(1000000, int.MaxValue);
            string infoXORed = DigitalSign.XOR(info(curRandom, token, preKey), XORkey);
            string responseXORed = DigitalSign.XOR(GetRequest(DigitalSign.XOR(urlToScript, XORkey), DigitalSign.XOR(infoXORed, XORkey)), XORkey);
            string[] responseSplitted = DigitalSign.XOR(responseXORed, XORkey).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (responseSplitted[0].Split('=')[1] == "1")
                {

                    if (int.Parse(DigitalSign.DecryptString(responseSplitted[4], preKey)) == token)
                    {
                        if (GetCPUID() == Encoding.UTF8.GetString(Convert.FromBase64String(responseSplitted[1].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0])))
                        {
                            if (DigitalSign.CompareRSAMethod(getSignedText(responseSplitted), responseSplitted[responseSplitted.Length - 1].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0], DigitalSign.XOR(pubKey, XORkey)))
                            {

                                DateTime CurrentTime = DateTime.Parse(responseSplitted[2].Split('=')[1]);
                                DateTime EndTime = DateTime.Parse(responseSplitted[3].Split('=')[1]);
                                TimeSpan ActivatedTime = EndTime.Subtract(CurrentTime);
                                if (ActivatedTime < TimeSpan.Zero)
                                {
                                    //  MessageBox.Show(getDays(ActivatedTime));
                                    materialLabel2.Text = getDays(ActivatedTime);
                                }
                                else
                                {
                                   // nsButton2.Enabled = true;
                                    // MessageBox.Show(getDays(ActivatedTime));
                                    Form1 main = new Form1();
                                    main.Show();
                                    materialLabel2.Text = getDays(ActivatedTime);
                                    Visible = false;
                                    Opacity = 0;

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (nowEnglish == true) MessageBox.Show("License not found!");
                    else MessageBox.Show("Лицензии не обнаружено!");
                }
            }
            catch (Exception es)
            {
                if (nowEnglish == true) MessageBox.Show(es.Message);
                else MessageBox.Show(es.Message);
            }

        }
        private static string GetCPUID()
        {
            VirtualProtect(buf_asm);
            IntPtr ptr = CallWindowProc(buf_asm, NULL, NULL, NULL, NULL);

            int s1 = Marshal.ReadInt32(ptr);
            int s2 = Marshal.ReadInt32(ptr, 4);
            int s3 = Marshal.ReadInt32(ptr, 8);
            int s4 = Marshal.ReadInt32(ptr, 12);
            return s1.ToString("X") + s2.ToString("X") + s3.ToString("X") + s4.ToString("X");
        }

        private void Login_Load(object sender, EventArgs e)
        {
            materialTextBox1.Text = GetCPUID();
            CheckLic();
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
