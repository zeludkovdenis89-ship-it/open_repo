using System;
using System.Collections.Generic;
using Npgsql;
using my_first_project.Models;
using System.Data;

namespace my_first_project
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
            "Host=localhost;Port=5432;Database=New_project;Username=postgres;Password=Den.0805";

            string usersSelect = "SELECT Id, Name, First_name, Last_name, Age, Email FROM Users";
            string tasksSelect = "SELECT Id, Title, Description FROM Tasks";

            DataTable usersTable = new DataTable();
            DataTable tasksTable = new DataTable();

            using (var Collection = new NpgsqlConnection(connectionString))
            {
                //Работа с таблицей Users
                using (var adapter = new NpgsqlDataAdapter(usersSelect, Collection))
                {
                    using (var commandBuilder = new NpgsqlCommandBuilder(adapter))
                    {
                        // Загрузка данных
                        adapter.Fill(usersTable);
                        Console.WriteLine("Данные успешно загружены в память");

                        // Редактируем выгруженные из БД таблицы
                        DataRow newRow = usersTable.NewRow();
                        newRow["Name"] = "Димитрий";
                        newRow["First_name"] = "А";
                        newRow["Last_name"] = "Мхецадзе";
                        newRow["Age"] = 45;
                        newRow["Email"] = $"Dimitryi_{Guid.NewGuid():N}@gmail.com";
                        usersTable.Rows.Add(newRow);

                        DataRow newRow2 = usersTable.NewRow();
                        newRow2["Name"] = "Алексей";
                        newRow2["First_name"] = "Ю";
                        newRow2["Last_name"] = "Диндонкан";
                        newRow2["Age"] = 30;
                        newRow2["Email"] = $"Alexey_{Guid.NewGuid():N}@gmail.com";
                        usersTable.Rows.Add(newRow2);

                        DataRow newRow3 = usersTable.NewRow();
                        newRow3["Name"] = "Мария";
                        newRow3["First_name"] = "Р";
                        newRow3["Last_name"] = "Грендищ";
                        newRow3["Age"] = 28;
                        newRow3["Email"] = $"Maria_{Guid.NewGuid():N}@gmail.com";
                        usersTable.Rows.Add(newRow3);

                        DataRow newRow4 = usersTable.NewRow();
                        newRow4["Name"] = "Сергей";
                        newRow4["First_name"] = "лЛ";
                        newRow4["Last_name"] = "Кирикович";
                        newRow4["Age"] = 35;
                        newRow4["Email"] = $"Sergey_{Guid.NewGuid():N}@gmail.com";
                        usersTable.Rows.Add(newRow4);

                        DataRow newRow5 = usersTable.NewRow();
                        newRow5["Name"] = "Ольга";
                        newRow5["First_name"] = "Н";
                        newRow5["Last_name"] = "Бузова";
                        newRow5["Age"] = 27;
                        newRow5["Email"] = $"Olga_{Guid.NewGuid():N}@gmail.com";
                        usersTable.Rows.Add(newRow5);

                        // Обновление существующих записей
                        if (usersTable.Rows.Count > 1)
                        {
                            usersTable.Rows[0]["Age"] = (int)usersTable.Rows[0]["Age"] + 1;
                        }
                        //Удаление записи
                        // usersTable.Rows[1].Delete();

                        // Пакетная синхронизация
                        adapter.Update(usersTable);
                        Console.WriteLine("Синхронизация завершенна. Все данные успешно отправлены на сервер");

                        Console.WriteLine("Текущий список пользователей");
                        PrintUsers(usersTable);
                    }
                }

                //Работа с таблицей Tasks
                using (var adapter = new NpgsqlDataAdapter(tasksSelect, Collection))
                {
                    using (var commandBuilder = new NpgsqlCommandBuilder(adapter))
                    {
                        // Загрузка данных
                        adapter.Fill(tasksTable);

                      
                        DataRow newTask1 = tasksTable.NewRow();
                        newTask1["Title"] = "Задача 1";
                        newTask1["Description"] = "Описание";
                        tasksTable.Rows.Add(newTask1);

                      
                        DataRow newTask2 = tasksTable.NewRow();
                        newTask2["Title"] = "Задача 2";
                        newTask2["Description"] = "Описание";
                        tasksTable.Rows.Add(newTask2);

                        
                        DataRow newTask3 = tasksTable.NewRow();
                        newTask3["Title"] = "Задача 3";
                        newTask3["Description"] = "Описание ";
                        tasksTable.Rows.Add(newTask3);

                        // Пакетная синхронизация
                        adapter.Update(tasksTable);
                        Console.WriteLine("Синхронизация завершенна");

                        Console.WriteLine("Текущий список задач");
                        PrintTasks(tasksTable);

                        //Удаление 2 случайных задач по Id
                        Random rnd = new Random();
                        int deleted = 0;
                        while (deleted < 2 && tasksTable.Rows.Count > 0)
                        {
                            int index = rnd.Next(tasksTable.Rows.Count);
                            DataRow row = tasksTable.Rows[index];
                            if (row.RowState == DataRowState.Deleted) continue;

                            int id = Convert.ToInt32(row["Id"]);
                            DataRow[] found = tasksTable.Select($"Id = {id}");
                            if (found.Length > 0)
                            {
                                found[0].Delete();
                                deleted++;
                            }
                        }

                        // Пакетная синхронизация
                        adapter.Update(tasksTable);
                        Console.WriteLine("Синхронизация завершенна");

                        Console.WriteLine("Текущий список задач");
                        PrintTasks(tasksTable);
                    }
                }

                using (var adapter = new NpgsqlDataAdapter(usersSelect, Collection))
                {
                    using (var commandBuilder = new NpgsqlCommandBuilder(adapter))
                    {
                        usersTable.Clear();
                        adapter.Fill(usersTable);

                        Random rnd = new Random();
                        if (usersTable.Rows.Count > 0)
                        {
                            int index = rnd.Next(usersTable.Rows.Count);
                            int id = Convert.ToInt32(usersTable.Rows[index]["Id"]);
                            DataRow[] found = usersTable.Select($"Id = {id}");
                            if (found.Length > 0)
                            {
                                found[0].Delete();
                            }
                        }

                        // Пакетная синхронизация
                        adapter.Update(usersTable);
                        Console.WriteLine("Синхронизация завершенна");
                        Console.WriteLine("Текущий список пользователей");
                        PrintUsers(usersTable);
                    }
                }
            }
        }

        static void PrintUsers(DataTable usersTable)
        {
            foreach (DataRow row in usersTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    var user = new User
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        First_name = row["First_name"].ToString(),
                        Last_name = row["Last_name"].ToString(),
                        Age = Convert.ToInt32(row["Age"]),
                        Email = row["Email"].ToString()
                    };
                    Console.WriteLine($"[{user.Id} name: {user.Name} First_name: {user.First_name} Last_name: {user.Last_name} Age: {user.Age} Email: {user.Email}]");
                }
            }
        }

        static void PrintTasks(DataTable tasksTable)
        {
            foreach (DataRow row in tasksTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    var task = new my_first_project.Models.Task
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        title = row["Title"].ToString(),
                        description = row["Description"].ToString()
                    };
                    Console.WriteLine($"[{task.Id} title: {task.title} description: {task.description}]");
                }
            }
        }
    }
}