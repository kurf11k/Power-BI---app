using BusinessLayer.DatasetTemplates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class EditTableTemplateForm : Form
    {
        DatasetTemplate DatasetTemplate;
        TableTemplate SelectedTable;

        Action<DatasetTemplate> SaveTemplate;

        public EditTableTemplateForm(DatasetTemplate datasetTemplate, Action<DatasetTemplate> saveTemplate)
        {
            InitializeComponent();
            DatasetTemplate = DeepCopy(datasetTemplate);
            SaveTemplate = saveTemplate;
        }

        private DatasetTemplate DeepCopy(DatasetTemplate original)
        {
            var json = JsonConvert.SerializeObject(original);
            return JsonConvert.DeserializeObject<DatasetTemplate>(json);
        }
        private void artefactsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ArtefactsListBox.SelectedIndex == -1)
                return;
            SelectedTable = DatasetTemplate.Tables[ArtefactsListBox.SelectedIndex];

            RemoveArtefactButton.Visible = !SelectedTable.IsDefault;
            if (SelectedTable.Expression == "") SelectedTable.Expression = null;

            UpdateColumnsDataGridView();
            UpdateMeasuresDataGridView();

            UpdateColumnDynamicComboBox();

            ExpressionCheckBox.Checked = !(SelectedTable.Expression == null);
            if (ExpressionCheckBox.Checked)
                ExpressionTextBox.Text = SelectedTable.Expression;
            else
                ExpressionTextBox.Text = "";

            ExpressionTextBox.Enabled = ExpressionCheckBox.Checked;
        }

        private void EditTableTemplateForm_Load(object sender, EventArgs e)
        {
            ArtefactsListBox.Items.AddRange(DatasetTemplate.Tables.ToArray());
            ArtefactsListBox.DisplayMember = "Name";
            ArtefactsListBox.SelectedIndex = 0;

            ColumnTypeComboBox.DataSource = Enum.GetNames(typeof(ColumnTypes));

            UpdateColumnDynamicComboBox();
            SetSourceTableComboBox();

            RelationTypeComboBox.DataSource = Enum.GetNames(typeof(RelationshipTypes));

            foreach (var relation in DatasetTemplate.Relationships)
            {
                RelationshipsDataGridView.Rows.Add(new string[] { relation.FromTable, relation.FromColumn, relation.ToTable, relation.ToColumn, relation.Type.ToString() });
                var lastRow = RelationshipsDataGridView.Rows[RelationshipsDataGridView.Rows.Count - 1];
                var buttonCell = lastRow.Cells[lastRow.Cells.Count - 1];
                var style = new DataGridViewCellStyle();
                style.Padding = new Padding(0, 0, 1000, 0);
                buttonCell.Style = style;            
            }
        }
        private void SetDestinationTableComboBox()
        {
            var choosedTable = SourceTableComboBox.SelectedItem as TableTemplate;
            var filteredTables = DatasetTemplate.Tables.ToList();
            filteredTables.Remove(choosedTable);
            DestinationTableComboBox.DataSource = filteredTables;
            DestinationTableComboBox.SelectedIndex = 0;
            DestinationTableComboBox.DisplayMember = "Name";
        }
        private void SetAgainstColumnOptions()
        {
            var choosedColumn = SourceColumnComboBox.SelectedItem as ColumnTemplate;
            if (choosedColumn == null)
                return;
            var filteredColumns = (DestinationTableComboBox.SelectedItem as TableTemplate).Columns.ToList();
            for(var i = 0; i < filteredColumns.Count; i++)
            {
                var col = filteredColumns[i];
                if (choosedColumn.Type != col.Type)
                {
                    filteredColumns.Remove(col);
                    i--;
                }
            }
            DestinationColumnComboBox.DataSource = filteredColumns;
            DestinationColumnComboBox.DisplayMember = "Name";
        }

        private void AddArtefactButton_Click(object sender, EventArgs e)
        {
            var name = ArtefactNameTextBox.Text;
            try
            {
                ArtefactNameTextBox.Text = "";
                DatasetTemplate.AddTable(new TableTemplate(name));
                ArtefactsListBox.Items.Add(name);
                SetSourceTableComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddColumnButton_Click(object sender, EventArgs e)
        {
            if (!IsColumnValid())
                return;
            var name = ColumnNameTextBox.Text;            
            ColumnNameTextBox.Text = "";
            var type = (ColumnTypes)Enum.Parse(typeof(ColumnTypes), ColumnTypeComboBox.SelectedItem.ToString());
            var isDynamic = (bool)ColumnDynamicComboBox.SelectedItem;
            var item = new string[] {name, type.ToString(), isDynamic.ToString() };

            var column = new ColumnTemplate(name, type, isDynamic);

            try
            {
                SelectedTable.AddColumn(column);
                ColumnDataGridView.Rows.Add(item);
                if (SelectedTable.NeedID())
                {
                    var idColumn = SelectedTable.Columns[0];
                    var itemId = new string[] {idColumn.Name, idColumn.Type.ToString(), idColumn.IsDynamic.ToString() };
                    ColumnDataGridView.Rows.Insert(0, itemId);
                    var buttonCell = ColumnDataGridView.Rows[0].Cells[ColumnDataGridView.Rows[0].Cells.Count - 1];
                    var style = new DataGridViewCellStyle();
                    style.Padding = new Padding(0, 0, 1000, 0);
                    buttonCell.Style = style;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (SelectedTable.Name == ((TableTemplate)SourceTableComboBox.SelectedItem).Name)
                SetSourceColumnComboBox();
            else if (SelectedTable.Name == ((TableTemplate)DestinationTableComboBox.SelectedItem).Name)
                SetAgainstColumnOptions();

            
        }

        private bool IsColumnValid()
        {
            if (ColumnNameTextBox.Text == "")
            {
                MessageBox.Show("You cannot add column without name.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (ColumnTypeComboBox.SelectedItem.ToString() == "")
            {
                MessageBox.Show("You cannot add column without type.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (ColumnDynamicComboBox.SelectedItem.ToString() == "")
            {
                MessageBox.Show("You cannot add a column without specifying a dynamic attribute.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool IsRelationshipValid()
        {
            if (SourceTableComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("You cannot add relationship without source table.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (SourceColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("You cannot add relationship without source column.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (DestinationTableComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("You cannot add relationship without destination table.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (DestinationColumnComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("You cannot add relationship without destination column.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (RelationTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("You cannot add relationship without relationship type.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void SourceTableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SourceColumnComboBox.DataSource = DatasetTemplate.Tables[SourceTableComboBox.SelectedIndex].Columns;
            SourceColumnComboBox.DisplayMember = "Name";
            SetDestinationTableComboBox();
            if (SourceColumnComboBox.Items.Count > 0)
                SourceColumnComboBox.SelectedIndex = 0;       
        }

        private void DestinationTableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetAgainstColumnOptions();
        }

        private void SourceColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {   
            if (DestinationTableComboBox.DataSource == null)
                SetDestinationTableComboBox();
            SetAgainstColumnOptions();
        }

        private void AddRelationshipButton_Click(object sender, EventArgs e)
        {
            if (!IsRelationshipValid())
                return;
            var sourceTable = SourceTableComboBox.SelectedItem as TableTemplate;
            var sourceColumn = SourceColumnComboBox.SelectedItem as ColumnTemplate;

            var destinationTable = DestinationTableComboBox.SelectedItem as TableTemplate;
            var destinationColumn = DestinationColumnComboBox.SelectedItem as ColumnTemplate;

            var typeString = RelationTypeComboBox.SelectedItem as string;
            RelationshipTypes type = (RelationshipTypes)Enum.Parse(typeof(RelationshipTypes), typeString);

            var name = $"{sourceTable.Name}_{sourceColumn.Name} - {destinationTable.Name}_{destinationColumn.Name}";
            var relation = new RelationshipTemplate
            {
                Name = name,
                FromTable = sourceTable.Name,
                FromColumn = sourceColumn.Name,
                ToTable = destinationTable.Name,
                ToColumn = destinationColumn.Name,
                Type = type
            };

            try
            {
                DatasetTemplate.AddRelationship(relation);
                RelationshipsDataGridView.Rows.Add(new string[] { relation.FromTable, relation.FromColumn, relation.ToTable, relation.ToColumn, relation.Type.ToString() });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RelationshipsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RelationshipsDataGridView.Columns.Count - 1 == e.ColumnIndex)
            {
                var result = MessageBox.Show("Do you really want to delete the relationship?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                var row = RelationshipsDataGridView.Rows[e.RowIndex].Cells;
                var relation = ParseRelationship(row);
                DatasetTemplate.RemoveRelationship(relation);
                RelationshipsDataGridView.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var dataGridView = (DataGridView)sender;
            dataGridView.ClearSelection();
        }

        private void ExpressionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ExpressionTextBox.Enabled = ExpressionCheckBox.Checked;
            if (ExpressionCheckBox.Checked)
                SelectedTable.Type = TableTypes.Calculated;
            else
                SelectedTable.ChangeTypeFromCalculated();

            UpdateColumnDynamicComboBox();

        }

        private void ExpressionTextBox_TextChanged(object sender, EventArgs e)
        {
            SelectedTable.Expression = ExpressionTextBox.Text;
        }

        private void RemoveArtefactButton_Click(object sender, EventArgs e)
        {
            RemoveAllRelationshipsOfTable(SelectedTable);
            DatasetTemplate.RemoveTable(SelectedTable);
            ArtefactsListBox.Items.RemoveAt(ArtefactsListBox.SelectedIndex);
            ArtefactsListBox.SelectedIndex = 0;
            SetSourceTableComboBox();
        }

        private void ColumnDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ColumnDataGridView.Columns.Count - 1 == e.ColumnIndex)
            {
                var result = MessageBox.Show("Do you really want to delete the column?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                var row = ColumnDataGridView.Rows[e.RowIndex].Cells;
                var name = row[0].Value.ToString();
                var type = (ColumnTypes) Enum.Parse(typeof(ColumnTypes),row[1].Value.ToString());
                var isDynamic = Boolean.Parse(row[2].Value.ToString());

                var column = new ColumnTemplate
                {
                    Name = name,
                    Type = type,
                    IsDynamic = isDynamic
                };
                RemoveAllRelationshipsOfColumn(column);
                SelectedTable.RemoveColumn(column);
                ColumnDataGridView.Rows.RemoveAt(e.RowIndex);

                if (SelectedTable.RemoveIdColumn())
                {
                    ColumnDataGridView.Rows.RemoveAt(0);
                }
               

                if (SelectedTable.Name == ((TableTemplate)SourceTableComboBox.SelectedItem).Name)
                    SetSourceColumnComboBox();
                else if (SelectedTable.Name == ((TableTemplate)DestinationTableComboBox.SelectedItem).Name)
                    SetAgainstColumnOptions();
            }
        }
        private void RemoveAllRelationshipsOfTable(TableTemplate table)
        {
            for(var i = 0; i < RelationshipsDataGridView.Rows.Count; i++)
            {
                DataGridViewRow row = RelationshipsDataGridView.Rows[i];
                var rel = ParseRelationship(row.Cells);
                if (rel.ToTable == table.Name || rel.FromTable == table.Name)
                {
                    RelationshipsDataGridView.Rows.Remove(row);
                    DatasetTemplate.RemoveRelationship(rel);
                    i--;
                }
            }
        }

        private void RemoveAllRelationshipsOfColumn(ColumnTemplate column)
        {
            for (var i = 0; i < RelationshipsDataGridView.Rows.Count; i++)
            {
                DataGridViewRow row = RelationshipsDataGridView.Rows[i];
                var rel = ParseRelationship(row.Cells);
                if (rel.ToColumn == column.Name || rel.FromColumn == column.Name)
                {
                    RelationshipsDataGridView.Rows.Remove(row);
                    DatasetTemplate.RemoveRelationship(rel);
                    i--;
                }
            }
        }
        private RelationshipTemplate ParseRelationship(DataGridViewCellCollection row)
        {
            var sourceTable = row[0].Value.ToString();
            var sourceColumn = row[1].Value.ToString();
            var destinationTable = row[2].Value.ToString();
            var destinationColumn = row[3].Value.ToString();

            var relation = new RelationshipTemplate
            {
                FromTable = sourceTable,
                FromColumn = sourceColumn,
                ToColumn = destinationColumn,
                ToTable = destinationTable
            };
            return relation;
        }
        private MeasureTemplate ParseMeasure(DataGridViewCellCollection row)
        {
            var name = row[0].Value.ToString();
            var expression = row[1].Value.ToString();

            var measure = new MeasureTemplate
            {
                Name = name,
                Expression = expression
            };
            return measure;
        }
        private void SetSourceTableComboBox()
        {
            SourceTableComboBox.DataSource = DatasetTemplate.Tables.ToList();
            SourceTableComboBox.DisplayMember = "Name";
            
            if (SourceTableComboBox.Items.Count > 0)
                SourceTableComboBox.SelectedIndex = 0;
        }

        private void SetSourceColumnComboBox()
        {
            SourceColumnComboBox.DataSource = SelectedTable.Columns.ToList();
            SourceColumnComboBox.DisplayMember = "Name";

            if (SourceColumnComboBox.Items.Count > 0)
                SourceColumnComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                DatasetTemplate.Check();
                SaveTemplate(DatasetTemplate);
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MeasuresDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MeasuresDataGridView.Columns.Count - 1 == e.ColumnIndex)
            {
                var result = MessageBox.Show("Do you really want to delete the measure?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                var row = MeasuresDataGridView.Rows[e.RowIndex].Cells;
                var measure = ParseMeasure(row);
                SelectedTable.RemoveMeasure(measure);
                MeasuresDataGridView.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void AddMeasureButton_Click(object sender, EventArgs e)
        {
            var measureName = MeasureNameTextBox.Text;
            var measureExpresion = MeasureExpressionTextBox.Text;
            if (IsMeasureValid())
            {
                try
                {
                    SelectedTable.AddMeasure(new MeasureTemplate { Name = measureName, Expression = measureExpresion });
                    MeasuresDataGridView.Rows.Add(new string[] { measureName, measureExpresion });
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                MeasureNameTextBox.Text = "";
                MeasureExpressionTextBox.Text = "";
            }
        }
        private bool IsMeasureValid()
        {
            if (MeasureNameTextBox.Text == "")
            {
                MessageBox.Show("You must enter measure name.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (MeasureExpressionTextBox.Text == "")
            {
                MessageBox.Show("You must enter measure expression.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void UpdateMeasuresDataGridView()
        {
            MeasuresDataGridView.Rows.Clear();
            foreach (var measure in SelectedTable.Measures)
            {
                var item = new string[] { measure.Name, measure.Expression };
                MeasuresDataGridView.Rows.Add(item);
                if (measure.IsDefault)
                {
                    var style = new DataGridViewCellStyle();
                    style.Padding = new Padding(0, 0, 1000, 0);
                    MeasuresDataGridView.Rows[MeasuresDataGridView.Rows.Count - 1].Cells[2].Style = style;
                }
            }
        }
        private void UpdateColumnsDataGridView()
        {
            ColumnDataGridView.Rows.Clear();
            foreach (var col in SelectedTable.Columns)
            {
                var item = new string[] { col.Name, col.Type.ToString(), col.IsDynamic.ToString() };
                ColumnDataGridView.Rows.Add(item);
                if (col.IsDefault)
                {
                    var style = new DataGridViewCellStyle();
                    style.Padding = new Padding(0, 0, 1000, 0);
                    ColumnDataGridView.Rows[ColumnDataGridView.Rows.Count - 1].Cells[3].Style = style;
                }
            }
        }
        private void UpdateColumnDynamicComboBox()
        {
            ColumnDynamicComboBox.Items.Clear();
            ColumnDynamicComboBox.Items.Add(false);
            ColumnDynamicComboBox.SelectedIndex = 0;

            if (SelectedTable.Type != TableTypes.Calculated)
            {
                ColumnDynamicComboBox.Items.Add(true);
            }
            else
            {
                SelectedTable.SetAllColumnsStatic();
                UpdateColumnsDataGridView();
            }
        }

        private void Button_DragEnter(object sender, DragEventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Color.Bisque;
        }

        private void Button_DragLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Color.White;
        }
    }
}
