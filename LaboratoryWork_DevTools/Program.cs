using System;

namespace LaboratoryWork_DevTools
{
    internal class Program
    {
        static Driver[] drivers;
        static Car[] cars;
        static Cargo[] cargo;
        static Random rand = new Random();
        static RouteSheet routeSheet = new RouteSheet();
        static int selectedItem;

        enum Worker
        {
            Logist,
            Driver
        }

        static int ReadNum()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        static void Main()
        {
            drivers = new Driver[] { new Driver(rand), new Driver(rand), new Driver(rand) };
            cars = new Car[] { new Car(rand), new Car(rand), new Car(rand) };
            cargo = new Cargo[] { new Cargo(), new Cargo(), new Cargo() };

            Console.WriteLine("Кем вы работаете? (1 - Логист, 2 - Водитель)");
            selectedItem = ReadNum();
            Console.Clear();

            switch (selectedItem)
            {
                case 1:
                    ContinueAsLogist();
                    break;
                case 2:
                    ContinueAsDriver();
                    break;
            }
        }

        static void ContinueAsLogist()
        {
            Driver currentDriver;
            Car currentCar;
            Cargo currentCargo;
            bool finishedManage = false;

            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 12);
                foreach (Driver driver in drivers)
                {
                    driver.ShowInfo();
                }
                Console.SetCursorPosition(0, 0);

                Console.WriteLine($"Когого водителя хотите назначить? (1 - {drivers[0].Name} {drivers[0].Age}, " +
                            $"2 - {drivers[1].Name} {drivers[1].Age}, 3 - {drivers[2].Name} {drivers[2].Age})");
                selectedItem = ReadNum() - 1;
                currentDriver = drivers[selectedItem];
                currentCar = cars[selectedItem];
                currentCargo = cargo[selectedItem];
                ShowTable(Worker.Logist);
                Console.WriteLine("Вы выбрали " + currentDriver.Name + " " + currentDriver.Age + "\n");

