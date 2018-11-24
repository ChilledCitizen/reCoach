
using System;
using System.Windows.Forms;



namespace Junction18
{
    class Program
    {

        //The Main() calls a function from SQL_Query_Handler with the parameter of playerGUID given in the frontend.
        //SQL_Query_Handler delivers the data to Data_Optimizer, which calls JSON_Handler to make a JSON from it
        //OR SQL_Query_Handler calls JSON_Handler and makes a json / an object from queried JSON, and delivers it to Data_Optimizer
        //Data_Optimizer creates another json with JSON_Handler, which is sent to Analyzer which analyzes the data and makes suggestions for improvements


        static void Main(string[] args)
        {
            SQL_Query_Handler.MK_Query(args[0]);
        }
    }
}
