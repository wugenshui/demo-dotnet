using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;

namespace WindowsWorkflowFoundation
{
    public partial class Form1 : Form
    {
        private WorkflowRuntime workflowRuntime;
        private WorkflowInstance workflowInstance;
        public class Work
        {

        }

        public Form1()
        {
            InitializeComponent();

            workflowRuntime = new WorkflowRuntime();
            workflowRuntime.StartRuntime();
            workflowRuntime.WorkflowCompleted += workflowRuntime_WorkflowCompleted;
            // Collapse approve/reject panel
            //this.Height = this.MinimumSize.Height;
        }

        private void workflowRuntime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Type type = typeof(Work);

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Amount", Int32.Parse(this.amount.Text));

            // Start the workflow.
            this.workflowInstance = workflowRuntime.CreateWorkflow(type, properties);
            this.workflowInstance.Start();
        }

        private void approveButton_Click(object sender, EventArgs e)
        {
        }

        private void rejectButton_Click(object sender, EventArgs e)
        {
        }

        private void amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && (!Char.IsDigit(e.KeyChar)))
                e.KeyChar = Char.MinValue;
        }

        private void amount_TextChanged(object sender, EventArgs e)
        {
            submitButton.Enabled = amount.Text.Length > 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
