using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AutoFinit.AppFunction;
using AutoFinit.Pojos;
using AutoFinit.Resources;
using System.Xml.Linq;

namespace AutoFinit
{
    public partial class Formulario : System.Web.UI.Page
    {
        string alfabeto;
        string estado;
        string estadoInicial;
        string estadoFinal;
        List<string> transiciones = new List<string>();
        List<string> transicion = new List<string>();

        private List<TransicionAFD> funcionesTransicionAutomata;
        private List<TransicionAFD> nuevosEstadosAutomata;
        private String estadoInicialAutomataFinitoDeterminista;
        private List<String> simbolosAutomataND;

        string alfabeto2;
        string estado2;
        string estadoInicial2;
        string estadoFinal2;
        List<string> transiciones2 = new List<string>();
        string Transit;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LeerArchivoXml();
                estadoAFN.Text = estado;
                alfabetoAFN.Text = alfabeto;
                estadoInicialAFN.Text = estadoInicial;
                estadoFinalAFN.Text = estadoFinal;
                CargarTablaTransicionesAFN();
                ConvertirAFN_AFD();

                estadoAFD.Text = estado2;
                alfabetoAFD.Text = alfabeto2;
                estadoInicialAFD.Text = estadoInicial2;
                estadoFinalAFD.Text = estadoFinal2;
                CargarTablaTransicionesAFD();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private void LeerArchivoXml()
        {
            string rutaArchivo = @"C:\MisArchivos\archivoXML.txt";
            StreamReader Document = new StreamReader(rutaArchivo);
            if (File.Exists(rutaArchivo))
            {
                string line;
                while ((line = Document.ReadLine()) != null)
                {
                    string nodo1 = "<ALFABETO>";
                    string nodo2 = "<ESTADO>";
                    string nodo3 = "<INICIAL>";
                    string nodo4 = "<FINAL>";
                    string nodo5 = @"(<TRANSICIONES>)?([\s]*)?([a-z0-9,&\s])?([\s]*)?(<\/TRANSICIONES>)?";
                    Regex exp = new Regex(nodo5);
                    if (line.IndexOf(nodo1) != -1)
                    {
                        string finNodo = "</ALFABETO>";
                        int FinAlfabeto = line.IndexOf(finNodo);
                        alfabeto = line.Substring(nodo1.Length+1, FinAlfabeto - finNodo.Length).Trim();
                    }
                    else if(line.IndexOf(nodo2) != -1)
                    {
                        string finNodo = "</ESTADO>";
                        int FinEstado = line.IndexOf(finNodo);
                        estado = line.Substring(nodo2.Length + 1, FinEstado - finNodo.Length).Trim();
                    }
                    else if (line.IndexOf(nodo3) != -1)
                    {
                        string finNodo = "</INICIAL>";
                        int FinEstadoIni = line.IndexOf(finNodo);
                        estadoInicial = line.Substring(nodo3.Length + 1, FinEstadoIni - finNodo.Length).Trim();
                    }
                    else if (line.IndexOf(nodo4) != -1)
                    {
                        string finNodo = "</FINAL>";
                        int FinEstadoFin = line.IndexOf(finNodo);
                        estadoFinal = line.Substring(nodo4.Length + 1, FinEstadoFin - finNodo.Length).Trim();
                    }
                    else if (exp.IsMatch(line))
                    {
                        string pattern = @"([\s]*)?[a-z0-9,&\s]([\s]*)?";
                        Regex test = new Regex(pattern);
                        if (test.IsMatch(line))
                        {
                            transiciones.Add(line.Trim());
                            transicion.AddRange(line.Split(new Char[] {',', ' ' }));
                        }
                    }
                }
                Document.Close();
            }
        }

