using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Linq;
using TodoWinformApp.Entities;


namespace TodoWinformApp
{
    public partial class TodoListForm : DevExpress.XtraEditors.XtraForm
    {
        public TodoListForm()
        {
            InitializeComponent();
        }

        

        private void TodoListForm_Load(object sender, EventArgs e)
        {
            LoadTodos();
        }

        private void LoadTodos() 
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession()) 
                    taskBindingSource.DataSource = session.Query<Task>();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("An error occurred while loading tasks: " + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        

        private void btnAddTodo_Click(object sender, EventArgs e)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var task = new Task
                    {
                        Description = txtAddTodo.Text
                    };
                    session.Save(task);
                    transaction.Commit();
                }

                MessageBox.Show("Updated Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while adding the task: " + ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this task?", "Delete", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            try
            {
                var todo = taskBindingSource.Current as Task;

                using (var session = NHibernateHelper.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(todo);
                    transaction.Commit();
                }

                MessageBox.Show("Deleted Successful", "Success", MessageBoxButtons.OK);
                LoadTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the task: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var todo = taskBindingSource.Current as Task;

                using (var session = NHibernateHelper.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(todo);
                    transaction.Commit();
                }

                MessageBox.Show("Updated Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            { 
                MessageBox.Show("An error occured while updating the task: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            }

        }

        private void taskDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
