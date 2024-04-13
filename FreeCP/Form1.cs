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

        // Windows API 导入
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 定义热键 ID
        private const int HOTKEY_ID = 1;
        // 定义热键 ID
        private const int HOTKEY_ID2 = 2;
        // 定义热键 ID
        private const int HOTKEY_ID3 = 3;

        // 定义控制台事件类型
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        /*
    0x0000: 无修饰键
    0x0002: Ctrl键
    0x0004: Shift键
    0x0008: Alt键
    0x4000: 用于与字符键结合使用，表示按下了Alt键
    0x8000: 用于与字符键结合使用，表示按下了Ctrl键
         */

        private NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolTip toolTip;
        public Form1()
        {
            InitializeComponent();
            // 注册热键 Ctrl+Q
            bindInfo.Text = "未绑定";
            bindInfo.ForeColor = Color.Red;
            Bind();
            // 创建通知图标
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = false;

            // 创建右键菜单
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("绑定", null, (sender, e) => Bind());
            contextMenu.Items.Add("解绑", null, (sender, e) => ReleaseBind());
            contextMenu.Items.Add("退出", null, (sender, e) => ExitApplication());

            // 将右键菜单关联到通知图标
            notifyIcon.ContextMenuStrip = contextMenu;

            // 关联窗体的关闭事件
            this.Resize += MainForm_Resize;

            // 关联通知图标的双击事件
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;


            // 创建 ToolTip 控件
            toolTip = new System.Windows.Forms.ToolTip();

            // 关联鼠标移入事件
            notifyIcon.MouseMove += NotifyIcon_MouseMove;

        }


        private void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            // 在鼠标移入时设置 ToolTip 文本
            notifyIcon.Text = bindInfo.Text;
        }
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 在双击通知图标时恢复窗体
            if (e.Button == MouseButtons.Left)
            {
                RestoreForm();
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // 判断窗体是否被最小化
            if (this.WindowState == FormWindowState.Minimized)
            {
                // 隐藏窗体，并取消关闭操作
                HideForm();
            }
        }


        private void HideForm()
        {
            // 隐藏窗体，并显示通知图标
            this.Hide();
            notifyIcon.Visible = true;
        }

        private void RestoreForm()
        {
            // 显示窗体，并隐藏通知图标
            this.Show();
            notifyIcon.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void ExitApplication()
        {
            // 退出应用程序
            notifyIcon.Dispose(); // 释放通知图标资源
            Application.Exit();
        }
        void Bind()
        {
            if (isBinded == false)
            {
                RegisterHotKey(this.Handle, HOTKEY_ID, 2, (uint)'Q');
                //RegisterHotKey(this.Handle, HOTKEY_ID2, 2, (uint)'D');
                RegisterHotKey(this.Handle, HOTKEY_ID3, 2, (uint)'B');
                bindInfo.Text = "已绑定";
                bindInfo.ForeColor = Color.Green;
                isBinded = true;
            }
        }
        // 处理窗体消息
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();

                // 根据热键ID执行相应的操作
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
            //如果好运的话用这个
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
            // 使用 LINQ 将字符串拆分成每个元素最多 chunkSize 个字符的数组
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





        // 控制台事件处理程序
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // 在这里可以处理控制台关闭等事件

            // 返回 false 表示继续传递事件，返回 true 表示阻止事件传递
            return false;
        }

        // 窗体关闭时取消注册热键
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
            bindInfo.Text = "未绑定";
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