        private void CargarTablaTransicionesAFN()
        {
            TableHeaderRow headerRow = new TableHeaderRow();
            headerRow.BackColor = Color.LightSkyBlue;

            TableHeaderCell headerTableCell = new TableHeaderCell();
            headerTableCell.Text = "Tabla de Transiciones";
            headerTableCell.Scope = TableHeaderScope.Column;
            headerTableCell.AbbreviatedText = "Tabla de Transiciones";

            headerRow.Cells.Add(headerTableCell);
            tabla1.Rows.Add(headerRow);
            foreach (string transicion in transiciones)
            {
                TableRow tRow = new TableRow();
                TableCell tCell = new TableCell();
                tabla1.Rows.Add(tRow);
                tRow.Cells.Add(tCell);
                tCell.Controls.Add(new LiteralControl(transicion));
            }
        }

        private void ConvertirAFN_AFD()
        {
            List<string> estados = new List<string>();
            List<string> alfabetos = new List<string>();
            List<string> estadosAceptacion = new List<string>();

            estados.AddRange(estado.Split(','));
            alfabetos.AddRange(alfabeto.Split(','));
            simbolosAutomataND = alfabetos;
            estadosAceptacion.AddRange(estadoFinal.Split(','));

            List<Transicion> listaFuncTransicionAutomataND = new List<Transicion>();
            int size = transicion.Count / 3;

            // Reestructura las funciones de transición del AFN en base a la información obtenida del archivo TXT.
            int index = 0;
            for (int i = 0; i < size; i++)
            {
                listaFuncTransicionAutomataND.Add(new Transicion(transicion[index], transicion[index + 1], transicion[index + 2]));
                index += 3;
            }

            // Llama a los métodos que ejecutan el algoritmo de conversión de AFN a AFD.
            Automata automata = new Automata();
            this.funcionesTransicionAutomata = automata.ConvertirAFNaAFD(estados, alfabetos, estadoInicial, estadosAceptacion, listaFuncTransicionAutomataND, ref this.nuevosEstadosAutomata);

            // Arma la tabla e imprime en el grid las funciones de transición generadas del Autómata Finito Determinista.

            DataTable table = new DataTable();
            table.Columns.Add("ESTADOS", typeof(String));
            // Arma el listado de elementos del alfabeto de las funciones de transición.
            foreach (String stade in alfabetos)
            {
                table.Columns.Add(stade, typeof(String));
            }

            // Variables para armar los Strings que contendrán las especificaciones de cada elemento de la quíntupla del nuevo AFD.
            String AFDestados = String.Empty;
            String AFDelementos = String.Empty;
            String AFDinicial = String.Empty;
            String AFDAceptacion = String.Empty;
            String AFDFuncionesTransicion = String.Empty;

            // Recorre cada una de las funciones de transición del nuevo AFD, concatenando el estado, hacia donde
            // se dirige con cada elemento del alfabeto y la composición de subestados de cada uno de los nuevos estados
            // para agregar cada uno de esos elementos a un row que será añadido a la tabla desplegada por el dataGridView.
            foreach (TransicionAFD stade in nuevosEstadosAutomata)
            {
                List<String> elementosTabla = new List<String>();
                elementosTabla.Add(stade.estado);
                AFDestados += "," + stade.estado;
                if (stade.aceptacion)
                {
                    AFDAceptacion += "," + stade.estado;
                }

                // Concatena cada uno de los estados destino para cada elemento del alfabeto.
                foreach (TransicionAFD transition in funcionesTransicionAutomata.Where(f => f.estado == stade.estado).ToList())
                {
                    elementosTabla.Add(transition.proximoEstado);
                    AFDFuncionesTransicion += System.Environment.NewLine + String.Format("\t,({0},{1},{2})", transition.estado, transition.simbolo, transition.proximoEstado);
                }
                String compEstados = String.Empty;

                // Arma la composición de subestados para cada uno de los nuevos estados del AFD.
                foreach (String e in stade.componentes)
                {
                    if (String.IsNullOrEmpty(compEstados))
                    {
                        compEstados += e;
                    }
                    else
                    {
                        compEstados += "," + e;
                    }
                }
                table.Rows.Add(elementosTabla.ToArray());
            }
            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
            
            // Formatea la quíntupla del AFD.
            AFDestados = AFDestados.Trim(new Char[] {'{', '}'}).ToString();
            estado2 = AFDestados.Remove(0, 1);
            AFDAceptacion = AFDAceptacion.Trim(new Char[] { '{', '}' }).ToString();
            estadoFinal2 = AFDAceptacion.Trim(new Char[] { ',' }).ToString();

            List<String> listaTransitions = new List<String>();
            AFDFuncionesTransicion = AFDFuncionesTransicion.Replace(" ", "");
            char[] Transitions = AFDFuncionesTransicion.ToCharArray();
            for (int x = 0; x < Transitions.Length; x++)
            {
                listaTransitions.Add(Transitions[x].ToString());
            }
            int tamaño = listaTransitions.Count / 11;
            int index2 = 0;
            for (int i = 0; i < tamaño; i++)
            {
                transiciones2.Add(listaTransitions[index2 + 5].ToString()+","+listaTransitions[index2 + 7].ToString()+","+listaTransitions[index2 + 9].ToString());
                index2 += 11;
            }

            foreach (String simbolo in alfabetos)
            {
                AFDelementos += "," + simbolo.Trim();
            }
            alfabeto2 = AFDelementos.Remove(0, 1);

            AFDinicial = nuevosEstadosAutomata.FirstOrDefault().estado;
            estadoInicialAutomataFinitoDeterminista = AFDinicial;
            estadoInicial2 = estadoInicialAutomataFinitoDeterminista;
        }

