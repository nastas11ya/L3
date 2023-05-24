using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace L3
{
    class Program
    {
        public static void AddNewSportsman()
        {
            var db = new MyDbContext();
            Console.WriteLine("Введите имя спортсмена:"); string name = Console.ReadLine();
            Console.WriteLine("Введите фамилию спортсмена:"); string surname = Console.ReadLine();
            Sportsman s = new Sportsman() { Name = name, SurName = surname };
            db.Sportsmen.Add(s);
            //db.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Sportsmen] ON");
            db.SaveChanges();
            //db.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [dbo].[Sportsmen] OFF");
        }

        public static void AddNewSubscription()
        {
            var db = new MyDbContext();
            Console.WriteLine("Кому вы хотите добавить абонемент?\nВведите имя спортсмена:");
            string name_sportsman= Console.ReadLine();
            Console.WriteLine("Введите фамилию спортсмена");
            string surname_sportsman = Console.ReadLine();
            Console.WriteLine("Какой абонемент вы хотите добавить?");
            string name = Console.ReadLine();
            Console.WriteLine("Введите дату");string data= Console.ReadLine();

            var sp = db.Sportsmen.Where(p => p.Name == name_sportsman && p.SurName == surname_sportsman).FirstOrDefault();
            Subscription s = new Subscription();
            if (sp != null)
            {
                s.Name = name; s.SportsmanId = sp.Id;s.DataOfIssue = data ;
                db.Subscriptions.Add(s);
            }
            
            db.SaveChanges();

            Subscription sub = db.Subscriptions.Where(p => p.Name == name).FirstOrDefault();
            if(sub != null)
            {
                var list_activities = sub.Activities;
                foreach(var a in list_activities)
                {
                    a.Subscriptions.Add(s);
                }
            }
            db.SaveChanges();
        }

        public static void AddNewActivity()
        {
            var db = new MyDbContext();
            Console.WriteLine("Какой вид активности вы хотите добавить?"); string name_activity= Console.ReadLine();
            Console.WriteLine("Какому виду абонемента вы хотите добавить эту активность?");
            string name_s = Console.ReadLine();

            var a = db.Activities.Where(p => p.Name == name_activity).FirstOrDefault();
            if (a == null)
            {
                a = new Activity { Name = name_activity };
                db.Activities.Add(a);
            }

            var list_s = db.Subscriptions.Where(p => p.Name == name_s);
            foreach (var sp in list_s)
            {
                //a.Subscriptions.Add(sp);
                sp.Activities.Add(a);
            }
            db.SaveChanges();
        }

        public static void DeleteSportsman()
        {
            var db = new MyDbContext();
            Console.WriteLine("Введите имя спотсмена:"); string name = Console.ReadLine();
            Console.WriteLine("Введите фамилию спортсмена:"); string surname = Console.ReadLine();
            Sportsman s=db.Sportsmen.Where(p => p.Name == name && p.SurName==surname).FirstOrDefault();

            //список абонементов спортсмена
            var sub_sportsman = db.Subscriptions.Where(p => p.SportsmanId == s.Id ).FirstOrDefault();
            sub_sportsman.Activities.Clear();
            //foreach (var sub in sub_sportsman)
            //{
            //    sub.Activities.Clear();
            //}
            db.SaveChanges();

            var list_activities= db.Activities;
            foreach (var a in list_activities)
            {
                if (a.Subscriptions.FirstOrDefault() == null)
                    db.Activities.Remove(a);
            }

            db.Sportsmen.Remove(s);
            db.SaveChanges();
        }

        //вроде работает 
        public static void DeleteSubscription()
        {
            var db = new MyDbContext();
            Console.WriteLine("Введите имя спортсмена:"); string name = Console.ReadLine();
            Console.WriteLine("Введите фамилию спортсмена:"); string surname = Console.ReadLine();
            Sportsman s = db.Sportsmen.Where(p => p.Name == name && p.SurName == surname).FirstOrDefault();

            Console.WriteLine("Какой абонемент вы хотите удалить?"); string sub_name = Console.ReadLine();

            Subscription sub=db.Subscriptions.Where(p => p.SportsmanId==s.Id && p.Name==sub_name).FirstOrDefault();
            
            //sub.Activities.Clear();
            //db.SaveChanges();
            //var list_activities = db.Activities;
            //foreach (var a in list_activities)
            //{
            //    if (a.Subscriptions.FirstOrDefault() == null)
            //        db.Activities.Remove(a);
            //}
            db.Subscriptions.Remove(sub);
            db.SaveChanges();

            //если у спортсмена больше нет абонементов, то удалим его
            var subs_for_sportsman = db.Subscriptions.Where(p => p.SportsmanId == s.Id).FirstOrDefault();
            if(subs_for_sportsman==null)
            {
                db.Sportsmen.Remove(s);
            }
            db.SaveChanges();
        }

        public static void DeleteActivity()
        {
            var db = new MyDbContext();
            Console.WriteLine("Какой вид активности вы хотите удалить?"); string name_activity = Console.ReadLine();
            //Console.WriteLine("Какому виду абонемента вы хотите удалить эту активность?");
            //string name_s = Console.ReadLine();

            Activity a=db.Activities.Where(p => p.Name==name_activity).FirstOrDefault();
            a.Subscriptions.Clear();
            db.SaveChanges();

            var list_subscriptions = db.Subscriptions;
            foreach (var s in list_subscriptions)
            {
                if (s.Activities.FirstOrDefault() == null)
                    db.Subscriptions.Remove(s);
            }
            db.Activities.Remove(a);
            db.SaveChanges();

            //если у спортсмена больше нет абонементов, то удалим его
            var list_sportsmen = db.Sportsmen;
            foreach(var s in list_sportsmen)
            {
                if (s.Subscriptions.FirstOrDefault() == null)
                    db.Sportsmen.Remove(s);
            }
            db.SaveChanges();
        }

        public static void EditSportsman()
        {
            var db = new MyDbContext();
            Console.WriteLine("    Данные о ком вы хотите изменить?");
            Console.WriteLine("    Введите имя спортсмена:"); string name = Console.ReadLine();
            Console.WriteLine("    Введите фамилию спортсмена:"); string surname = Console.ReadLine();
            Sportsman s = db.Sportsmen.Where(p => p.Name == name && p.SurName == surname).FirstOrDefault();

            Console.WriteLine("        Что именно вы хотите изменить?\n" +
                    "        1 - редактировать имя\n" +
                    "        2 - редактировать фамилию\n" +
                    "        0 - ничего\n");

            int i = Convert.ToInt32(Console.ReadLine());
            while(i!=0)
            {
                switch (i)
                {
                    case 1:
                        Console.WriteLine("        Введите новое имя:"); string new_name = Console.ReadLine();
                        s.Name = new_name;
                        db.SaveChanges();
                        break;
                    case 2:
                        Console.WriteLine("        Введите новую фамилию:"); string new_surname = Console.ReadLine();
                        s.SurName = new_surname;
                        db.SaveChanges();
                        break;

                    default:
                        break;
                }

                Console.WriteLine("        Что именно вы хотите изменить?\n" +
                    "        1 - редактировать имя\n" +
                    "        2 - редактировать фамилию\n" +
                    "        0 - ничего\n");
                i = Convert.ToInt32(Console.ReadLine());
            }
        }

        public static void EditSubscription()
        {
            var db = new MyDbContext();
            Console.WriteLine("Какой абонемент вы хотите изменить?"); string sub_name = Console.ReadLine();
            Console.WriteLine("Введите дату выдачи:"); string d = Console.ReadLine();
            Subscription sub = db.Subscriptions.Where(p => p.DataOfIssue == d && p.Name == sub_name).FirstOrDefault();

            Console.WriteLine("        Что именно вы хотите изменить?\n" +
                    "        1 - редактировать имя\n" +
                    "        2 - редактировать дату выдачи\n" +
                    "        0 - ничего\n");
            int i = Convert.ToInt32(Console.ReadLine());
            while (i != 0)
            {
                switch (i)
                {
                    case 1:
                        Console.WriteLine("        Введите новое имя:"); string new_name = Console.ReadLine();
                        sub.Name = new_name;
                        db.SaveChanges();
                        break;
                    case 2:
                        Console.WriteLine("        Введите новую дату:"); string new_data = Console.ReadLine();
                        sub.DataOfIssue = new_data;
                        db.SaveChanges();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("        Что именно вы хотите изменить?\n" +
                    "        1 - редактировать имя\n" +
                    "        2 - редактировать дату выдачи\n" +
                    "        0 - ничего\n");
                i = Convert.ToInt32(Console.ReadLine());
            }
        }

        public static void EditActivity()
        {
            var db = new MyDbContext();
            Console.WriteLine("Какую активность вы хотите изменить?"); string act_name = Console.ReadLine();
            var act = db.Activities.Where(p => p.Name == act_name).FirstOrDefault();

            Console.WriteLine("Введите новое имя активности:"); string new_name = Console.ReadLine();
            act.Name = new_name;
            db.SaveChanges();
        }


        public static void Main(string[] args)
        {           
            using (var db = new MyDbContext())
            {
                ////добавление
                //Sportsman s1 = new Sportsman() { Name = "Alex", SurName = "Alekseevskiy" };
                //Sportsman s2 = new Sportsman() { Name = "Nik", SurName = "Golev" };
                ////Sportsman s3 = new Sportsman() { Name = "Nastya", SurName = "Lubaya" };
                //db.Sportsmen.Add(s1); db.Sportsmen.Add(s2); /*db.Sportsmen.Add(s3);*/
                //db.SaveChanges();

                //Subscription sub1 = new Subscription {Name="спортзал", SportsmanId = 2, /*ActivityId = 1,*/ DataOfIssue = "02.01.2022" };
                //Subscription sub2 = new Subscription { Name = "ЛА", SportsmanId = 1, /*ActivityId = 2,*/ DataOfIssue = "23.09.2022" };
                //Subscription sub3 = new Subscription() { Name = "танцы", SportsmanId = 1, /*ActivityId = 1,*/ DataOfIssue = "15.11.2022" };
                //db.Subscriptions.AddRange(new List<Subscription> { sub1, sub2, sub3 });
                //db.SaveChanges();

                //Activity a1 = new Activity { Name = "фитнес" };
                //a1.Subscriptions.Add(sub1);
                //a1.Subscriptions.Add(sub3);
                //Activity a2 = new Activity { Name = "тренажерный зал" };
                //a2.Subscriptions.Add(sub1);
                //a2.Subscriptions.Add(sub2);
                //Activity a3 = new Activity { Name = "стадион" };
                //a3.Subscriptions.Add(sub2);
                //db.Activities.Add(a1); db.Activities.Add(a2); db.Activities.Add(a3);
                //db.SaveChanges();

               

                Console.WriteLine("Что Вы хотите сделать?\n" +
                    "1 - добавить нового спортсмена\n" +
                    "2 - добавить новый абонемент спортсмену\n" +
                    "3 - добавить новую активность(деятельность) абонементу\n" +
                    "4 - удалить спортсмена\n" +
                    "5 - удалить абонемент у спортсмена\n" +
                    "6 - удалить активность\n" +
                    "7 - редактировать данные о спортсмене\n" +
                    "8 - редактировать данные об абонементе\n" +
                    "9 - редактитовать данные об активности\n" +
                    "10 - посмотреть таблицу спортсменов\n" +
                    "11 - посмотреть таблицу абонементов\n" +
                    "12 - посмотреть таблицу активностей\n" +
                    "13 - соответствие абонементов и активностей\n" +
                    "0 - ничего");

                int i = Convert.ToInt32(Console.ReadLine());
                while(i!=0)
                {
                    switch (i)
                    {
                        case 1:
                            AddNewSportsman();
                            break;
                        case 2:
                            AddNewSubscription();
                            break;
                        case 3:
                            AddNewActivity();
                            break;
                        case 4:
                            DeleteSportsman();
                            break;
                        case 5:
                            DeleteSubscription();
                            break;
                        case 6:
                            DeleteActivity();
                            break;
                        case 7:
                            EditSportsman();
                            break;
                        case 8:
                            EditSubscription();
                            break;
                        case 9:
                            EditActivity();
                            break;
                        case 10:
                            do
                            {
                                Console.WriteLine("\nСпортсмены:");
                                var sportsmen = db.Sportsmen.ToList();
                                foreach (var s in sportsmen)
                                {
                                    Console.WriteLine($"{s.Id}. \t{s.Name} \t{s.SurName}");
                                }

                            } while (Console.ReadKey().Key != ConsoleKey.Escape);
                            break;
                        case 11:
                            do
                            {
                                Console.WriteLine("\nАбонементы:");
                                var subscriptions = db.Subscriptions.ToList();
                                foreach (var s in subscriptions)
                                {
                                    Console.WriteLine($"{s.Id}. \t{s.Name} \t{s.SportsmanId} \t{s.DataOfIssue}");
                                }

                            } while (Console.ReadKey().Key != ConsoleKey.Escape);
                            break;
                        case 12:
                            do
                            {
                                Console.WriteLine("\nАктивности:");
                                var activities = db.Activities.ToList();
                                foreach (var a in activities)
                                {
                                    Console.WriteLine($"{a.Id}. \t{a.Name}");
                                }
                            } while (Console.ReadKey().Key != ConsoleKey.Escape);
                            break;
                        case 13:
                            do
                            {
                                Console.WriteLine("\nCоответствие абонементов и активностей:");
                                var sub_list = db.Subscriptions.ToList();
                                foreach (var l in sub_list)
                                {
                                    var act_list = l.Activities.ToList();
                                    foreach (var a in act_list)
                                    {
                                        Console.WriteLine($"{l.Id}. \t{a.Id}");
                                    }
                                }
                            } while (Console.ReadKey().Key != ConsoleKey.Escape);
                            break;
                        default:
                            break;

                    }
                    Console.WriteLine("Что Вы хотите сделать?\n" +
                    "1 - добавить нового спортсмена\n" +
                    "2 - добавить новый абонемент спортсмену\n" +
                    "3 - добавить новую активность(деятельность) абонементу\n" +
                    "4 - удалить спортсмена\n" +
                    "5 - удалить абонемент у спортсмена\n" +
                    "6 - удалить активность\n" +
                    "7 - редактировать данные о спортсмене\n" +
                    "8 - редактировать данные об абонементе\n" +
                    "9 - редактитовать данные об активности\n" +
                    "10 - посмотреть таблицу спортсменов\n" +
                    "11 - посмотреть таблицу абонементов\n" +
                    "12 - посмотреть таблицу активностей\n" +
                    "13 - соответствие абонементов и активностей\n" +
                    "0 - ничего");

                    i = Convert.ToInt32(Console.ReadLine());
                }
                
            }
        }
    }
}
