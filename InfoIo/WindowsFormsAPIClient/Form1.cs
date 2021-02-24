using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAPIClient
{
    public partial class Form1 : Form
    {

        string creativestoken = "";
        string token = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                APIClient api = new APIClient(txtEndPoint.Text);
                QueryResult result = api.executeQuery(this.getQueryData(), token);
                for(int i=0; i <= result.Data.Columns.Count - 1; i++)
                {
                    string header = this.getColumnHeader(result.Columns, result.Data.Columns[i].ColumnName);
                    result.Data.Columns[i].ColumnName = header;
                }
                txtResults.Text = "Resultados - Fila 1: " + System.Environment.NewLine;
                DataRow row = result.Data.Rows[0];
                for(int i=0; i <= result.Data.Columns.Count - 1; i++)
                {
                    txtResults.Text += result.Data.Columns[i].ColumnName + ": " + row[i].ToString() + System.Environment.NewLine;
                }
                txtResults.Text += (result.Data.Rows.Count - 1).ToString() + " filas más ...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void btnCreatives_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                APIClient api = new APIClient(txtEndPoint.Text);
                var result = api.getCreatives(creativestoken, "SUPLEM. Y DOMINICALES", "3212664");
                txtResults.Text = result.Headers.ContentLength.ToString() + " bytes recibidos";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                APIClient api = new APIClient(txtEndPoint.Text);
                Filter_Request req = new Filter_Request();
                req.Group = "Grupo Marcas";
                req.VariableName = "Producto";
                req.FilterValue = "limp";
                VariableFiltro values = api.getFilterValues(req, token);
                txtResults.Text = "";
                for (int i = 0; i <= values.FilterValues.Count - 1; i++)
                {
                    txtResults.Text += values.FilterValues[i] + System.Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void btnToken_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                APIClient api = new APIClient(txtEndPoint.Text);
                AuthRequest req = new AuthRequest();
                req.ApiKey = "apikey";
                req.UserName = "username";
                req.Password = "base64encodedpassword";
                AuthToken tk = api.getAuthToken(req);
                txtResults.Text = "Creatives Token: " + tk.creatives_Token + System.Environment.NewLine +  "Token: " + tk.Token + System.Environment.NewLine + "Expires: " + tk.Expires;
                creativestoken = tk.creatives_Token;
                token = tk.Token;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void btnVariables_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                APIClient api = new APIClient(txtEndPoint.Text);
                List<Variable> vars = api.getVariables(token);
                txtResults.Text = "";
                for(int i=0; i<= vars.Count-1; i++)
                {
                    Variable var = vars[i];
                    txtResults.Text += "Group: " + var.Group + " / VariableName: " + var.VariableName + System.Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;  
        }

        private string getColumnHeader(List<ColumnInfo> columns, string columnKey)
        {
            string ret = "";
            try
            {
                for(int i=0; i<=columns.Count-1; i++)
                {
                    if (columns[i].ColumnName.Equals(columnKey))
                    {
                        ret = columns[i].ColumnHeader;
                        break;
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        private string getQueryData()
        {
            string ret = "{" + 
                "\"Name\": \"API test\"," +
                "\"Options\": {" +
                    "\"Consolidated\": false, " +
                    "\"Date_From\": \"2019-01-01\", " + 
                    "\"Date_To\": \"2019-01-31\"," + 
                    "\"Measures\": {" +
                        "\"Inserciones\": false, " + 
                        "\"InvEstudioInfoadex\": false, " + 
                        "\"InvTarifa\": true, " + 
                        "\"Ocupacion\": false " +
                    "}" + 
                "}," + 
                "\"Rows\": [" +
                    "{" +
                        "\"Group\": \"Grupo Medios\"," +
                        "\"VariableName\": \"Soporte\"" + 
                    "}," +
                    "{" +
                        "\"Group\": \"Grupo Marcas\"," + 
                        "\"VariableName\": \"Marca\"" + 
                    "}," +
                    "{" +
                        "\"Group\": \"Grupo Marcas\"," + 
                        "\"VariableName\": \"Modelo\"" + 
                    "}," +
                    "{" +
                        "\"Group\": \"Grupo Marcas\"," + 
                        "\"VariableName\": \"Anunciante\"" +
                    "}" +
                "]," +
                "\"Filter\": [" + 
                    "{" + 
                        "\"FilterValues\": [" + 
                            "\"INTERNET\"" + 
                        "]," + 
                        "\"Group\": \"Grupo Medios\"," + 
                        "\"VariableName\": \"Medio\" " + 
                    "}" + 
                "]" + 
            "}";
            return ret;
        }

    }
}