        private void CargarTablaTransicionesAFD()
        {
            TableHeaderRow headerRow2 = new TableHeaderRow();
            headerRow2.BackColor = Color.LightSkyBlue;

            TableHeaderCell headerTableCell2 = new TableHeaderCell();
            headerTableCell2.Text = "Tabla de Transiciones";
            headerTableCell2.Scope = TableHeaderScope.Column;
            headerTableCell2.AbbreviatedText = "Tabla de Transiciones";

            headerRow2.Cells.Add(headerTableCell2);
            tabla2.Rows.Add(headerRow2);
            foreach (string transicion in transiciones2)
            {
                TableRow tRow2 = new TableRow();
                TableCell tCell2 = new TableCell();
                tabla2.Rows.Add(tRow2);
                tRow2.Cells.Add(tCell2);
                tCell2.Controls.Add(new LiteralControl(transicion));
            }
        }

        protected void BtnExportar_Click(object sender, EventArgs e)
        {
            string rutaDestino = @"C:\MisArchivos";
            string file_path = @"C:\MisArchivos\documentXML.xml";
            XmlTextWriter writer;
            
            try
            {
                /*
                foreach (string trans in transiciones2)
                {
                    Transit += " "+trans;
                }

                XDocument xml = new XDocument
                    (
                        new XElement("AUTOMATA",
                        new XElement("ESTADO", estado2),
                        new XElement("ALFABETO", alfabeto2),
                        new XElement("INICIAL", estadoInicial2),
                        new XElement("FINAL", estadoFinal2),
                        new XElement("TRANSICIONES", Transit))
                    );
                xml.Save(@"C:\MisArchivos\documentXML.xml");
                */
                
                resultadoAF.Text = "";
                writer = new XmlTextWriter(file_path, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument(true);
                //writer.Indentation = 2;
                writer.WriteStartElement("AUTOMATA");
                writer.WriteElementString("ESTADO", estado2);
                writer.WriteElementString("ALFABETO", alfabeto2);
                writer.WriteElementString("INICIAL", estadoInicial2);
                writer.WriteElementString("FINAL", estadoFinal2);
                createNode(transiciones2, writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                resultadoAF.Text = "XML File created ! ";
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private void createNode(List<String> transition, XmlTextWriter writer)
        {
            writer.WriteStartElement("TRANSICIONES");
            //writer.WriteStartElement("TRANSICION");
            foreach (string trans in transition)
            {
                writer.WriteString("\n\n\n"+trans +"\n\n\n");
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Evento click del boton para comparar la cadena. Recibe el String de la cadena ingresada y llama al metodo encargado de validar
        /// si la cadena es de aceptación o no. Imprime el resultado en un label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnValidar_Click(object sender, EventArgs e)
        {
            if (this.funcionesTransicionAutomata == null)
            {
                this.resultadoAF.Text = "";
                //RangeValidator1.ErrorMessage = "Debe generar un AFD.";
            }
            else
            {
                String contenidoCadenaValidar = this.lenguaje.Text.Trim().Replace(@"\s", "");
                TransicionAFD status = new TransicionAFD();
                status = this.validarCadena(contenidoCadenaValidar, status);
                if (status != null && nuevosEstadosAutomata.Where(s => s.estado == status.proximoEstado).FirstOrDefault().aceptacion)
                {
                    //this.resultadoAF.ForeColor = System.Drawing.Color.Green;
                    this.resultadoAF.Text = "la cadena es correcta";
                }
                else if (status == null)
                {
                    //this.resultadoAF.ForeColor = System.Drawing.Color.Red;
                    this.resultadoAF.Text = "la cadena es incorrecta";
                }
                else
                {
                    //this.resultadoAF.ForeColor = System.Drawing.Color.Red;
                    this.resultadoAF.Text = "la cadena es incorrecta";
                }
            }
        }

        /// <summary>
        /// Valida que la cadena ingresada sea de aceptación o no.
        /// </summary>
        /// <param name="cadenaValidar">Cadena a validar.</param>
        /// <param name="status">Estado resultante al terminar de recorrer cada elemento de la cadena a validar.</param>
        /// <param name="index">Indice del elemento de la cadena que se está evaluando.</param>
        /// <returns></returns>
        private TransicionAFD validarCadena(String cadenaValidar, TransicionAFD status)
        {
            // Se utiliza como flag para que el primer elemento de la cadena inicie su recorrido desde el primer estado el autómata.
            int contador = 0;

            // Si se recibe una cadena vacía (lambda) y el primer estado es de aceptación, retorna ese primer estado como estado resultante.
            if (String.IsNullOrEmpty(cadenaValidar) && funcionesTransicionAutomata.Where(s => s.estado == estadoInicialAutomataFinitoDeterminista).FirstOrDefault().aceptacion)
            {
                status = funcionesTransicionAutomata.Where(s => s.estado == estadoInicialAutomataFinitoDeterminista).FirstOrDefault();
                return status;
            }
            else if (String.IsNullOrEmpty(cadenaValidar))
            {
                return null;
            }

            // Recorre cada elemento de la cadena, posición por posición, y devuelve el estado resultante al terminar de recorrer la cadena.
            foreach (Char elemento in cadenaValidar)
            {

                // Si el elemento que se evalúa no pertenece al alfabeto original, retorna un estado nulo y sale del método.
                if (simbolosAutomataND.Contains(elemento.ToString()))
                {

                    // Si es el primer elemento de la cadena, se posiciona en el estado inicial del autómata y parte de ese estado.
                    // De lo contrario, sigue recorriendo con el estado en el que se quedó.
                    if (contador == 0)
                    {
                        status = this.funcionesTransicionAutomata.Where(f => f.simbolo == elemento.ToString() && f.estado == this.estadoInicialAutomataFinitoDeterminista).FirstOrDefault();
                    }
                    else
                    {
                        status = this.funcionesTransicionAutomata.Where(f => f.estado == status.proximoEstado && f.simbolo == elemento.ToString()).FirstOrDefault();
                    }
                }
                else
                {
                    return null;
                }

                contador = 1;

            }

            return status;
        }
    }
}