using System.Windows.Forms;

namespace CryptoShark.Utility
{
    public class EnhancedForm : Form
    {
        public void ShowWarning(string message)
        {
            MessageBox.Show(this, message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void ShowError(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfo(string message)
        {
            MessageBox.Show(this, message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public DialogResult ShowQuestion(string message, bool showCancel = false)
        {
            if (showCancel)
                return MessageBox.Show(this, message, "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            else
                return MessageBox.Show(this, message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public EnhancedForm() : base()
        {

        }
    }
}