                while (!finishedManage)
                {
                    Console.WriteLine("Выберите пункт из списка возможностей:\n" +
                    "1. Узнать местонахождение водителя\n" +
                    "2. Отправить водителя\n" +
                    "3. Загрузить водителя в данном месте\n" +
                    "4. Разгрузить водителя в данном месте\n" +
                    "5. Выбрать другого водителя\n");
                    selectedItem = ReadNum();
                    ShowTable(Worker.Logist);

                    switch (selectedItem)
                    {
                        case 1:
                            Console.WriteLine("Местонахождение водителя: " + currentDriver.Location + "\n");
                            break;
                        case 2:
                            SentDriver(currentDriver, currentCar, Worker.Logist);
                            break;
                        case 3:
                            LoadCargo(currentCar, currentCargo, Worker.Logist);
                            break;
                        case 4:
                            UnloadCargo(currentCar, currentCargo, Worker.Logist);
                            break;
                        case 5:
                            finishedManage = true;
                            break;
                    }
                }
                finishedManage = false;
            }
        }

        static void ContinueAsDriver()
        {
            Driver currentDriver = drivers[0];
            Car currentCar = cars[0];
            Cargo currentCargo = cargo[0];
            int spentFuel;

            ShowTable(Worker.Driver);
            while (true)
            {
                Console.WriteLine("Выберите пункт из списка возможностей:\n" +
                    "1. Отправиться в поездку\n" +
                    "2. Узнать количество топлива\n" +
                    "3. Заправиться\n" +
                    "4. Загрузиться\n" +
                    "5. Разгрузиться\n" +
                    "6. Узнать полную информацию о машине\n"
                    );
                selectedItem = ReadNum();
                ShowTable(Worker.Driver);
                switch (selectedItem)
                {
                    case 1:
                        if (currentCar.CurrentFuel < currentCar.FuelConsumption)
                        {
                            Console.WriteLine("Мало топлива, поездка невозможна\n");
                        }
                        else
                        {
                            spentFuel = rand.Next(5, currentCar.FuelConsumption);
                            SentDriver(currentDriver, currentCar, Worker.Driver, spentFuel);
                            Console.WriteLine("На эту поездку ушло " + spentFuel + " л\n");
                        }
                        break;
                    case 2:
                        Console.WriteLine("У вас " + currentCar.CurrentFuel + "л топлива\n");
                        break;
                    case 3:
                        currentCar.CurrentFuel = currentCar.TankVolume;
                        Console.WriteLine("Вы полностью заправились!\n");
                        break;
                    case 4:
                        LoadCargo(currentCar, currentCargo, Worker.Driver);
                        break;
                    case 5:
                        UnloadCargo(currentCar, currentCargo, Worker.Driver);
                        break;
                    case 6:
                        currentCar.ShowInfo();
                        break;
                }
            }
        }

        static void ShowTable(Worker Worker)
        {
            Console.Clear();
            switch (Worker)
            {
                case Worker.Logist:
                    Console.SetCursorPosition(0, 19);
                    Console.Write($"Имя\tГород\tГруз\tВес\n" +
                        $"{drivers[0].Name}\t{drivers[0].Location}\t{cars[0].Cargo}\t{cargo[0].Weight}\n" +
                        $"{drivers[1].Name}\t{drivers[1].Location}\t{cars[1].Cargo}\t{cargo[1].Weight}\n" +
                        $"{drivers[2].Name}\t{drivers[2].Location}\t{cars[2].Cargo}\t{cargo[2].Weight}\n");
                    break;

                case Worker.Driver:
                    Console.SetCursorPosition(0, 21);
                    Console.Write("Имя\tГород\tГруз\tКг\n" +
                        $"{drivers[0].Name}\t{drivers[0].Location}\t{cars[0].Cargo}\t{cargo[0].Weight}");
                    break;
            }
            Console.SetCursorPosition(0, 0);
        }

        static void SentDriver(Driver CurrentDriver, Car CurrentCar, Worker Worker, int SpentFuel = 0)
        {
            int SelectedItem;

            Console.WriteLine("Куда отправиться?");
            routeSheet.ShowList();
            SelectedItem = ReadNum() - 1;
            if (routeSheet.List[SelectedItem] == CurrentDriver.Location)
            {
                ShowTable(Worker);
                Console.WriteLine("Мы уже в этом городе\n");
            }
            else
            {
                CurrentDriver.Location = routeSheet.List[SelectedItem];
                CurrentCar.CurrentFuel -= SpentFuel;
                ShowTable(Worker);
                Console.WriteLine("Будем там с минуты на минуту\n");
            }
        }

        static void LoadCargo(Car CurrentCar, Cargo CurrentCargo, Worker Worker)
        {
            int SelectedItem;

            if (CurrentCar.Cargo == "Пусто")
            {
                Console.WriteLine("Список груза:");
                CurrentCargo.ShowList();
                SelectedItem = ReadNum() - 1;
                CurrentCar.Cargo = CurrentCargo.List[SelectedItem];
                Console.WriteLine("Кол-во килограмм? Максимум " + CurrentCar.LoadCapacity);
                SelectedItem = ReadNum();
                if (CurrentCar.LoadCapacity <= SelectedItem)
                {
                    CurrentCargo.Weight = CurrentCar.LoadCapacity;
                }
                else
                {
                    CurrentCargo.Weight = SelectedItem;
                }
                ShowTable(Worker);
                Console.WriteLine("Машина загружена\n");
            }
            else
            {
                Console.WriteLine("У водителя уже есть груз: " + CurrentCar.Cargo +
                    " " + CurrentCargo.Weight + " кг\n");
            }
        }

        static void UnloadCargo(Car CurrentCar, Cargo CurrentCargo, Worker Worker)
        {
            if (CurrentCar.Cargo == "Пусто")
            {
                Console.WriteLine("В машине нет груза\n");
            }
            else
            {
                CurrentCar.Cargo = "Пусто";
                CurrentCargo.Weight = 0;
                ShowTable(Worker);
                Console.WriteLine("Машина разгружена\n");
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
        public string[] Brands = { "Lada", "Renault", "Hyundai", "KIA", "Man", "Kamaz", "Volvo" };
        public string Brand;
        public string Cargo;
        public int[] LoadCapacities = {100, 350, 1000};
        public int LoadCapacity;
        public int ProductionYear;
        public int TankVolume;
        public int FuelConsumption;
        public int CurrentFuel;

        public Car(Random rand)
        {
            Brand = Brands[rand.Next(0, Brands.Length)];
            LoadCapacity = LoadCapacities[rand.Next(0, LoadCapacities.Length)];
            ProductionYear = rand.Next(2000, 2016);
            TankVolume = rand.Next(1, 6) * 100;
            FuelConsumption = rand.Next(20, 51);
            CurrentFuel = TankVolume;
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