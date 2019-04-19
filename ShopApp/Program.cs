using System;
using System.Linq;

namespace ShopApp
{
    class Program
    {
        public static int IntParser(int from, int to)
        {
            int chouse;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out chouse) && chouse >= from && chouse <= to)
                    break;

                Console.WriteLine("Неверный ввод");
            }
            return chouse;
        }

        static void Main(string[] args)
        {
            using (var context = new AppContext())
            {
                #region Registration/Login 
                Console.WriteLine("Добро пожаловать в Shop Application!");
                User user = new User();
                int chouse = 0;
                while (true)
                {
                    Console.WriteLine("1 - Для регистрации");
                    Console.WriteLine("2 - Для входа");

                    chouse = IntParser(1, 2);

                    if (chouse == 1)
                    {
                        while (true)
                        {
                            Console.WriteLine("Введите логин");
                            user.Login = Console.ReadLine();

                            Console.WriteLine("Введите пароль");
                            user.Password = Console.ReadLine();

                            Console.WriteLine("Введите Ваше имя");
                            user.Name = Console.ReadLine();

                            bool isLoginExists = false;

                            foreach (var existingUser in context.Users)
                            {
                                if (existingUser.Login == user.Login)
                                {
                                    Console.WriteLine("Человек с таким же логином уже есть");
                                    Console.WriteLine("Введите другой логин!");
                                    isLoginExists = true;
                                    break;
                                }
                            }

                            if (!isLoginExists)
                            {
                                context.Users.Add(user);
                                context.SaveChanges();
                                break;
                            }

                        }
                        break;
                    }
                    else if (chouse == 2)
                    {
                        while (true)
                        {
                            Console.WriteLine("Введите логин");
                            string login = Console.ReadLine();

                            Console.WriteLine("Введите пароль");
                            string password = Console.ReadLine();

                            bool isExcists = false;

                            foreach (var existingUser in context.Users)
                            {
                                if (existingUser.Login == login && existingUser.Password == password)
                                {
                                    user = existingUser;
                                    isExcists = true;
                                    break;
                                }
                            }

                            if (isExcists) break;

                            Console.WriteLine("Логин или пароль введены неверно!\n");
                        }
                        break;
                    }
                }
                #endregion
                
                Console.Clear();
                while (true)
                {
                    Console.WriteLine("\tГлавное меню.");
                    Console.WriteLine("1 - Работа с корзинами");
                    Console.WriteLine("2 - Вывод корзин");
                    Console.WriteLine("3 - Выход.");

                    chouse = IntParser(1, 3);

                    if (chouse == 1)
                    {
                        #region Work with baskets
                        while (true)
                        {
                            Console.WriteLine("\n1 - Добавить корзину");
                            Console.WriteLine("2 - Оплатить корзину");
                            Console.WriteLine("3 - Выход");

                            chouse = IntParser(1, 3);

                            if (chouse == 1)
                            {
                                #region Adding Basket

                                Basket basket = new Basket
                                {
                                    IsPaid = false,
                                    User = user
                                };

                                while (true)
                                {
                                    int productId = 1;
                                    foreach (var product in context.Products)
                                    {
                                        if (product.Amount >= 1)
                                        {
                                            Console.WriteLine("ИД продукта - " + productId);
                                            Console.WriteLine("Название - " + product.Name);
                                            Console.WriteLine("Цена продукта - " + product.Cost);
                                            Console.WriteLine();
                                        }
                                        productId++;
                                    }

                                    Console.WriteLine("Для добавления в корзину продукта введите его ИД");
                                    Console.WriteLine("Для завершения введите 0");
                                    productId = IntParser(0, context.Products.Count());

                                    if (productId == 0)
                                    {
                                        if (basket.Products.Count > 0)
                                        {
                                            context.Baskets.Add(basket);
                                            context.SaveChanges();
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        int currentProductId = 1;
                                        foreach (var product in context.Products)
                                        {
                                            if (productId == currentProductId)
                                            {
                                                product.Amount--;
                                                basket.Products.Add(product);
                                                break;
                                            }
                                            currentProductId++;
                                        }
                                    }
                                }

                                #endregion
                            }
                            else if (chouse == 2)
                            {
                                #region Payment Proccess

                                // Выбор корзины из существующих у пользователя
                                int basketId = 1;
                                Console.WriteLine();
                                foreach (var basket in context.Baskets)
                                {
                                    if (!basket.IsPaid)
                                    {
                                        Console.WriteLine("ИД корзины - " + basketId);
                                        Console.WriteLine("\tПродукты корзины");
                                        foreach (var product in basket.Products)
                                        {
                                            Console.WriteLine(product.Name);
                                        }
                                        Console.WriteLine();
                                    }
                                    basketId++;
                                }
                                Console.WriteLine("Введите ИД корзины для оплаты");
                                Console.WriteLine("Для выхода введите 0");

                                chouse = IntParser(0, context.Baskets.Count());

                                if (chouse == 0) break;

                                // Вывод общей суммы
                                int cost = 0;
                                basketId = 1;
                                foreach (var basket in context.Baskets)
                                {
                                    if (basketId == chouse)
                                    {
                                        foreach (var product in basket.Products)
                                        {
                                            cost += product.Cost;
                                        }
                                        break;
                                    }
                                    basketId++;
                                }
                                Console.WriteLine("Общая стоимость - " + cost);
                                Console.WriteLine("Хотите ли вы продолжить оплату? (1 - да, 2 - нет)");

                                int curentBasketId = chouse;

                                chouse = IntParser(1, 2);

                                if (chouse == 1)
                                {
                                    basketId = 1;
                                    foreach (var basket in context.Baskets)
                                    {
                                        if (basketId == curentBasketId)
                                        {
                                            foreach (var oldBasket in context.Baskets)
                                            {
                                                if (oldBasket.Id == basket.Id)
                                                {
                                                    oldBasket.PaymentDate = DateTime.Now;
                                                    oldBasket.IsPaid = true;
                                                }
                                            }

                                            context.SaveChanges();
                                            Console.WriteLine("Оплата успешно проведена!");
                                            break;
                                        }
                                        basketId++;
                                    }
                                }
                                else break;
                                #endregion
                            }
                            else
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                    else if (chouse == 2)
                    {
                        foreach (var basket in context.Baskets)
                        {
                            if (basket.User.Id == user.Id)
                            {
                                if (basket.IsPaid)
                                {
                                    Console.WriteLine("Оплачена " + basket.PaymentDate.ToString());
                                }
                                else
                                {
                                    Console.WriteLine("Не оплачена");
                                }
                                Console.WriteLine("-Продукты");
                                foreach (var product in basket.Products)
                                {
                                    Console.WriteLine(product.Name + " " + product.Cost);
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    else if (chouse == 3)
                        break;

                }
            }
        }
    }
}
