using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoFinit.Pojos;

namespace AutoFinit.AppFunction
{
    public class Automata
    {
        /// <summary>
        /// Array con todos los valores de los posibles nuevos estados al generar el AFD.
        /// </summary>
        private readonly String[] STATUS_VALUES = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// Indice del Array STATUS_VALUES, para ubicar cual debe ser el siguiente nuevo estado durante la generación del AFD.
        /// </summary>
        public int STATUS_INDEX = 0;

        /// <summary>
        /// Inicializa el proceso para generar el AFD a partir del AFN ó AFN-E.
        /// </summary>
        /// <param name="estadosAutomataND">Todos los estados que posee el AFN</param>
        /// <param name="simbolosAutomataND">Alfabeto del AFN</param>
        /// <param name="estadoInicialAutomataND">Estado inicial del AFN</param>
        /// <param name="estadosAceptacionAutomataND">Estados de aceptación del AFN</param>
        /// <param name="listaFuncTransicionAutomataND">Colección de objetos tipo Transición que contienen las funciones de transición</param>
        public List<TransicionAFD> ConvertirAFNaAFD(List<String> estadosAutomataND, List<String> simbolosAutomataND, String estadoInicialAutomataND, List<String> estadosAceptacionAutomataND, List<Transicion> listaFuncTransicionAutomataND, ref List<TransicionAFD> funcionesTransicionAFD)
        {

            List<TransicionAFD> nuevosEstadosAutomata = new List<TransicionAFD>();
            List<TransicionAFD> funcionesTransicionAutomata = new List<TransicionAFD>();
            List<String> coleccionEstados = new List<String>();
            TransicionAFD newTransicion = new TransicionAFD();

            // Se agrega el estado inicial a la composición del nuevo primer estado, se manda a llamar la función CerraduraEpsilon
            //para obtener el primer estado del AFD y se agrega a la nueva colección de estados del AFD como un estado no marcado.
            coleccionEstados.Add(estadoInicialAutomataND);
            coleccionEstados = this.CerraduraEpsilon(coleccionEstados, listaFuncTransicionAutomataND, coleccionEstados.Count, 0);
            coleccionEstados.Sort();
            newTransicion.componentes = coleccionEstados;
            newTransicion.marca = false;
            newTransicion.aceptacion = false;
            foreach (String estado in newTransicion.componentes)
            {
                if (estadosAceptacionAutomataND.Contains(estado))
                {
                    newTransicion.aceptacion = true;
                    break;
                }
            }
            newTransicion.estado = STATUS_VALUES[STATUS_INDEX];
            STATUS_INDEX++;
            nuevosEstadosAutomata.Add(newTransicion);
            List<String> listaEstados = new List<String>();

            // Se inicializa el recorrido de los estados sin marcar de la tabla T.
            //this.ObtenerEstadosAlfabeto(nuevosEstadosAutomata, simbolosAutomataND, listaFuncTransicionAutomataND, listaEstados, funcionesTransicionAutomata, nuevosEstadosAutomata.Count, 0, false);

            IList<TransicionAFD> estadosSinMarcar = new List<TransicionAFD>();
            do
            {
                TransicionAFD estadoActual = nuevosEstadosAutomata.Where(q => q.marca == false).FirstOrDefault();
                estadoActual.marca = true;

                foreach (String simbolo in simbolosAutomataND)
                {
                    listaEstados = Mueve(estadoActual, simbolo, listaFuncTransicionAutomataND);
                    listaEstados = CerraduraEpsilon(listaEstados, listaFuncTransicionAutomataND, listaEstados.Count, 0);

                    if (listaEstados.Count == 0)
                    {
                        listaEstados.Add("-Vacio-");
                    }

                    // Se verifica si el posible nuevo estado encontrado existe en la tabla de nuevos estados del AFD
                    TransicionAFD newTrans;
                    bool flagExists = false;
                    TransicionAFD posibleNuevoEstado = nuevosEstadosAutomata.Where(q => q.componentes.SequenceEqual(listaEstados)).FirstOrDefault();
                    if (posibleNuevoEstado != null)
                    {
                        flagExists = true;
                    }

                    // Verifica si la lista de estados contiene al menos un estado de aceptación y guarda el resultado en un flag
                    bool aceptacion = false;
                    foreach (String estado in listaEstados)
                    {
                        if (estadosAceptacionAutomataND.Contains(estado))
                        {
                            aceptacion = true;
                            break;
                        }
                    }

                    // Si el nuevo estado generado no existe en la Tabla T, se crea el nuevo estado y se añade a la lista de
                    // nuevos estados del AFD, y luego, se añade la funcion de transicion a la lista correspondiente.
                    // De lo contrario, solo se añade la función de transición.
                    if (!flagExists)
                    {
                        // Se crea el nuevo estado generado y se agrega a la Tabla T de estados del AFD
                        newTrans = new TransicionAFD();
                        newTrans.estado = STATUS_VALUES[STATUS_INDEX];
                        STATUS_INDEX++;
                        newTrans.componentes = listaEstados;
                        newTrans.marca = false;
                        newTrans.aceptacion = aceptacion;
                        nuevosEstadosAutomata.Add(newTrans);

                        // Se añade la funcion de transición a la lista de funciones de transición del AFD
                        TransicionAFD newTrans2 = new TransicionAFD();
                        newTrans2.estado = estadoActual.estado;
                        newTrans2.simbolo = simbolo;
                        newTrans2.proximoEstado = newTrans.estado;
                        newTrans2.componentes = estadoActual.componentes;
                        newTrans2.aceptacion = estadoActual.aceptacion;
                        funcionesTransicionAutomata.Add(newTrans2);
                    }
                    else
                    {
                        // Se añade la funcion de transición a la lista de funciones de transición del AFD
                        newTrans = new TransicionAFD();
                        newTrans.estado = estadoActual.estado;
                        newTrans.simbolo = simbolo;
                        newTrans.proximoEstado = posibleNuevoEstado.estado;
                        newTrans.componentes = estadoActual.componentes;
                        newTrans.aceptacion = aceptacion;
                        funcionesTransicionAutomata.Add(newTrans);
                    }
                }
                estadosSinMarcar = nuevosEstadosAutomata.Where(q => q.marca == false).ToList();
            } while (estadosSinMarcar.Count > 0);

            funcionesTransicionAFD = nuevosEstadosAutomata;

            return funcionesTransicionAutomata;
        }

