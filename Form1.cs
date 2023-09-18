namespace HW_Server_TCP
{
    public partial class Form1 : Form
    {
        SynchronizationContext uiContext;
        Controller.Controller controller;
        public Form1()
        {
            InitializeComponent();
            uiContext = new SynchronizationContext();
            controller = new Controller.Controller(this);
        }


        public void IniListBox(string text)
        {
            uiContext.Send(x=>listBox1.Items.Add(text),null);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(controller.StartThreadForAccept));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}