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
            //FactoryMethod();

            //Decorator();
            Proxy();

            //Strategy();

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

        #region 结构型模式

        // 装饰模式
        static void Decorator()
        {
            Person person = new Person("小菜");
            TShirts tshirt = new TShirts();
            Trouser trouser = new Trouser();
            Sneaker sneaker = new Sneaker();
            sneaker.Decorate(person);
            trouser.Decorate(sneaker);
            tshirt.Decorate(trouser);
            tshirt.Show();

            Suit suit = new Suit();
            Tie tie = new Tie();
            LeatherShoes leatherShoes = new LeatherShoes();
            leatherShoes.Decorate(person);
            tie.Decorate(leatherShoes);
            suit.Decorate(tie);
            suit.Show();
        }

        // 代理模式
        static void Proxy()
        {
            SchoolGirl girl = new SchoolGirl("李娇娇");
            Proxy proxy = new Proxy(girl);
            proxy.GiveDolls();
            proxy.GiveFlowers();
            proxy.GiveChocolate();
        }

        #endregion

        #region 行为型模式

        // 策略模式
        static void Strategy()
        {
            CashNormal cashNormal = new CashNormal();
            CashContext contextNormal = new CashContext(cashNormal);
            Console.WriteLine("700:" + contextNormal.GetResult(700));

            CashRebate cashRebate = new CashRebate("0.8");
            CashContext contextRebate = new CashContext(cashRebate);
            Console.WriteLine("700打8折:" + contextRebate.GetResult(700));

            CashReturn cashReturn = new CashReturn("300", "100");
            CashContext contextReturn = new CashContext(cashReturn);
            Console.WriteLine("700满300减100:" + contextReturn.GetResult(700));
        }

        #endregion
    }
}
