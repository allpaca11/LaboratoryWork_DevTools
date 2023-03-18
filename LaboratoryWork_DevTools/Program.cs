using System;

namespace LaboratoryWork_DevTools
{
    internal class Program
    {
        static void Main()
        {
            Random rand = new Random();
            Driver[] drivers = { new Driver(rand), new Driver(rand), new Driver(rand) };
            Car[] cars = { new Car(rand), new Car(rand), new Car(rand) };
            Cargo[] cargo = { new Cargo(), new Cargo(), new Cargo() };
            RouteSheet routeSheet = new RouteSheet();
            Console.WriteLine("Кем вы работаете? (1 - Логист, 2 - Водитель)");
            int SelectedItem = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            if (SelectedItem == 1)
            {
                ContinueAsLogist();
            }
            else
            {
                ContinueAsDriver();
            }
            void ContinueAsLogist()
            {
                ShowDriverInfo("Logist");
                while (true)
                {
                    Console.WriteLine("Выберите пункт из списка возможностей:\n" +
                        "1. Получить общую информацию о водителях\n" +
                        "2. Назначить водителя для маршрута\n");
                    SelectedItem = Convert.ToInt32(Console.ReadLine());
                    ShowDriverInfo("Logist");
                    if (SelectedItem == 1)
                    {
                        foreach (Driver driver in drivers)
                        {
                            driver.ShowInfo();
                        }
                    }
                    else
                    {
                        int SelectedDriver;
                        Driver CurrentDriver;
                        Car CurrentCar;
                        Cargo CurrentCargo;
                        ChooseDriver(out SelectedDriver, out CurrentDriver, out CurrentCar, out CurrentCargo);
                        while (true)
                        {
                            
                            Console.WriteLine("Выберите пункт из списка возможностей:\n" +
                                "1. Узнать местонахождение водителя\n" +
                                "2. Отправить водителя\n" +
                                "3. Загрузить водителя в данном месте\n" +
                                "4. Разгрузить водителя в данном месте\n" +
                                "5. Выбрать другого водителя\n");
                            SelectedItem = Convert.ToInt32(Console.ReadLine());
                            ShowDriverInfo("Logist");
                            switch (SelectedItem)
                            {
                                case 1:
                                    Console.WriteLine("Местонахождение водителя: " + CurrentDriver.Location + "\n");
                                    break;
                                case 2:
                                    Console.WriteLine("Куда отправить водителя?");
                                    routeSheet.ShowList();
                                    SelectedItem = Convert.ToInt32(Console.ReadLine()) - 1;
                                    CurrentDriver.Location = routeSheet.List[SelectedItem];
                                    ShowDriverInfo("Logist");
                                    Console.WriteLine("Водитель будет там с минуты на минуту\n");
                                    break;
                                case 3:
                                    if (CurrentCar.Cargo == "Пусто")
                                    {
                                        Console.WriteLine("Список груза:");
                                        CurrentCargo.ShowList();
                                        SelectedItem = Convert.ToInt32(Console.ReadLine()) - 1;
                                        CurrentCar.Cargo = CurrentCargo.List[SelectedItem];
                                        Console.WriteLine("Кол-во килограмм? Максимум " + CurrentCar.LoadCapacity);
                                        SelectedItem = Convert.ToInt32(Console.ReadLine());
                                        if (CurrentCar.LoadCapacity <= SelectedItem)
                                        {
                                            CurrentCargo.Weight = CurrentCar.LoadCapacity;
                                        }
                                        else
                                        {
                                            CurrentCargo.Weight = SelectedItem;
                                        }
                                        ShowDriverInfo("Logist");
                                        Console.WriteLine("Машина загружена\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("У водителя уже есть груз: " + CurrentCar.Cargo + 
                                            " " + CurrentCargo.Weight + " кг\n");
                                    }
                                    break;
                                case 4:
                                    if (CurrentCar.Cargo == "Пусто")
                                    {
                                        ShowDriverInfo("Logist");
                                        Console.WriteLine("В машине нет груза\n");
                                    }
                                    else
                                    {
                                        CurrentCar.Cargo = "Пусто";
                                        CurrentCargo.Weight = 0;
                                        ShowDriverInfo("Logist");
                                        Console.WriteLine("Машина разгружена\n");
                                    }
                                    break;
                                case 5:
                                    ChooseDriver(out SelectedDriver, out CurrentDriver, out CurrentCar, out CurrentCargo);
                                    break;
                            }
                        }
                
                    }
                }
            }
            void ContinueAsDriver()
            {
                ShowDriverInfo("Driver");
                while (true)
                {
                    Console.WriteLine("Выберите пункт из списка возможностей:\n" +
                        "1. Отправиться в поездку\n" +
                        "2. Узнать количество топлива\n" +
                        "3. Заправиться\n" +
                        "4. Загрузиться\n" +
                        "5. Разгрузиться\n" +
                        "6. Унать полную информацию о машине\n"
                        );
                    SelectedItem = Convert.ToInt32(Console.ReadLine());
                    ShowDriverInfo("Driver");
                    switch (SelectedItem)
                    {
                        case 1:
                            if (cars[0].CurretFuel < cars[0].FuelConsumption)
                            {
                                Console.WriteLine("Мало топлива, поездка невозможна\n");
                            }
                            else
                            {
                                Console.WriteLine("Куда отправиться?\n");
                                routeSheet.ShowList();
                                SelectedItem = Convert.ToInt32(Console.ReadLine()) - 1;
                                if (drivers[0].Location == routeSheet.List[SelectedItem])
                                {
                                    ShowDriverInfo("Driver");
                                    Console.WriteLine("Вы уже в этом городе\n");
                                }
                                else
                                {
                                    drivers[0].Location = routeSheet.List[SelectedItem];
                                    int SpentFuel = rand.Next(5, cars[0].FuelConsumption);
                                    cars[0].CurretFuel -= SpentFuel;
                                    ShowDriverInfo("Driver");
                                    Console.WriteLine("На эту поездку ушло " + SpentFuel + " л\n");
                                }
                                
                            }
                            break;
                        case 2:
                            Console.WriteLine("У вас " + cars[0].CurretFuel + "л топлива\n");
                            break;
                        case 3:
                            cars[0].CurretFuel = cars[0].TankVolume;
                            Console.WriteLine("Вы полностью заправились!\n");
                            break;
                        case 4:
                            if (cars[0].Cargo == "Пусто")
                            {
                                Console.WriteLine("Список груза:\n");
                                cargo[0].ShowList();
                                SelectedItem = Convert.ToInt32(Console.ReadLine()) - 1;
                                cars[0].Cargo = cargo[0].List[SelectedItem];
                                Console.WriteLine("Кол-во килограмм? Максимум " + cars[0].LoadCapacity);
                                SelectedItem = Convert.ToInt32(Console.ReadLine());
                                if (cars[0].LoadCapacity < SelectedItem)
                                {
                                    cargo[0].Weight = cars[0].LoadCapacity;
                                }
                                else
                                {
                                    cargo[0].Weight = SelectedItem;
                                }
                                ShowDriverInfo("Driver");
                                Console.WriteLine("Машина загружена\n");
                            }
                            else
                            {
                                Console.WriteLine("У водителя уже есть груз: " + cars[0].Cargo + " " + cargo[0].Weight + " кг\n");
                            }
                            break;
                        case 5:
                            if (cars[0].Cargo == "Пусто")
                            {
                                Console.WriteLine("В машине нет груза\n");
                            }
                            else
                            {
                                cars[0].Cargo = "Пусто";
                                cargo[0].Weight = 0;
                                ShowDriverInfo("Driver");
                                Console.WriteLine("Машина разгружена\n");
                            }
                            break;
                        case 6:
                            cars[0].ShowInfo();
                            break;
                    }
                }
                
            }

            
            void ChooseDriver(out int SelectedDriver, out Driver CurrentDriver, out Car CurrentCar, out Cargo CurrentCargo)
            {
                Console.WriteLine($"Когого водителя хотите назначить? (1 - {drivers[0].Name} {drivers[0].Age}, " +
                                $"2 - {drivers[1].Name} {drivers[1].Age}, 3 - {drivers[2].Name} {drivers[2].Age})");
                SelectedDriver = Convert.ToInt32(Console.ReadLine()) - 1;
                ShowDriverInfo("Driver");
                CurrentDriver = drivers[SelectedDriver];
                CurrentCar = cars[SelectedDriver];
                CurrentCargo = cargo[SelectedDriver];
                Console.WriteLine("Вы выбрали " + CurrentDriver.Name + " " + CurrentDriver.Age + "\n");
            }
            void ShowDriverInfo(string Worker)
            {
                Console.Clear();
                if (Worker == "Logist")
                {
                    Console.SetCursorPosition(0, 19);
                    Console.Write($"Имя\tГород\tГруз\tВес\n" +
                        $"{drivers[0].Name}\t{drivers[0].Location}\t{cars[0].Cargo}\t{cargo[0].Weight}\n" +
                        $"{drivers[1].Name}\t{drivers[1].Location}\t{cars[1].Cargo}\t{cargo[1].Weight}\n" +
                        $"{drivers[2].Name}\t{drivers[2].Location}\t{cars[2].Cargo}\t{cargo[2].Weight}\n");
                }
                else
                {
                Console.SetCursorPosition(0, 21);
                Console.Write("Имя\tГород\tГруз\tКг\n" +
                    $"{drivers[0].Name}\t{drivers[0].Location}\t{cars[0].Cargo}\t{cargo[0].Weight}");
                }
                Console.SetCursorPosition(0, 0);
            }
        }
    }
    internal class Driver
    {
        string[] Names = new string[] {"Сергей", "Михаил", "Иван", "Антон", "Роман", "Алексей", "Кирилл", "Максим", 
            "Арсений", "Арсен", "Павел", "Никита", "Захар", "Эльмир", "Устин"};
        RouteSheet routeSheet = new RouteSheet();
        public string Location;
        public string Name;
        public int Age;
        public int Experience;
        public void ShowInfo()
        {
            Console.WriteLine($"Имя: {Name}\nВозраст: {Age}\nОпыт работы: {Experience}\n");
        }
        public Driver(Random rand)
        {
            Name = Names[rand.Next(0, Names.Length)];
            Age = rand.Next(20, 50);
            Experience = rand.Next(2, 12);
            Location = routeSheet.List[rand.Next(0, routeSheet.List.Length)];
        }
    }
    internal class Car
    {
        Random rand = new Random();
        public string[] Brands = { "Lada", "Renault", "Hyundai", "KIA", "Man", "Kamaz", "Volvo" };
        public string Brand;
        public string Cargo;
        public int[] LoadCapacities = {100, 350, 1000};
        public int LoadCapacity;
        public int ProductionYear;
        public int TankVolume;
        public int FuelConsumption;
        public int CurretFuel;
        public Car(Random rand)
        {
            Brand = Brands[rand.Next(0, Brands.Length)];
            LoadCapacity = LoadCapacities[rand.Next(0, LoadCapacities.Length)];
            ProductionYear = rand.Next(2000, 2016);
            TankVolume = rand.Next(1, 6) * 100;
            FuelConsumption = rand.Next(20, 51);
            CurretFuel = TankVolume;
            Cargo = "Пусто";
        }
        public void ShowInfo()
        {
            Console.WriteLine($"Бренд: {Brand}\nГрузоподъёмность: {LoadCapacity}\nГод производства: {ProductionYear}\n" +
                $"Объём топливного бака: {TankVolume}\nРасход топлива: {FuelConsumption} л на 100 км\n");
        }
    }
    internal class Cargo
    {
        public string[] List = { "Сталь", "Брёвна", "Уголь", "Горох", "Соя", "Лён", "Окна", "Грунт", "Песок", "Кирпичи" };
        public int Weight;
        public void ShowList()
        {
            for (int i = 0; i < List.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + List[i]);
            }
        }
    }
    internal class RouteSheet
    {
        public string[] List = { "Омск", "Томск", "Чита", "Брянск", "Москва", "Казань", "Самара", "Пермь" };

        public void ShowList()
        {
            for (int i = 0; i < List.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + List[i]);
            }
        }
    }
}