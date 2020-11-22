using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JAMDL.tools
{
    public static class tools
    {
        public static Task WaitForExitAsync(this Process process)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => tcs.TrySetResult(null);
            return process.HasExited ? Task.CompletedTask : tcs.Task;
        }

        // Taken From Carlos Delgado
        // https://ourcodeworld.com/authors/sdkcarlos

        public static void EnableContextMenu(this RichTextBox rtb)
        {
            if (rtb.ContextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                cms.ShowImageMargin = false;

                ToolStripMenuItem tsmiUndo = new ToolStripMenuItem("Undo");
                tsmiUndo.Click += (sender, e) => rtb.Undo();

                ToolStripMenuItem tsmiRedo = new ToolStripMenuItem("Redo");
                tsmiRedo.Click += (sender, e) => rtb.Redo();

                ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
                tsmiCut.Click += (sender, e) => rtb.Cut();

                ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
                tsmiCopy.Click += (sender, e) => rtb.Copy();

                ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
                tsmiPaste.Click += (sender, e) => rtb.Paste();

                ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
                tsmiDelete.Click += (sender, e) => rtb.SelectedText = "";

                ToolStripMenuItem tsmiSelectAll = new ToolStripMenuItem("Select All");
                tsmiSelectAll.Click += (sender, e) => rtb.SelectAll();


                cms.Items.Add(tsmiCut);
                cms.Items.Add(tsmiCopy);
                cms.Items.Add(tsmiPaste);
                cms.Items.Add(new ToolStripSeparator());
                cms.Items.Add(tsmiUndo);
                cms.Items.Add(tsmiRedo);
                cms.Items.Add(tsmiDelete);
                cms.Items.Add(new ToolStripSeparator());
                cms.Items.Add(tsmiSelectAll);
                // When opening the menu, check if the condition is fulfilled 
                // in order to enable the action
                cms.Opening += (sender, e) =>
                {
                    tsmiUndo.Enabled = !rtb.ReadOnly && rtb.CanUndo;
                    tsmiRedo.Enabled = !rtb.ReadOnly && rtb.CanRedo;
                    tsmiCut.Enabled = !rtb.ReadOnly && rtb.SelectionLength > 0;
                    tsmiCopy.Enabled = rtb.SelectionLength > 0;
                    tsmiPaste.Enabled = !rtb.ReadOnly && Clipboard.ContainsText();
                    tsmiDelete.Enabled = !rtb.ReadOnly && rtb.SelectionLength > 0;
                    tsmiSelectAll.Enabled = rtb.TextLength > 0 && rtb.SelectionLength < rtb.TextLength;
                };

                rtb.ContextMenuStrip = cms;
            }
        }
    }
}
