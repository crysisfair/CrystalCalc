using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.IO;
using Antlr4.Runtime.Misc;

namespace Antlr
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = null;
            TextReader reader = Console.In;
            if(args.Count() > 0)
            {
                file = args[0];
                reader = new StreamReader(file);
            }
            try
            {
                var ai = new AntlrInputStream(reader);
                var lexer = new ExprLexer(ai);
                var tokens = new CommonTokenStream(lexer);
                var parser = new ExprParser(tokens);
                var tree = parser.prog();
                CalcVisitor calc = new CalcVisitor();
                calc.Visit(tree);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class CalcVisitor : ExprBaseVisitor<int>
    {
        protected Dictionary<string, int> variables = new Dictionary<string, int>();

        public override int VisitInt([NotNull] ExprParser.IntContext context)
        {
            return int.Parse(context.INT().GetText());
        }

        public override int VisitId([NotNull] ExprParser.IdContext context)
        {
            string id = context.ID().GetText();
            if(variables.Keys.Contains(id))
            {
                return variables[id];
            }
            else
            {
                throw new Exception(String.Format("{0} has no definition. {1}", id, context.Start));
            }
        }

        public override int VisitMulDiv([NotNull] ExprParser.MulDivContext context)
        {
            int left = Visit(context.expr(0));
            int right = Visit(context.expr(1));
            int res = 0;
            if(context.op.Type == ExprParser.MUL)
            {
                res = left * right;
            }
            else
            {
                if(right == 0)
                {
                    throw new DivideByZeroException(String.Format("{0} is divided by zero. {1}", left, context.Start));
                }
                res = left / right;
            }
            return res;
        }

        public override int VisitAddSub([NotNull] ExprParser.AddSubContext context)
        {
            int left = Visit(context.expr(0));
            int right = Visit(context.expr(1));
            int res = 0;
            if(context.op.Type == ExprParser.ADD)
            {
                res = left + right;
            }
            else
            {
                res = left - right;
            }
            return res;
        }

        public override int VisitAssign([NotNull] ExprParser.AssignContext context)
        {
            string id = context.ID().GetText();
            int value = Visit(context.expr());
            if(variables.Keys.Contains(id))
            {
                variables[id] = value;
            }
            else
            {
                variables.Add(id, value);
            }
            if (context.end.Text == ";")
            {
                //Console.WriteLine("DO NOT PRINT");
            }
            else
            {
                Console.WriteLine("{0} = {1}", id, value);
            }
            return value;
        }

        public override int VisitPrintExpr([NotNull] ExprParser.PrintExprContext context)
        {
            int value = Visit(context.expr());
            if (context.end.Text == ";")
            {
                Console.WriteLine("DO NOT PRINT");
            }
            else
            {
                Console.WriteLine(value);
            }
            return value;
        }

        public override int VisitParens([NotNull] ExprParser.ParensContext context)
        {
            int value = Visit(context.expr());
            return value;
        }

    }
}
