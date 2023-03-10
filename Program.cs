using MySql.Data.MySqlClient;
bool canLogin = false;
int id = -1;
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
    id = rdr.GetInt32(1);
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
    while (true)
    {
        Console.WriteLine("Напишите сообщение");
        string message = Console.ReadLine();
        string msgSQL = "INSERT INTO `msg`(`text`, `user`) VALUES (@messageText,@userId)";
        MySqlCommand sendMessage = new MySqlCommand(msgSQL, con);
        sendMessage.Parameters.AddWithValue("messageText", message);
        sendMessage.Parameters.AddWithValue("userId", id);
        con.Open();
        sendMessage.ExecuteNonQuery();
        con.Close();
        Console.WriteLine("------------------------------------------------");
        string allMsg = "SELECT `users`.`nikname` as \"Nik\" ,`time`,`text` FROM `msg` JOIN `users` WHERE `msg`.`user` = `users`.`id` ORDER BY `time` ASC";
        MySqlCommand getAllMsg = new MySqlCommand(allMsg, con);
        con.Open();
        MySqlDataReader allMessages = getAllMsg.ExecuteReader();
        while (allMessages.Read())
        {
            Console.WriteLine("{0} - {1}:{2} ", allMessages.GetString(1), allMessages.GetString(0), allMessages.GetString(2));
        }
        allMessages.Close();
        con.Close();
    }
}