using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BaseDeDatos
{
    /// <summary>
    /// Clase que contiene un label y un control para encapsularlos y manejarlos como un solo control para facilitar el entendimiento del código
    /// </summary>
    public class Controles
    {
        protected Label label;
        protected Control controlPrincipal;
        protected uint tamañoPorcentajeY;   //Indica en porcentaje el tamaño que va a adoptar el control automaticamente cuando cambie el tamaño de la ventana
        protected uint tamañoPorcentajeX;
       
        
        /// <summary>
        /// Propiedad que regresa obtiene o asigna el tamaño del control encapsulado
        /// </summary>
        public Point tamControl
        {
            get { return this.tamañoControl(); }
            set
            {
                if (value.X != -1)
                {
                    this.controlPrincipal.Width = value.X;
                }
                this.controlPrincipal.Height = value.Y-this.label.Height;

            }
        }
        /// <summary>
        /// Propiedad que regresa las coordenadas actuales del control encapsulado
        /// </summary>
        public Rectangle coordenadas
        {
            get
            {
                return new Rectangle(this.label.Location.X,
                                    this.label.Location.Y,
                                    this.controlPrincipal.Location.X + this.controlPrincipal.Width,
                                    this.label.Location.Y + this.label.Height + this.controlPrincipal.Height);
            }
        }

        protected Controles()
        {
            this.label = new Label();
        }

        /// <summary>
        /// Inicializa el label y el control añadiendolos a la forma donde se van a mostrar
        /// </summary>
        /// <param name="f"> ventana padre del los controles</param>
        /// <param name="text"> texto que se le dará al label</param>
        protected void inicializaControl(Form f,string text)
        {
            this.label.Text = text;
            this.label.Size = (TextRenderer.MeasureText(this.label.Text, this.label.Font));
            f.Controls.Add(this.label);
            f.Controls.Add(this.controlPrincipal);
        }

        /// <summary>
        /// Calcula el tamaño de los controles label y control como uno solo
        /// </summary>
        /// <returns>Tamaño en x y en y</returns>
        private Point tamañoControl()
        {
            Point tam = new Point();
            tam.X = this.controlPrincipal.Size.Width;
            tam.Y = this.label.Size.Height + this.controlPrincipal.Size.Height;

            return tam;
        }

        /// <summary>
        /// Asigna una nueva ubicación al control encapsulado(label y control)
        /// </summary>
        /// <param name="x">posicion en x</param>
        /// <param name="y">posición en y</param>
        public void ubicacion(int x, int y)
        {
            this.label.Location = new Point(x, y);
            this.controlPrincipal.Location = new Point(x, y + this.label.Size.Height);
        }

        public Point TamañoPorcentaje()
        {
            Point p = new Point((int)this.tamañoPorcentajeX,(int)this.tamañoPorcentajeY);
            return p;
        }

        ~Controles()
        {
            this.label.Dispose();
            this.controlPrincipal.Dispose();
        }
    }
}
