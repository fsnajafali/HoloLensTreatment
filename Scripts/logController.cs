using UnityEngine;
using UnityEditor;
using System.IO;
using System;
public class LogController : MonoBehaviour
{
    DateTime Now;
    static string file_title;

    void Start()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        file_title = cur_time.ToString();
        //file_title = "/" + file_title;
    }


    public static void Print(int day, int hour, int minute, string text)
    {
         
        //string path = "Assets/Resources/" + file_title + ".txt";
        string path = Application.persistentDataPath + file_title + ".txt";


        //Write some text
        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine("Day,Hour,Message,Current Cash,Net Change,Delivery Expense,Employee Expense,FOH,BOH,Items Sold,Items Expired,Upcoming Deliveries");

        if (minute < 10)
        {
            writer.WriteLine(day + "," + hour + ":0" + minute + "," + text);
        }

        else
        {
            writer.WriteLine(day + "," + hour + ":" + minute + "," + text);
        }

        writer.Close();

        //Re-import the file to update the reference in the editor
        Resources.Load(path);
    }

    public static void PrintEndOfDay(float cash, string change, decimal deliveryExpense, decimal employeeExpense, int FOH, int BOH, int itemsSold, int itemsExpired, int numDeliveries)
    {

        string path = Application.persistentDataPath + file_title + ".txt";
        StreamWriter writer = new StreamWriter(path, true);
     
        writer.WriteLine(",,," + cash + "," + change + "," + deliveryExpense + "," + employeeExpense + "," + FOH + "," + BOH + "," + itemsSold + "," + itemsExpired + "," + numDeliveries);

        writer.Close();
        Resources.Load(path);
    }

    


}