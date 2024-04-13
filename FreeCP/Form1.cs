using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FreeCP
{
    public partial class Form1 : Form
    {

        // Windows API ����
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // �����ȼ� ID
        private const int HOTKEY_ID = 1;
        // �����ȼ� ID
        private const int HOTKEY_ID2 = 2;
        // �����ȼ� ID
        private const int HOTKEY_ID3 = 3;

        // �������̨�¼�����
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        /*
    0x0000: �����μ�
    0x0002: Ctrl��
    0x0004: Shift��
    0x0008: Alt��
    0x4000: �������ַ������ʹ�ã���ʾ������Alt��
    0x8000: �������ַ������ʹ�ã���ʾ������Ctrl��
         */

        private NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolTip toolTip;
        public Form1()
        {
            InitializeComponent();
            // ע���ȼ� Ctrl+Q
            bindInfo.Text = "δ��";
            bindInfo.ForeColor = Color.Red;
            Bind();
            // ����֪ͨͼ��
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = false;

            // �����Ҽ��˵�
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("��", null, (sender, e) => Bind());
            contextMenu.Items.Add("���", null, (sender, e) => ReleaseBind());
            contextMenu.Items.Add("�˳�", null, (sender, e) => ExitApplication());

            // ���Ҽ��˵�������֪ͨͼ��
            notifyIcon.ContextMenuStrip = contextMenu;

            // ��������Ĺر��¼�
            this.Resize += MainForm_Resize;

            // ����֪ͨͼ���˫���¼�
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;


            // ���� ToolTip �ؼ�
            toolTip = new System.Windows.Forms.ToolTip();

            // ������������¼�
            notifyIcon.MouseMove += NotifyIcon_MouseMove;

        }


        private void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            // ���������ʱ���� ToolTip �ı�
            notifyIcon.Text = bindInfo.Text;
        }
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // ��˫��֪ͨͼ��ʱ�ָ�����
            if (e.Button == MouseButtons.Left)
            {
                RestoreForm();
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // �жϴ����Ƿ���С��
            if (this.WindowState == FormWindowState.Minimized)
            {
                // ���ش��壬��ȡ���رղ���
                HideForm();
            }
        }


        private void HideForm()
        {
            // ���ش��壬����ʾ֪ͨͼ��
            this.Hide();
            notifyIcon.Visible = true;
        }

        private void RestoreForm()
        {
            // ��ʾ���壬������֪ͨͼ��
            this.Show();
            notifyIcon.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void ExitApplication()
        {
            // �˳�Ӧ�ó���
            notifyIcon.Dispose(); // �ͷ�֪ͨͼ����Դ
            Application.Exit();
        }
        void Bind()
        {
            if (isBinded == false)
            {
                RegisterHotKey(this.Handle, HOTKEY_ID, 2, (uint)'Q');
                //RegisterHotKey(this.Handle, HOTKEY_ID2, 2, (uint)'D');
                RegisterHotKey(this.Handle, HOTKEY_ID3, 2, (uint)'B');
                bindInfo.Text = "�Ѱ�";
                bindInfo.ForeColor = Color.Green;
                isBinded = true;
            }
        }
        // ��������Ϣ
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();

                // �����ȼ�IDִ����Ӧ�Ĳ���
                if (id == HOTKEY_ID)
                {
                    //Console.WriteLine("Hotkey Ctrl+Q pressed. Exiting application.");
                    ReleaseBind();
                }
                //else if (id == HOTKEY_ID2)
                //{
                //    GetClipBoardText();
                //}
                else if (id == HOTKEY_ID3)
                {
                    GetClipBoardText();
                    PasteAll();
                }
            }

            base.WndProc(ref m);
        }
        void GetClipBoardText()
        {
            //SendKeys.SendWait("^(c)");
            richTextBox1.Text = GetClipboardText();
        }

        void PasteAll()
        {
            //������˵Ļ������
            //SetClipboardText(richTextBox1.Text);
            //SendKeys.SendWait("^(v)");

            string[] allShortStr = SplitString(richTextBox1.Text, 300);

            foreach (var item in allShortStr)
            {
                SetClipboardText(item);
                SendKeys.SendWait("^(v)");
            }
        }

        string[] SplitString(string input, int chunkSize)
        {
            // ʹ�� LINQ ���ַ�����ֳ�ÿ��Ԫ����� chunkSize ���ַ�������
            return Enumerable.Range(0, (int)Math.Ceiling((double)input.Length / chunkSize))
                .Select(i => input.Substring(i * chunkSize, Math.Min(chunkSize, input.Length - i * chunkSize)))
                .ToArray();
        }

        void SetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting clipboard text: {ex.Message}");
            }
        }
        string GetClipboardText()
        {
            string clipboardText = "";

            try
            {
                if (Clipboard.ContainsText())
                {
                    clipboardText = Clipboard.GetText();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting clipboard text: {ex.Message}");
            }

            return clipboardText;
        }





        // ����̨�¼��������
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // ��������Դ������̨�رյ��¼�

            // ���� false ��ʾ���������¼������� true ��ʾ��ֹ�¼�����
            return false;
        }

        // ����ر�ʱȡ��ע���ȼ�
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ReleaseBind();
            base.OnFormClosing(e);
        }
        bool isBinded = false;
        void ReleaseBind()
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
           // UnregisterHotKey(this.Handle, HOTKEY_ID2);
            UnregisterHotKey(this.Handle, HOTKEY_ID3);
            bindInfo.Text = "δ��";
            bindInfo.ForeColor = Color.Red;
            isBinded = false;
        }

    
       
        private void ReleaseButton_Click(object sender, EventArgs e)
        {
            ReleaseBind();
        }

        private void BindButton_Click(object sender, EventArgs e)
        {
            Bind();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
