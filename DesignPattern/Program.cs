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
            //Singleton1();
            //Builder();
            //Prototype();

            //Adapter();
            //Decorator();
            //Proxy();
            //Facade();
            //Bridge();
            //Composite();
            Flyweight();

            //Strategy();
            //TemplateMethod();
            //Observer();
            //Iterator();
            //ChainOfResponsibility();
            //Command();
            //Memento();
            //State();
            Visitor();
            Mediator();
            Interpreter();

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

        // 工厂方法 定义一个创建对象的接口，但由子类决定需要实例化哪一个类。工厂方法使得子类实例化的过程推迟。
        static void FactoryMethod()
        {
            IFactory factory = new VolunteerFactory();
            LeiFeng lerFeng = factory.CreateLeiFeng();

            lerFeng.Sweep();
            lerFeng.Wash();
            lerFeng.BuyRice();
        }

        // 抽象工厂 提供一个接口，可以创建一系列相关或相互依赖的对象，而无需指定它们具体的类。
        static void AbstractFactory()
        {
            IUser user = DataAccess.CreateUser();
            user.Insert();
            user.Search();
        }

        // 单例模式 保证一个类只有一个实例，并提供一个访问它的全局访问点
        static void Singleton1()
        {
            Director director1 = new Director();
            Director director2 = new Director();
            Console.WriteLine("普通类director1==director2?" + (director1 == director2));

            Singleton sing1 = Singleton.GetInstance();
            Singleton sing2 = Singleton.GetInstance();
            Console.WriteLine("单例类sing1==sing2?" + (sing1 == sing2));
        }

        // 建造者模式 将一个复杂类的表示与其构造相分离，使得相同的构建过程能够得出不同的表示。
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

        // 原型模式 用原型实例指定创建对象的类型，并且通过拷贝这个原型来创建新的对象
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

        // 适配器模式 将一个类的接口转换成用户希望得到的另一种接口。它使原本不相容的接口得以协同工作
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

        // 装饰模式 动态地给一个对象添加一些额外的职责。它提供了用子类扩展功能的一个灵活的替代，比派生一个子类更加灵活
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

        // 代理模式 为其他对象提供一种代理以控制这个对象的访问。
        static void Proxy()
        {
            SchoolGirl girl = new SchoolGirl("李娇娇");
            Proxy proxy = new Proxy(girl);
            proxy.GiveDolls();
            proxy.GiveFlowers();
            proxy.GiveChocolate();
        }

        // 外观模式 定义一个高层接口，为子系统中的一组接口提供一个一致的外观，从而简化了该子系统的使用
        static void Facade()
        {
            Fund fund = new Fund();
            fund.Buy();
            fund.Sell();
        }

        // 桥接模式 将类的抽象部分和它的实现部分分离开来，使它们可以独立地变化
        static void Bridge()
        {
            HandsetBrand ab;

            ab = new HandsetBrandN();
            ab.SetHandsetSoft(new HandsetGame());
            ab.Run();
            ab.SetHandsetSoft(new HandsetAddress());
            ab.Run();

            ab = new HandsetBrandM();
            ab.SetHandsetSoft(new HandsetGame());
            ab.Run();
            ab.SetHandsetSoft(new HandsetAddress());
            ab.Run();
        }

        // 组合模式 将对象组合成树型结构以表示“整体-部分”的层次结构，使得用户对单个对象和组合对象的使用具有一致性
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

        // 享元模式 提供支持大量细粒度对象共享的有效方法。
        static void Flyweight()
        {
            WebSiteFactory factory = new WebSiteFactory();

            WebSite w1 = new ConcreteWebSite("产品展示");
            w1.Use(new User("张三"));
            WebSite w2 = new ConcreteWebSite("产品展示");
            w2.Use(new User("李四"));
            WebSite w3 = new ConcreteWebSite("博客");
            w3.Use(new User("王五"));
            WebSite w4 = new ConcreteWebSite("博客");
            w4.Use(new User("何六"));
        }

        #endregion

        #region 行为型模式

        // 策略模式 定义一系列算法，把它们一个个封装起来，并且使它们之间可互相替换，从而让算法可以独立于使用它的用户而变化。
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

        // 模板方法 定义一个操作中的算法骨架，而将一些步骤延迟到子类中，使得子类可以不改变一个算法的结构即可重新定义算法的某些特定步骤。
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

        // 观察者模式 定义对象间的一种一对多的依赖关系，当一个对象的状态发生改变时，所有依赖于它的对象都得到通知并自动更新。
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

        // 职责链模式 通过给多个对象处理请求的机会，减少请求的发送者与接收者之间的耦合。将接收对象链接起来，在链中传递请求，直到有一个对象处理这个请求。
        static void ChainOfResponsibility()
        {
            CommonManager cm = new CommonManager("经理");
            Majordomo md = new Majordomo("总监");
            GeneralManager gm = new GeneralManager("总经理");
            cm.SetSuperior(md);
            md.SetSuperior(gm);

            Request req1 = new Request()
            {
                Type = RequestType.请假,
                Content = "小菜请假",
                Number = 1
            };
            cm.RequestApplications(req1);

            Request req2 = new Request()
            {
                Type = RequestType.请假,
                Content = "小菜请假",
                Number = 4
            };
            cm.RequestApplications(req2);

            Request req3 = new Request()
            {
                Type = RequestType.加薪,
                Content = "小菜请求加薪",
                Number = 500
            };
            cm.RequestApplications(req3);

            Request req4 = new Request()
            {
                Type = RequestType.加薪,
                Content = "小菜请求加薪",
                Number = 1000
            };
            cm.RequestApplications(req4);
        }

        // 命令模式 将一个请求封装为一个对象，从而可用不同的请求对客户进行参数化，将请求排队或记录请求日志，支持可撤销的操作。 
        static void Command()
        {
            Barbecuer boy = new Barbecuer();
            Command cmd1 = new BakeMuttonCommand(boy);
            Command cmd2 = new BakeChickenWingCommand(boy);

            Waiter girl = new Waiter();
            girl.SetOrder(cmd1);
            girl.SetOrder(cmd2);
            girl.Notify();
        }

        // 备忘录模式 在不破坏封装性的前提下，捕获一个对象的内部状态，并在该对象之外保存这个状态，从而可以在以后将该对象恢复到原先保存的状态。
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

        // 状态模式 允许一个对象在其内部状态改变时改变它的行为。
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

        // 访问者模式 表示一个作用于某对象结构中的各元素的操作，使得在不改变各元素的类的前提下定义作用于这些元素的新操作
        static void Visitor()
        {

        }

        // 中介者模式 用一个中介对象来封装一系列的对象交互。它使各对象不需要显式地相互调用，从而达到低耦合，还可以独立地改变对象间的交互
        static void Mediator()
        {
            UnitedNationsSecurityCouncil unsc = new UnitedNationsSecurityCouncil();

            USA c1 = new USA(unsc);
            Iraq c2 = new Iraq(unsc);
            unsc.Country1 = c1;
            unsc.Country2 = c2;

            c1.Declare("不准研制核武器，否则要发动战争！");
            c2.Declare("我们没有核武器，也不怕侵略。");
        }

        // 解释器模式 给定一种语言，定义它的文法表示，并定义一个解释器，该解释器用来根据文法表示来解释语言中的句子。
        static void Interpreter()
        {

        }

        #endregion
    }
}
