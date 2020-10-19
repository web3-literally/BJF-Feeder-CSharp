#property copyright "Copyright 2015, MetaQuotes Software Corp.";
#property link "https://www.mql5.com";
#property version "1.00";
#property strict

#import "winmm.dll"

int timeBeginPeriod(int);

#import "feeder_connector.dll"

int ConnectToFeeder(string,int,string);
void DisconnectFromFeeder();
int ReceiveData(string&,double&[],double&[]);

#import

enum Feeders
{
   UK = 0, // UK
   US = 1 // US
};

extern Feeders Feeder = US;
extern string username;

string Input_String_00000038;
string Input_String_00000018;
string Input_String_00000028;
int Return_Integer_00000000;
string Input_String_00000008;
int Input_Integer_00000000;
int Global_Integer_00000000;
short Return_Short_00000000;
string Global_String_00000000;
int Global_Integer_00000001;
long Global_Long_00000002;
string Global_String_00000001;
int Global_Integer_00000002;
double Global_Double_00000002;
int Global_Integer_00000003;
double Global_Double_00000003;
bool Global_Boolean_00000004;
double Inline_Double_00000004;
long Return_Long_00000000;
double Global_Double_00000004;
string Input_String_0000007C;
int Input_Integer_00000044;

string Input_String_0000007C[18] = { "AUDUSD", "EURUSD", "EURJPY", "GBPUSD", "NZDUSD", "USDJPY", "USDCHF", "USDCAD", "XAUUSD", "GDAXI", "EURNOK", "USDNOK", "EURSEK", "USDSEK", "EURTRY", "USDTRY", "EURMXN", "USDMXN" };
Input_Integer_00000000 = 9236;
Input_String_00000008 = "185.95.16.126";
Input_String_00000018 = "label_status_info";
Input_String_00000028 = "label_status_value";
Input_String_00000038 = "feeder_label";
Input_Integer_00000044 = 18;

int init()
{
   int Local_Integer_FFFFFFFC;

   Local_Integer_FFFFFFFC = 0;
   timeBeginPeriod(1);
   ObjectCreate(0, Input_String_00000038, OBJ_LABEL, 0, 0, 0);
   ObjectSetInteger(0, Input_String_00000038, 6, 15128749);
   ObjectSetInteger(0, Input_String_00000038, 102, 5);
   ObjectSetInteger(0, Input_String_00000038, 103, 30);
   ObjectSetString(0, Input_String_00000038, 1001, "Arial");
   ObjectSetInteger(0, Input_String_00000038, 100, 14);
   ObjectSetString(0, Input_String_00000038, 999, "BJF Trading Group Feeder");
   ObjectCreate(0, Input_String_00000018, OBJ_LABEL, 0, 0, 0);
   ObjectSetInteger(0, Input_String_00000018, 6, 16119285);
   ObjectSetInteger(0, Input_String_00000018, 102, 5);
   ObjectSetInteger(0, Input_String_00000018, 103, 60);
   ObjectSetString(0, Input_String_00000018, 1001, "Arial");
   ObjectSetInteger(0, Input_String_00000018, 100, 14);
   ObjectSetString(0, Input_String_00000018, 999, "STATUS:");
   ObjectCreate(0, Input_String_00000028, OBJ_LABEL, 0, 0, 0);
   ObjectSetInteger(0, Input_String_00000028, 6, 65535);
   ObjectSetInteger(0, Input_String_00000028, 102, 90);
   ObjectSetInteger(0, Input_String_00000028, 103, 60);
   ObjectSetString(0, Input_String_00000028, 1001, "Arial");
   ObjectSetInteger(0, Input_String_00000028, 100, 14);
   ObjectSetString(0, Input_String_00000028, 999, "Connecting...");
   EventSetTimer(1);
   ChartRedraw(0);
   Local_Integer_FFFFFFFC = 0;
   return 0;
}

void OnDeinit(const int reason)
{
   ObjectSetInteger(0, Input_String_00000028, 6, 255);
   ObjectSetString(0, Input_String_00000028, 999, "Disconnecting...");
   DisconnectFromFeeder();
   ObjectDelete(Input_String_00000028);
   ObjectDelete(Input_String_00000018);
   ObjectDelete(Input_String_00000038);
}

