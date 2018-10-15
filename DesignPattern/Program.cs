using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            FactoryMethod();

            Console.ReadKey();
        }

        #region 创建型模式

        // 工厂方法
        static void FactoryMethod()
        {
            Operation operate = OperationFactory.createOperate("+");
            operate.NumberA = 1;
            operate.NumberB = 2;
            Console.WriteLine("1+2=" + operate.GetResult());

            operate = OperationFactory.createOperate("-");
            operate.NumberA = 1;
            operate.NumberB = 2;
            Console.WriteLine("1-2=" + operate.GetResult());

            operate = OperationFactory.createOperate("*");
            operate.NumberA = 1;
            operate.NumberB = 2;
            Console.WriteLine("1*2=" + operate.GetResult());

            operate = OperationFactory.createOperate("/");
            operate.NumberA = 1;
            operate.NumberB = 2;
            Console.WriteLine("1/2=" + operate.GetResult());
        }

        #endregion
    }
}
