using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Reg
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadKeys(Registry.CurrentUser);
            LoadKeys(Registry.LocalMachine);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void LoadKeys(RegistryKey rootKey)
        {
            await Task.Run(() =>
            {
                treeView1.Nodes.Add(new TreeNode(rootKey.Name));
                AddSubKeys(rootKey, treeView1.Nodes[treeView1.Nodes.Count - 1]);
            });
        }

        private void AddSubKeys(RegistryKey key, TreeNode node)
        {
            string[] subKeyNames = key.GetSubKeyNames();
            foreach (string subKeyName in subKeyNames)
            {
                RegistryKey subKey = key.OpenSubKey(subKeyName);
                if (subKey != null)
                {
                    TreeNode subNode = node.Nodes.Add(subKeyName);

                    if (subNode.Level < 2)
                        AddSubKeys(subKey, subNode);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Items.Clear();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(e.Node.FullPath);

            if (key != null)
            {
                foreach (string valueName in key.GetValueNames())
                {
                    object valueData = key.GetValue(valueName);
                    ListViewItem item = listView1.Items.Add(valueName);
                    item.SubItems.Add(valueData?.ToString());
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}