void OnTimer()
{
   string String_String_00000000;
   string String_String_00000001;
   string String_String_00000002;
   string String_String_00000003;
   string String_String_00000004;
   string String_String_00000005;
   string Local_String_FFFFFFF0;
   string Local_String_FFFFFFE0;
   short Local_Short_FFFFFFDE;
   string Local_String_FFFFFFA8;
   double Local_Double_FFFFF968;
   double Local_Double_FFFFF2F4;
   string Local_String_FFFFF28C;
   int Local_Integer_FFFFF288;
   int Local_Integer_FFFFF284;
   int Local_Integer_FFFFF280;
   int Local_Integer_FFFFF27C;
   string Local_String_FFFFF270;
   double Local_Double_FFFFF268;
   double Local_Double_FFFFF260;

   Local_Short_FFFFFFDE = 0;
   string Local_String_FFFFFFA8[];
   double Local_Double_FFFFF968[];
   double Local_Double_FFFFF2F4[];
   string Local_String_FFFFF28C[];
   Local_Integer_FFFFF288 = 0;
   Local_Integer_FFFFF284 = 0;
   Local_Integer_FFFFF280 = 0;
   Local_Integer_FFFFF27C = 0;
   Local_Double_FFFFF268 = 0;
   Local_Double_FFFFF260 = 0;
   EventKillTimer();
   if (Feeder == 1) { // Goto_00000004
   Input_String_00000008 = "185.95.19.32";
   } // Label 00000004
   if (Feeder == 0) { // Goto_00000006
   Input_String_00000008 = "185.95.16.126";
   } // Label 00000006
   String_String_00000000 = username;
   String_String_00000001 = Input_String_00000008;
   Global_Integer_00000000 = ConnectToFeeder(String_String_00000001, Input_Integer_00000000, String_String_00000000);
   if (Global_Integer_00000000 == 0) { // Goto_00000008
   GlobalVariableSet("FEEDER_connected", 0);
   ObjectSetInteger(0, Input_String_00000028, 6, 255);
   ObjectSetString(0, Input_String_00000028, 999, "Disconnected");
   ChartRedraw(0);
   ExpertRemove();
   } // Label 00000008
   Local_String_FFFFFFF0 = "symbol dummy string";
   Local_String_FFFFFFE0 = ";";
   Local_Short_FFFFFFDE = 0;
   Local_Short_FFFFFFDE = StringGetCharacter(Local_String_FFFFFFE0, 0);
   ArrayFree(Local_Double_FFFFF968);
   ArrayResize(Local_Integer_FFFFF968, 200);
   ArrayFree(Local_Double_FFFFF2F4);
   ArrayResize(Local_Integer_FFFFF2F4, 200);
   ObjectSetInteger(0, Input_String_00000028, 6, 32768);
   ObjectSetString(0, Input_String_00000028, 999, "Connected");
   GlobalVariableSet("FEEDER_connected", 1);
   Print("Conected to feeder");
   ChartRedraw(0);
   if (_StopFlag == 0) { // Goto_0000000F
   do { // Label 0000000D
   //232 || 00000056 | 00000001 | 00000000 | 00000000 | 00000108 | 00000000 | fffffff0 | ffffffff | 00000000 | 00000000 | 00000000 | 00000000 | 00000000 | 00000000 | 00000000 | 00000000
   Global_Integer_00000000 = ReceiveData(Local_String_FFFFFFF0, Local_Address_FFFFF968.m_FFFFFFE8, Local_Address_FFFFF2F4.m_FFFFFFE8);
   Local_Integer_FFFFF288 = Global_Integer_00000000;
   Local_Integer_FFFFF284 = StringSplit(Local_String_FFFFFFF0, Local_Short_FFFFFFDE, Local_String_FFFFF28C) - 1;
   if (Global_Integer_00000000 >= 0) Goto 00000013;
   GlobalVariableSet("FEEDER_connected", 0);
   ObjectSetInteger(0, Input_String_00000028, 6, 255);
   ObjectSetString(0, Input_String_00000028, 999, "Disconnected");
   ChartRedraw(0);
   Print("Disconnected from feeder...");
   Local_Integer_FFFFF280 = 1;
   if (_StopFlag != 0) Goto 00000020;
   do { // Label 00000016
   ObjectSetString(0, Input_String_00000028, 999, "Connecting...");
   ChartRedraw(0);
   Print("Reconnecting to feeder...Attempt number ", Local_Integer_FFFFF280);
   Sleep(10000);
   String_String_00000001 = username;
   String_String_00000002 = Input_String_00000008;
   Global_Integer_00000000 = ConnectToFeeder(String_String_00000002, Input_Integer_00000000, String_String_00000001);
   if (Global_Integer_00000000 == 0) Goto 00000019;
   GlobalVariableSet("FEEDER_connected", 1);
   ObjectSetInteger(0, Input_String_00000028, 6, 32768);
   ObjectSetString(0, Input_String_00000028, 999, "Connected");
   ChartRedraw(0);
   Print("Conected to feeder");
   Goto 00000020;
   // Label 00000019
   Local_Integer_FFFFF280 = Local_Integer_FFFFF280 + 1;
   } while (_StopFlag == 0); // Goto_00000016
   Goto 00000020;
   // Label 00000013
   if (Local_Integer_FFFFF288 != 0 && Local_Integer_FFFFF288 == Local_Integer_FFFFF284) { // Goto_00000020
   Local_Integer_FFFFF27C = 0;
   if (Local_Integer_FFFFF288 > 0) { // Goto_00000020
   do { // Label 00000022
   Local_String_FFFFF270 = Local_String_FFFFF28C[Local_Integer_FFFFF27C];
   StringReplace(Local_String_FFFFF270, "/", "");
   if (Local_String_FFFFF270 == "") { // Goto_00000027
   String_String_00000002 = IntegerToString(Local_String_FFFFF28C[Local_Integer_FFFFF27C], 0, 32);
   Print("Client: Error in symbol name, wrong code ", String_String_00000002);
   ArrayFree(Local_String_FFFFF28C);
   ArrayFree(Local_Double_FFFFF2F4);
   ArrayFree(Local_Double_FFFFF968);
   ArrayFree(Local_String_FFFFFFA8);
   return UnknownScope_Void_00000000;
   } // Label 00000027
   Local_Double_FFFFF268 = Local_Double_FFFFF968[Local_Integer_FFFFF27C];
   Local_Double_FFFFF260 = Local_Double_FFFFF2F4[Local_Integer_FFFFF27C];
   if ((Local_Double_FFFFF268 == 0) || (Local_Double_FFFFF260 == 0)) { // Goto_0000002F
   // Label 00000030
   Print("WARNING:Zero quotes (", Local_Double_FFFFF268, "/", Local_Double_FFFFF260, ")were received for ", Local_String_FFFFF270);
   } // Label 0000002F
   String_String_00000003 = "FEEDER_" + Local_String_FFFFF270;
   String_String_00000003 = String_String_00000003 + "_BID";
   GlobalVariableSet(String_String_00000003, Local_Double_FFFFF268);
   String_String_00000004 = "FEEDER_" + Local_String_FFFFF270;
   String_String_00000004 = String_String_00000004 + "_ASK";
   GlobalVariableSet(String_String_00000004, Local_Double_FFFFF260);
   String_String_00000005 = "FEEDER_" + Local_String_FFFFF270;
   String_String_00000005 = String_String_00000005 + "_TICKTIME";
   GlobalVariableSet(String_String_00000005, TimeLocal());
   Local_Integer_FFFFF27C = Local_Integer_FFFFF27C + 1;
   } while (Local_Integer_FFFFF27C < Local_Integer_FFFFF288); // Goto_00000022
   }} // Label 00000020
   } while (_StopFlag == 0); // Goto_0000000D
   } // Label 0000000F
   ArrayFree(Local_String_FFFFF28C);
   ArrayFree(Local_Double_FFFFF2F4);
   ArrayFree(Local_Double_FFFFF968);
   ArrayFree(Local_String_FFFFFFA8);
   // Label 00000003
}