        /// <summary>
        /// Genera la lista de estados resultantes al movilizarse con epsilon.
        /// </summary>
        /// <param name="coleccionEstados">Colección de todos los estados que componen el estado actual a evaluar.</param>
        /// <param name="listaFuncTransicionAutomataND">Lista de funciones de transición.</param>
        /// <param name="cont">Longitud actual de la colección de estados que se evaluan.</param>
        /// <param name="index">Ultima posición hasta donde se recorrió la lista.</param>
        /// <returns>Lista con los estados resultantes al movilizarse con epsilon.</returns>
        public List<String> CerraduraEpsilon(List<String> coleccionEstados, List<Transicion> listaFuncTransicionAutomataND, int cont, int index)
        {

            int newIndex = index;

            // Recorre todas las funciones de transición que se movilizan con Epsilon con cada estado de la colección.
            for (int i = index; i < cont; i++)
            {
                foreach (Transicion trans in listaFuncTransicionAutomataND.Where(f => f.estado.Equals(coleccionEstados[i]) && f.simbolo.Equals("&") && !coleccionEstados.Contains(f.proximoEstado)))
                {
                    //if (trans.estado.Equals(coleccionEstados[i]) && trans.simbolo.Equals("e") && !coleccionEstados.Contains(trans.proximoEstado))
                    //{
                    coleccionEstados.Add(trans.proximoEstado);
                    //}
                }
                newIndex = i + 1;
            }

            // Si se añadieron nuevos estados, se vuelve a llamar a esta función para verificar si también se movilizan con epsilon.
            int newCont = coleccionEstados.Count;
            if (newCont > cont)
            {
                this.CerraduraEpsilon(coleccionEstados, listaFuncTransicionAutomataND, newCont, newIndex);
            }

            coleccionEstados.Sort();
            return coleccionEstados;
        }

        /// <summary>
        /// Genera la nueva coleccion de estados que se obtiene al moverse desde la composición de un estado del 
        /// Automata Finito Determinista con cada uno de los simbolos del alfabeto.
        /// </summary>
        /// <param name="estadoAutomata">Estado que se evalua en ese momento</param>
        /// <param name="simbolo">Simbolo del alfabeto con el cual se recorre el AFN</param>
        /// <param name="listaFuncTransicionAutomataND">Listado de funciones de transicion del AFN</param>
        /// <returns>Coleccion de estados resultante al moverse con un simbolo del alfabeto con la composicion de estados de un estado nuevo del AFD</returns>
        public List<String> Mueve(TransicionAFD estadoAutomata, String simbolo, List<Transicion> listaFuncTransicionAutomataND)
        {
            // Por cada estado que contenga la composición de estados, por cada función de transición del AFN, verifica que el estado y simbolo sean iguales.
            // De ser así, si el estado destino con esa combinación no existe en la lista de nuevos estados, la añade.
            List<String> listaNuevosEstados = new List<String>();
            foreach (String estado in estadoAutomata.componentes)
            {
                foreach (Transicion transicion in listaFuncTransicionAutomataND)
                {
                    if (transicion.estado.Equals(estado) && transicion.simbolo.Equals(simbolo) && !listaNuevosEstados.Contains(transicion.proximoEstado))
                    {
                        listaNuevosEstados.Add(transicion.proximoEstado);
                    }
                }
            }
            listaNuevosEstados.Sort();
            return listaNuevosEstados;
        }
    }
}