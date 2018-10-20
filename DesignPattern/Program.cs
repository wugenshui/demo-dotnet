using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //SimpleFactory();
            //FactoryMethod();
            //AbstractFactory();
            //Builder();
            //Prototype();

            //Adapter();
            //Decorator();
            //Proxy();
            //Facade();
            //Composite();

            //Strategy();
            //State();
            //TemplateMethod();
            //Observer();
            Iterator();
            //Memento();

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

        // 抽象工厂
        static void AbstractFactory()
        {
            IUser user = DataAccess.CreateUser();
            user.Insert();
            user.Search();
        }

        // 建造者模式
        static void Builder()
        {
            // 客户找到电脑城老板说要买电脑，这里要装两台电脑
            // 创建指挥者和构造者
            Director director = new Director();
            Builder b1 = new ConcreteBuilder1();
            Builder b2 = new ConcreteBuilder2();

            // 老板叫员工去组装第一台电脑
            director.Construct(b1);

            // 组装完，组装人员搬来组装好的电脑
            Computer computer1 = b1.GetComputer();
            computer1.Show();

            // 老板叫员工去组装第二台电脑
            director.Construct(b2);
            Computer computer2 = b2.GetComputer();
            computer2.Show();

            Console.Read();
        }

        // 原型模式
        static void Prototype()
        {
            CloneClass a = new CloneClass();
            a.i = 10;
            a.iArr = new int[] { 8, 9, 10 };
            Console.WriteLine("a 初始化：\ti=" + a.i + "\tiArr=" + string.Join(",", a.iArr));
            CloneClass b = a.Clone() as CloneClass;
            Console.WriteLine("b 浅拷贝a：\ti=" + b.i + "\tiArr=" + string.Join(",", b.iArr));
            CloneClass c = a.DeepClone();
            Console.WriteLine("c 深拷贝a：\ti=" + c.i + "\tiArr=" + string.Join(",", c.iArr));

            a.i = 15;
            a.iArr[0] = 88;
            Console.WriteLine("a.i=" + a.i);
            Console.WriteLine("a.iArr[0]=" + a.iArr[0]);
            Console.WriteLine("b 浅拷贝a：\ti=" + b.i + "\tiArr=" + string.Join(",", b.iArr));
            Console.WriteLine("c 深拷贝a：\ti=" + c.i + "\tiArr=" + string.Join(",", c.iArr));
            Console.WriteLine("浅拷贝复制了值类型的值和引用类型的引用");
        }

        #endregion

        #region 结构型模式

        // 适配器模式
        static void Adapter()
        {
            Player b = new Forwards("巴蒂尔");
            b.Attack();

            Player m = new Guards("麦克格雷迪");
            m.Attack();

            Player y = new Translator("姚明");
            y.Attack();
            y.Defence();
        }

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

        // 外观模式
        static void Facade()
        {
            Fund fund = new Fund();
            fund.Buy();
            fund.Sell();
        }

        // 组合模式
        static void Composite()
        {
            ConcreteCompany root = new ConcreteCompany("北京总公司");
            root.Add(new HRDepartment("总公司人力资源部"));
            root.Add(new FinanceDepartment("总公司财务部"));

            ConcreteCompany comp1 = new ConcreteCompany("上海华东分公司");
            comp1.Add(new HRDepartment("上海华东分公司人力资源部"));
            comp1.Add(new FinanceDepartment("上海华东分公司财务部"));
            root.Add(comp1);

            ConcreteCompany comp2 = new ConcreteCompany("杭州办事处");
            comp2.Add(new HRDepartment("杭州办事处人力资源部"));
            comp2.Add(new FinanceDepartment("杭州办事处财务部"));
            root.Add(comp2);

            Console.WriteLine("\n结构图：");
            root.Display(1);

            Console.WriteLine("\n职责：");
            root.LineOfDuty();
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

        // 状态模式
        static void State()
        {
            Work work = new Work();
            work.Hour = 9;
            work.WriteProgram();
            work.Hour = 10;
            work.WriteProgram();
            work.Hour = 11;
            work.WriteProgram();
            work.Hour = 12;
            work.WriteProgram();
            work.Hour = 13;
            work.WriteProgram();
            work.Hour = 14;
            work.WriteProgram();
            work.Hour = 17;

            work.TaskFinish = false;
            work.WriteProgram();
            work.Hour = 19;
            work.WriteProgram();
            work.Hour = 22;
            work.WriteProgram();
        }

        // 模板方法
        static void TemplateMethod()
        {
            Console.WriteLine("学生A答卷:");
            TestPaper testPaperA = new TestPaperA();
            testPaperA.TestQuestion1();
            testPaperA.TestQuestion2();
            testPaperA.TestQuestion3();

            Console.WriteLine("学生B答卷:");
            TestPaper testPaperB = new TestPaperB();
            testPaperB.TestQuestion1();
            testPaperB.TestQuestion2();
            testPaperB.TestQuestion3();
        }

        // 观察者模式
        static void Observer()
        {
            INotifyer notifyer = new Secretary();

            Observer observer1 = new StockObserver("张三", notifyer);
            Observer observer2 = new StockObserver("魏观察", notifyer);

            notifyer.Attach(observer1);
            notifyer.Attach(observer2);

            notifyer.Detach(observer2);
            notifyer.NotifyerState = "老板回来了";
            notifyer.Notify();
        }

        // 迭代器模式 提供一种方法来顺序访问一个聚合对象中的各个元素，而不需要暴露该对象的内部表示
        static void Iterator()
        {
            IList<string> list = new List<string>();
            list.Add("大鸟");
            list.Add("小菜");
            list.Add("行李");
            list.Add("老外");
            list.Add("内部员工");
            list.Add("小偷");

            foreach (var item in list)
            {
                Console.WriteLine("{0} 请买车票！", item);
            }
        }

        // 备忘录模式
        static void Memento()
        {
            GameRole role = new GameRole();
            role.GetInitState();
            role.Display();

            RoleStateCaretaker stateAdmin = new RoleStateCaretaker();
            stateAdmin.Memento = role.SaveState();

            role.Fight();
            role.Display();

            role.RecoveryState(stateAdmin.Memento);
            role.Display();
        }
        #endregion
    }
}
