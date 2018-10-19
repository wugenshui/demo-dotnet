﻿using System;
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
            //Prototype();

            //Decorator();
            //Proxy();

            //Strategy();
            TemplateMethod();

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

        #endregion
    }
}
