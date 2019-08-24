using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculator
{
    public partial class Form1 : Form
    {

        List<Char> symbols = new List<char>();

        char[] operands = { '+', '-', '*', '/' };
        char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        bool haveOperand = false;
        bool haveMinus = false;
        bool haveZap = false;
        bool pricol = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void toOutput(char symbol = 'n')
        {

            if (symbol != 'n')
            {
                if (operands.Contains(symbol))
                {
                    if (symbols.Count < 1 && symbol != '-')
                        return;

                    else if (symbols.Count > 1 && operands.Contains(symbols[symbols.Count - 1]))
                    {
                         symbols[symbols.Count - 1] = symbol;
                    }
                    else if (!haveOperand)
                    {
                         symbols.Add(symbol);
                        haveZap = false;
                    }

                    if (symbols.Count == 1 && symbol == '-')
                        haveMinus = true;

                    haveOperand = true;
                }
                else if (symbol == ',')
                {
                    if (symbols.Count > 0 && !operands.Contains(symbols[symbols.Count-1]) && !haveZap)
                    {
                        symbols.Add(symbol);

                        haveZap = true;
                    }
                }

                else
                {
                    symbols.Add(symbol);

                    if (haveMinus)
                    {
                        haveMinus =false;
                        haveOperand = false ;
                    }
                }
            }

            mainField.Text = string.Empty;


            foreach(var n in symbols)
            {
                mainField.Text += n.ToString();
            }
        }

        void buttonSymbol(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if(button != null )
            {
                toOutput(button.Text[0]);
            }
        }

        
        
      
        private void buttonClear_Click(object sender, EventArgs e)
        {
            mainField.Text = string.Empty;
            symbols.Clear();
            haveZap = false;
            haveOperand = false;
            haveMinus = false;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            remove();
        }


        private void buttonRavno_Click(object sender, EventArgs e)
        {
            if (haveOperand)
            {
                try
                {
                    if (pricol)
                    {
                        prikol();
                        pricol = false;
                    }
                    else
                        calculate();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Произошла какая-то херня");
                }
                haveZap = false;
                haveOperand = false;
                haveMinus = false;
            }
            //
        }


        private void remove()
        {
            if (operands.Contains(symbols[symbols.Count - 1]))
            {
                haveOperand = false;
                if (symbols.Count == 1)
                    haveMinus = false;
            }

            symbols.RemoveAt(symbols.Count - 1);
            toOutput();
        }
        float result;
        private void calculate()
        {
            string in1 = "";
            bool second = false;
            string in2 = "";
            char operand = '+';


            for(int i = 0; i <symbols.Count; i++)
            {
                if (operands.Contains(symbols[i]))
                {
                    if (i != 0)
                    {
                        operand = symbols[i];
                        second = true;
                        continue;
                    }
                }

                if (!second)
                    in1 += symbols[i].ToString();
                else
                    in2 += symbols[i].ToString();
            }

            if (in1 == "" || in2 == "") return;
            result = 0;
            haveOperand = false;

            if (operand == '/' && in2 == "0")
            {
                mainField.Text = "Бесконечность";
                symbols.Clear();
                return;
            }
            
            switch (operand)
            {
                case '+': result = float.Parse(in1) + float.Parse(in2); break;
                case '-': result = float.Parse(in1) - float.Parse(in2); break;
                case '*': result = float.Parse(in1) * float.Parse(in2); break;
                case '/': result = float.Parse(in1) / float.Parse(in2); break;


            }
            symbols.Clear();
            mainField.Text = (result.ToString());
            for (int i = 0; i<result.ToString().Length; i++)
            {
                symbols.Add(result.ToString()[i]);
            }
            
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string code = e.KeyCode.ToString();
            char outSymb = 'n';

            if (code.Contains("NumPad"))
            {
                outSymb = code[code.Length - 1];
            }
            if (code.Length == 2 && code.Contains('D'))
            {
                outSymb = code[1];
                
            }
            switch (code)
            {
                case "OemMinus": outSymb = '-'; break;
                case "Divide": outSymb = '/'; break;
                case "Multiply": outSymb = '*'; break;
                case "Add": outSymb = '+'; break;
                case "Subtract": outSymb = '-'; break;
                case "Back": remove(); return;


            }

            try
            {
                toOutput(outSymb);
            }

            catch
            {

            }

        }


        int value = 0;
        private void prikol()
        {
            label1.Text = "Подождите, пока компьютер считает введенные вами числа";
            panel1.Visible = true;
            progressBar1.Value = 0;
            Random rand = new Random();
            value = rand.Next(1, 3);
            timer1.Enabled = true;

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if ( progressBar1.Value + value < progressBar1.Maximum )

                progressBar1.Value += value;

            else
            {
                panel1.Visible = false;
                timer1.Enabled = false;
                calculate();

            }
        }
    }



    class Calculator
    {
        List<char> operands = new List<char>{ '+', '-', '*', '/'};
        List<char> numbers = new List<char>{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public List<char> Symbols { get; private set; } = new List<char>();

        private TextBox textBox;


        int OperandsCount
        {
            get
            {
                int n = 0;
                foreach ( var sym in Symbols )
                {
                    if ( operands.Contains(sym) ) n++;
                }
                return n;
            }
        }

        int PointsCount
        {
            get
            {
                int n = 0;
                foreach ( var sym in Symbols )
                {
                    if ( sym == ',' ) n++;
                }
                return n;
            }
        }

        int NumbersCount
        {
            get
            {
                int n = 0;
                foreach ( var sym in Symbols )
                {
                    if ( numbers.Contains(sym) ) n++;
                }
                return n;
            }
        }

        public bool MayPutPoint
        {
            get
            {

                if ( PointsCount == 1 && OperandsCount == 0 || PointsCount == 2 && OperandsCount >= 1 ) // Чтобы не было - 456 + 432,54,43
                    return false;
                else
                    return true;
            }
        }

        
        public bool IsLastSymbolOperandOrPoint
        {
            get
            {
                var symb = Symbols[Symbols.Count - 1];
                return ( operands.Contains(symb) || symb == ',' );
            }
            
        }


        public Calculator(Form form, int x = 32, int y = 30 )
        {
            textBox = new TextBox()
            {
                Name = "MainField",
                ReadOnly = true,
                Font = new Font("Microsoft Sans Serif", 19.8f),
            };
            form.Controls.Add(textBox);
            
        }

        public Calculator(TextBox tb)
        {
            textBox = tb;
        }

        public void Add(char symbol)
        {
            if ( numbers.Contains(symbol) )
            {
                Symbols.Add(symbol);
            }
            else
            {
                if ( Symbols.Count > 0 ) return; //Если еще ничего не введено, то первым символом не может быть операнд

                if ( !IsLastSymbolOperandOrPoint ) //Последний символ -не операнд и не запятая
                {
                    if ( symbol == ',' && MayPutPoint)
                    {
                        Symbols.Add(symbol);

                    }
                    else if (OperandsCount == 0) // и операнда еще нет 
                    {
                        Symbols.Add(symbol);
                    }
                }
            }
            this.Display();
        }
        public void Display()
        {
            foreach(var symb in Symbols )
            {
                textBox.Text += symb;
            }
        }

        public void Calculate()
        {
            var in1 = "";
            var in2 = "";
            double result = 0;
            var second = false;
            var operand = '+';
            foreach(var sym in Symbols )
            {
                if ( operands.Contains(sym) )
                {
                    operand = sym;
                    second = true;
                }

                if ( !second )
                {
                    in1 += sym;
                }
                else
                {
                    in2 += sym;
                }
            }

            if(operand == '/' && in2 == "0" )
            {
                textBox.Text = "Бесконечность";
                Symbols.Clear();
                return;
            }
            try
            {
                switch ( operand )
                {
                    case '+': result = float.Parse(in1) + float.Parse(in2); break;
                    case '-': result = float.Parse(in1) - float.Parse(in2); break;
                    case '*': result = float.Parse(in1) * float.Parse(in2); break;
                    case '/': result = float.Parse(in1) / float.Parse(in2); break;


                }
            }

            catch
            {
                return;
            }

            Symbols.Clear();
            textBox.Text = ( result.ToString() );
            for ( int i = 0; i < result.ToString().Length; i++ )
            {
                Symbols.Add(result.ToString()[i]);
            }
        }

    }
}
