﻿using MySql.Data.MySqlClient;
bool canLogin = false;
int id=-1;
string cs = @"server=localhost;userid=pk111;password=123456;database=pk111";
MySqlConnection con = new MySqlConnection(cs);
con.Open();
Console.WriteLine("Введите логин(ник)");
string findNik = Console.ReadLine();
Console.WriteLine("Введите пароль");
string findPassword = Console.ReadLine();
string sql = "SELECT `password`, `id` FROM `users` WHERE `nikname` = @nik";
MySqlCommand cmd = new MySqlCommand(sql, con);
cmd.Parameters.AddWithValue("@nik", findNik);
cmd.Prepare();
MySqlDataReader rdr = cmd.ExecuteReader();
if (rdr.HasRows)
{
    Console.WriteLine("Такой пользователь есть");
    rdr.Read();
    string dbPassword = rdr.GetString(0);
    if (dbPassword == findPassword)
    {
        Console.WriteLine("Пароль совпадает");
        //тут у пользователя всё хорошо
        canLogin = true;
    }
    else
    {
        Console.WriteLine("Пароль НЕ совпадает");
    }
}
else
{
    Console.WriteLine("Такого пользователя неть :-|");
}
rdr.Close();
con.Close();
if (canLogin)
{
    string onlineSQL = "UPDATE `users` SET `online`=true WHERE `nikname`=@onlineNik";
    MySqlCommand onlineCmd = new MySqlCommand(onlineSQL, con);
    onlineCmd.Parameters.AddWithValue("onlineNik", findNik);
    con.Open();
    onlineCmd.Prepare();
    onlineCmd.ExecuteNonQuery();
    con.Close();

    Console.WriteLine("Напишите сообщение");
    string message = Console.ReadLine();
    string msgSQL = "INSERT INTO `msg`(`text`, `user`) VALUES (@messageText,@userId)";
    MySqlCommand sendMessage = new MySqlCommand(msgSQL, con);
    sendMessage.Parameters.AddWithValue("messageText", message);
    sendMessage.Parameters.AddWithValue("userId",);
}



//сделать авторизацию по паре логин-пароль
/*
    пользователь пишет логин и пароль
    у БД спрашиваем есть ли такой логин
    если такой логин есть, то получаем из БД пароль
    проверяем совпадение пароля
    если пароль совпадает, то авторизуем пользователя.
*/