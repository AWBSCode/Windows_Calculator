using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // making the custom border.
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        private void headBorder_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        // custom border end.

        // Calculator variables
        private bool isDotUsed = false;
        private bool firstAction = true;
        private double num1 = 0.0;
        private double num2 = 0.0;
        private double resultNum = 0.0;
        private string op = "";
        private Button lastBtn = null;

        private void btnNumbers_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (firstAction)
            {
                lblResult.Text = "";
                firstAction = false;
            }

            if (btn.Text == "." && isDotUsed)
            {
                return;
            }

            if (btn.Text == ".")
            {
                isDotUsed = true;
            }

            lblResult.Text += btn.Text;
        }

        private double GetNumberOnScreen()
        {
            try
            {
                return double.Parse(lblResult.Text);
            }
            catch (FormatException)
            {
                lblLastAction.Text = "Invalid number format";
                return 0.0;
            }
        }

        private bool IsDivisionByZero(double divisor)
        {
            if (divisor == 0)
            {
                lblLastAction.Text = "ERROR! Division by Zero";
                lblLastAction.ForeColor = Color.Red;
                return true;
            }
            return false;
        }

        private void PerformOperation(string operationSign)
        {
            switch (operationSign)
            {
                case "+":
                    resultNum = num1 + num2;
                    break;
                case "-":
                    resultNum = num1 - num2;
                    break;
                case "/":
                    if (IsDivisionByZero(num2))
                        return;
                    resultNum = num1 / num2;
                    break;
                case "×":
                    resultNum = num1 * num2;
                    break;
                case "1/x":
                    if (IsDivisionByZero(num2))
                        return;
                    resultNum = 1 / num2;
                    break;
                case "%":
                    resultNum = num1 % num2;
                    break;
                case "x^2":
                    resultNum = num2 * num2;
                    break;
                case "sqr":
                    resultNum = Math.Sqrt(num2);
                    break;
                default:
                    break;
            }
            lblResult.Text = resultNum.ToString();
        }

        private void Btn_Operation_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (!firstAction && lastBtn != null && lastBtn != btn)
            {
                btnEqual_Click(sender, e);
            }

            firstAction = true;
            isDotUsed = false;
            op = btn.Tag.ToString();
            num1 = GetNumberOnScreen();
            lblLastAction.Text = $"{num1} {op}";
            lastBtn = btn;
        }

        private void GetFinalTotal()
        {
            num2 = GetNumberOnScreen();
            PerformOperation(op);
            lblLastAction.Text = $"{num1} {op} {num2} = {resultNum}";
            num1 = resultNum;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            if (op == "") return;
            GetFinalTotal();
            firstAction = true;
            op = "";
        }

        // Controls and Features
        private void RestartAndReset()
        {
            isDotUsed = false;
            num1 = 0.0;
            num2 = 0.0;
            resultNum = 0.0;
            op = "";
            firstAction = true;
            lblResult.Text = "0";
            lblLastAction.Text = "";
            if (lastBtn != null) lastBtn.Enabled = true;
            lastBtn = null;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            RestartAndReset();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (lblResult.Text.Length == 1)
            {
                lblResult.Text = "0";
                firstAction = true;
                return;
            }
            lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1);
            if (!lblResult.Text.Contains("."))
            {
                isDotUsed = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblResult.Text = "0";
            firstAction = true;
        }

        private void btnReverseSign_Click(object sender, EventArgs e)
        {
            double num = GetNumberOnScreen();
            if (num != 0.0)
            {
                num = -num;
                lblResult.Text = num.ToString(); 
            }
        }
    }
}
