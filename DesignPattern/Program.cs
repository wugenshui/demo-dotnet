using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    [Serializable]
    class Student
    {
        public string name { get; set; }
    }

    [Serializable]
    class DemoClass : ICloneable
    {
        public int i = 0;
        public int[] iArr = { 1, 2, 3 };
        public Student student = new Student() { name = "张三" };

        public object Clone()
        {
            return this.MemberwiseClone() as DemoClass;
        }

        public DemoClass Clone2() //深clone
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as DemoClass;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //SimpleFactory();
            FactoryMethod();

            //Decorator();
            //Proxy();

            //Strategy();
            DemoClass a = new DemoClass();
            a.i = 10;
            a.iArr = new int[] { 8, 9, 10 };
            DemoClass b = a.Clone() as DemoClass;
            DemoClass c = a.Clone2();

            // 更改 a 对象的iArr[0], 导致 b 对象的iArr[0] 也发生了变化 而 c不会变化
            a.iArr[0] = 88;

            Console.WriteLine("MemberwiseClone");
            Console.WriteLine(b.i);
            foreach (var item in b.iArr)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Clone2");
            Console.WriteLine(c.i);
            foreach (var item in c.iArr)
            {
                Console.WriteLine(item);
            }



            Console.ReadKey();
        }

        #region 创建型模式

        // 简单工厂
        static void SimpleFactory()
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

        // 工厂方法
        static void FactoryMethod()
        {
            IFactory factory = new VolunteerFactory();
            LeiFeng lerFeng = factory.CreateLeiFeng();

            lerFeng.Sweep();
            lerFeng.Wash();
            lerFeng.BuyRice();
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
