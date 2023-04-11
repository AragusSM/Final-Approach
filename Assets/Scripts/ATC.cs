using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.IO;


 // PlaneData Class
public class PlaneData
{
    public string callSign;
    public string iata;
    public string icao;
    public string arriveDepart;
    public string adIATA;
    public string planeType;
    public string priority;
    public float fuel;
    public float timeTillOnTime;
    public string planeAsset;
    public int maxPassengers;
    public char planeSize;
    public float minFuel;
    public float timeToTerminal;
    public float timeToRunway;
    public float timeToLand;
    public float timeToAir;
    public string ident;
}

// class that handles the atc game logic
public class ATC : MonoBehaviour
{
    public Terminal terminal;   // terminal object where all departing planes originate from
    public Sky sky; // sky object where all arriving planes originate from
    Airplane selectedAirplane;  // the airplane that the user has currently selected
    public int selectedButton = -1; // the button that the user has currently selected, -1 if none
    public bool isDepartureButton = true; //notifies the aesthetic renderer if the button selected was a runwaya or departure button
    GameObject panel; //display panel object
    GameObject[] panelElements; //display panel elements
   
    public List<PlaneData> allPlaneData; // list of planeObject data pulled from csv

    public HashSet<int> chosenPlanes; // indices of the chosen planes (shared across sky and terminal)
    public TextAsset textAssetData;
    public AudioSource planeSound2;

    // called when a plane is selected from the departures UI
    void Start() {
        GameObject g = FindInActiveObjectByName("FlightDisplay");
        GameObject ui = g.transform.GetChild(0).gameObject;
        GameObject selection = ui.transform.GetChild(0).gameObject;
        panel = selection.transform.GetChild(0).gameObject;
        GameObject flightText = panel.transform.Find("FlightNumberText").gameObject;
        GameObject fuelText = panel.transform.Find("FuelText").gameObject;
        GameObject statusText = panel.transform.Find("StatusText").gameObject;
        GameObject waitText = panel.transform.Find("WaitText").gameObject;
        GameObject description = panel.transform.Find("Description").gameObject;
        panelElements = new GameObject[5];
        panelElements[0] = flightText;
        panelElements[1] = fuelText;
        panelElements[2] = statusText;
        panelElements[3] = waitText;
        panelElements[4] = description;
        allPlaneData = new List<PlaneData>();
        chosenPlanes = new HashSet<int>();
        // chnage this path based on where you put the csv file
        readPlaneData("/Users/neal/final-approach-vr/Assets/VRPlaneInfo.csv");


    }

    // read plane data from csv file
    public void readPlaneData(string fileName) {
        
        // OLD IMPLEMENTATION:
        // string csvString = "Air Force ONE,N/A,N/A,Andrews Air Force Base,AFB,Boeing 747,Priority,28,230,passagiermaschine_747_static,416,A,3,20,20,15,15\nKorean Air 539,KAL539,KE539,Seoul,ICN,Boeing 747,N/A,36,-325,passagiermaschine_747_static,416,A,3,20,20,15,15\nLufthansa 419,DLH419,LH419,Frankfurt,FRA,Boeing 747,N/A,25,120,passagiermaschine_747_static,416,A,3,20,20,15,15\nQatar Airways 8830,QTR8830,QR8830,Melbourne,MEL,Boeing 747,N/A,38,215,passagiermaschine_747_static,416,A,3,20,20,15,15\nCathay Pacific 3081,CPA3081,CX3081,Hong Kong,HKG,Boeing 747,N/A,36,295,passagiermaschine_747_static,416,A,3,20,20,15,15\nSingapore Airlines 7290,SIA7290,SQ7290,Singapore,SIN,Boeing 747,N/A,6,-3,passagiermaschine_747_static,416,A,3,20,20,15,15\nAir Force ABYSS,N/A,N/A,Ramstein Air Base,RMS,Lockheed C-130,Priority,17,-498,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nAir Force KILO,N/A,N/A,RAF Mildenhall,MHZ,Lockheed C-130,Priority,7,-529,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nAir Force SCRON,N/A,N/A,Peterson Air Force Base,COS,Lockheed C-130,Priority,38,-199,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nAir Force EDGAR,N/A,N/A,Bagram Air Base,OAI,Lockheed C-130,Priority,22,-509,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nIcelandair 614,ICE614,FI614,New York JFK,JFK,Boeing 757,N/A,17,77,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 937,DAL937,DL937,Denver,DEN,Boeing 757,N/A,52,272,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 1976,UAL1976,UA1976,San Francisco,SFO,Boeing 757,N/A,47,89,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 947,DAL947,DL947,Atlanta,ATL,Boeing 757,N/A,36,231,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 1432,UAL1432,UA1432,Los Angeles,LAX,Boeing 757,N/A,18,-489,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 714,UAL714,UA714,Chicago,ORD,Boeing 757,N/A,12,-124,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 724,UAL724,UA724,Honolulu,HNL,Boeing 757,N/A,45,155,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 2120,DAL2120,DL2120,Las Vegas,LAS,Boeing 757,N/A,11,88,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 2310,UAL2310,UA2310,Boston,BOS,Boeing 757,N/A,47,-428,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 8116,UAL8116,UA8116,Newark,EWR,Boeing 757,N/A,22,-104,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 1426,DAL1426,DL1426,Minneapolis,MSP,Boeing 757,N/A,29,-297,frachtmaschine_757_static,239,B,4,20,20,15,15\n";
        
        // string[] csvStringDelimed = csvString.Split('\n');
        // string[] newCSVArr = textAssetData.Split(new string{"\n"}, System.StringSplitOptions.None);

        // initialize an array of strings
        string[] allValues = new string[82];
        
        allValues[0] = "Air Force ONE,N/A,N/A,Air Force ONE,Andrews Air Force Base,AFB,Boeing 747,Priority,28,230,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[1] = "Korean Air 539,KAL539,KE539,KAL539,Seoul,ICN,Boeing 747,,36,-325,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[2] = "Lufthansa 419,DLH419,LH419,DLH419,Frankfurt,FRA,Boeing 747,,25,120,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[3] = "Qatar Airways 8830,QTR8830,QR8830,QTR8830,Melbourne,MEL,Boeing 747,,38,215,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[4] = "Cathay Pacific 3081,CPA3081,CX3081,CPA3081,Hong Kong,HKG,Boeing 747,,36,295,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[5] = "Singapore Airlines 7290,SIA7290,SQ7290,SIA7290,Singapore,SIN,Boeing 747,,6,-3,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[6] = "Air Force ABYSS,N/A,N/A,Air Force ABYSS,Ramstein Air Base,RMS,Lockheed C-130,Priority,17,-498,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[7] = "Air Force KILO,N/A,N/A,Air Force KILO,RAF Mildenhall,MHZ,Lockheed C-130,Priority,7,-529,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[8] = "Air Force SCRON,N/A,N/A,Air Force SCRON,Peterson Air Force Base,COS,Lockheed C-130,Priority,38,-199,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[9] = "Air Force EDGAR,N/A,N/A,Air Force EDGAR,Bagram Air Base,OAI,Lockheed C-130,Priority,22,-509,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[10] = "Icelandair 614,ICE614,FI614,ICE614,New York JFK,JFK,Boeing 757,,17,77,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[11] = "Delta 937,DAL937,DL937,DAL937,Denver,DEN,Boeing 757,,52,272,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[12] = "United 1976,UAL1976,UA1976,UAL1976,San Francisco,SFO,Boeing 757,,47,89,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[13] = "Delta 947,DAL947,DL947,DAL947,Atlanta,ATL,Boeing 757,,36,231,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[14] = "United 1432,UAL1432,UA1432,UAL1432,Los Angeles,LAX,Boeing 757,,18,-489,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[15] = "United 714,UAL714,UA714,UAL714,Chicago,ORD,Boeing 757,,12,-124,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[16] = "United 724,UAL724,UA724,UAL724,Honolulu,HNL,Boeing 757,,45,155,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[17] = "Delta 2120,DAL2120,DL2120,DAL2120,Las Vegas,LAS,Boeing 757,,11,88,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[18] = "United 2310,UAL2310,UA2310,UAL2310,Boston,BOS,Boeing 757,,47,-428,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[19] = "United 8116,UAL8116,UA8116,UAL8116,Newark,EWR,Boeing 757,,22,-104,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[20] = "Delta 1426,DAL1426,DL1426,DAL1426,Minneapolis,MSP,Boeing 757,,29,-297,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[21] = "Delta 1226,DAL1226,DL1226,DAL1226,Detroit,DTW,Boeing 757,,5,-296,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[22] = "easyJet Switzerland 1417,EZS1417,DS1417,EZS1417,Geneva,GVA,Airbus A320,,21,160,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[23] = "IndiGO 5306,IGO5306,6E5306,IGO5306,Chennai,MAA,Airbus A320,,19,-564,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[24] = "Austrian Airlines 295,AUA295,OS295,AUA295,Vienna,VIE,Airbus A320,,44,-476,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[25] = "Eurowings 8054,EWG8054,EW8054,EWG8054,Berlin,BER,Airbus A320,,46,94,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[26] = "JetBlue 900,JBU900,B6900,JBU900,Fort Lauderdale,FLL,Airbus A320,,37,198,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[27] = "Iberia Express 3925,IBS3925,I23925,IBS3925,Palma de Mallorca ,PMI,Airbus A320,,18,-376,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[28] = "Spirit 3374,NKS3374,NK3374,NKS3374,Memphis,MEM,Airbus A320,Emergency,10,189,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[29] = "Frontier 2510,FFT2510,F92510,FFT2510,Tampa,TPA,Airbus A320,,9,102,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[30] = "American Airlines 1686,AAL1686,AA1686,AAL1686,Phoenix,PHX,Airbus A320,,12,-454,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[31] = "American Airlines 2632,AAL2632,AA2632,AAL2632,Washington Dulles,IAD,Airbus A320,,27,287,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[32] = "United 539,UAL539,UA539,UAL539,San Antonio,SAT,Airbus A320,,46,-417,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[33] = "Delta 2388,DAL2388,DL2388,DAL2388,Seattle,SEA,Airbus A320,,21,86,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[34] = "JetBlue 1284,JBU1284,B61284,JBU1284,New York JFK,JFK,Airbus A320,,43,-433,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[35] = "JetBlue 399,JBU399,B6399,JBU399,LaGuardia,LGA,Airbus A320,,21,-144,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[36] = "Nile Air 141,NIA141,NP141,NIA141,Cairo,CAI,Airbus A320,,6,156,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[37] = "Allegiant Air 2278,AAY2278,G42278,AAY2278,Phoenix,IWA,Airbus A320,,24,-256,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[38] = "Delta 1543,DAL1543,DL1543,DAL1543,Dallas,DFW,Airbus A320,,44,-514,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[39] = "Avianca 127,AVA127,AV127,AVA127,Bogota,BOG,Airbus A320,,11,39,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[40] = "JetBlue 2626,JBU2626,B62626,JBU2626,Raleigh-Durham,RDU,Airbus A320,,53,-565,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[41] = "Delta 2350,DAL2350,DL2350,DAL2350,Indianapolis,IND,Airbus A320,,37,-160,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[42] = "IndiGO 702,IGO702,6E702,IGO702,Bengaluru,BLR,Airbus A320,,24,-42,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[43] = "Qatar Airways 235,QTR235,QR235,QTR235,Doha,DOH,Airbus A320,,12,-89,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[44] = "Air Arabia 482,ABY482,G9482,ABY482,Sharjah,SHJ,Airbus A320,,11,-2,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[45] = "Jetstar Asia 243,JSA243,3K243,JSA243,Singapore Changi,SIN,Airbus A320,,5,-410,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[46] = "Delta 1712,DAL1712,DL1712,DAL1712,Salt Lake City,SLC,Airbus A320,,47,-373,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[47] = "MEDEVAC N400SC,N/A,N/A,MEDEVAC N400SC,Boston Logan,BOS,Fairchild FH-227,Priority,17,-110,privatjet2_static,52,E,9,20,20,15,15";
        allValues[48] = "MEDEVAC N782AL,N/A,N/A,MEDEVAC N782AL,Steamboat Springs,SBS,Fairchild FH-227,Priority,46,-194,privatjet2_static,52,E,9,20,20,15,15";
        allValues[49] = "N711Z,N/A,N/A,N711Z,North Little Rock,ORK,Cessna 172,,20,-187,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[50] = "N8949U,N/A,N/A,N8949U,New Richmond,RNH,Beechcraft Bonanza,,41,55,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[51] = "N7368R,N/A,N/A,N7368R,St. George,SGU,Beechcraft Bonanza,,25,177,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[52] = "N168NL,N/A,N/A,N168NL,Provo,PVU,Beechcraft Bonanza,,37,-138,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[53] = "N168NL,N/A,N/A,N168NL,Burbank,BUR,Beechcraft Bonanza,,26,-562,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[54] = "N733SW,N/A,N/A,N733SW,Cedar City,CDC,Beechcraft Bonanza,,13,-265,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[55] = "VH-DDC,N/A,N/A,VH-DDC,Coffs Harbour,CFS,Beechcraft Bonanza,,31,-575,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[56] = "GTX510,N/A,N/A,GTX510,El Paso,ELP,Beechcraft Bonanza,,31,246,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[57] = "GTX510,N/A,N/A,GTX510,Tulsa,TUL,Beechcraft Bonanza,,45,-237,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[58] = "N789J,N/A,N/A,N789J,Guntersville,8A1,Beechcraft Bonanza,,49,-330,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[59] = "N67443,N/A,N/A,N67443,Hubler Field,ID00,Cessna 172,,46,164,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[60] = "N46953,N/A,N/A,N46953,North Perry,HWO,Cessna 172,,46,-394,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[61] = "N48068,N/A,N/A,N48068,South Valley,U42,Cessna 172,,59,-366,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[62] = "N95787,N/A,N/A,N95787,Dane County,MSN,Cessna 172,,22,-247,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[63] = "N48SU,N/A,N/A,N48SU,Griffith-Merrillville,05C,Cessna 172,,22,-190,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[64] = "N49469,N/A,N/A,N49469,Fort Lauderdale Exec,FXR,Cessna 172,,17,-178,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[65] = "N340SX,N/A,N/A,N340SX,Elbert County-Patz FId,EBA,Cessna 172,Emergency,51,-553,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[66] = "N4786P,N/A,N/A,N4786P,Daugherty Field,LGB,Cessna 172,,27,-71,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[67] = "N4852P,N/A,N/A,N4852P,Southwest Georgia,ABY,Cessna 172,,34,-569,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[68] = "N5335P,N/A,N/A,N5335P,Hayward Exec,HWD,Cessna 172,,56,-75,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[69] = "N5151 B,N/A,N/A,N5151 B,Santa Clara County,RHV,Cessna 172,,20,-92,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[70] = "N757NJ,N/A,N/A,N757NJ,Lakeland Linder,LAL,Cessna 172,,51,-433,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[71] = "N48787,N/A,N/A,N48787,Glendale Muni,GEU,Cessna 172,,41,95,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[72] = "N64823,N/A,N/A,N64823,Tampa Exec,VDF,Cessna 172,,21,38,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[73] = "N48967,N/A,N/A,N48967,Myrtle Beach,MYR,Cessna 172,,15,-171,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[74] = "N152DM,N/A,N/A,N152DM,Jacksonville Exec At Craig,CRG,Cessna 172,,39,167,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[75] = "TMB193,N/A,N/A,TMB193,Oakland,OAK,Honda HA-420,,58,-265,privatjet1_static,5,F,10,20,20,15,15";
        allValues[76] = "GTW184,N/A,N/A,GTW184,Van Nuys,VNY,Honda HA-420,,27,33,privatjet1_static,5,F,10,20,20,15,15";
        allValues[77] = "N143RP,N/A,N/A,N143RP,St Louis,SUS,Honda HA-420,,36,-93,privatjet1_static,5,F,10,20,20,15,15";
        allValues[78] = "JIT679,N/A,N/A,JIT679,Piedmont Triad,GSO,Honda HA-420,,27,-161,privatjet1_static,5,F,10,20,20,15,15";
        allValues[79] = "JIT100,N/A,N/A,JIT100,Boca Raton,BCT,Honda HA-420,,54,-388,privatjet1_static,5,F,10,20,20,15,15";
        allValues[80] = "JIT41,N/A,N/A,JIT41,Salt Lake City,SLC,Honda HA-420,,30,-299,privatjet1_static,5,F,10,20,20,15,15";
        allValues[81] = "JIT404,N/A,N/A,JIT404,Arlington,GKY,Honda HA-420,,35,91,privatjet1_static,5,F,10,20,20,15,15";






    for (int i = 0; i < allValues.Length; i++) {
        string[] dataValues = allValues[i].Split(',');
        PlaneData planeData = new PlaneData();
        // assign data values to plane data
        planeData.callSign = dataValues[0];
        planeData.iata = dataValues[1];
        planeData.icao = dataValues[2];
        planeData.ident = dataValues[3];
        planeData.arriveDepart = dataValues[4];
        planeData.adIATA = dataValues[5];
        planeData.planeType = dataValues[6];
        planeData.priority = dataValues[7];
        planeData.fuel = float.Parse(dataValues[8]);
        planeData.timeTillOnTime = float.Parse(dataValues[9]);
        planeData.planeAsset = dataValues[10];
        planeData.maxPassengers = int.Parse(dataValues[11]);
        planeData.planeSize = dataValues[12][0];
        planeData.minFuel = float.Parse(dataValues[13]);
        planeData.timeToTerminal = float.Parse(dataValues[14]);
        planeData.timeToRunway = float.Parse(dataValues[15]);
        planeData.timeToLand = float.Parse(dataValues[16]);
        planeData.timeToAir = float.Parse(dataValues[17]);
        allPlaneData.Add(planeData);
    }
        
    }


    public void selectDepartingAirplane(int index) {
        if (selectedAirplane == terminal._planes[index]) {  // deselect the currently selected plane
            selectedAirplane = null;
            selectedButton = -1;
            isDepartureButton = true;
            display(false, selectedAirplane);
        }else{  // select the selected plane
            selectedAirplane = terminal._planes[index];
            selectedButton = index;
            isDepartureButton = true;
            display(true, selectedAirplane);
        }
    }

    // called when a plane is selected from the arrivals UI
    public void selectArrivingAirplane(int index) {
        if (selectedAirplane == sky._planes[index]) {  // deselect the currently selected plane
            selectedAirplane = null;
            selectedButton = -1;
            isDepartureButton = false;
            display(false, selectedAirplane);
        }else{  // select the selected plane
            selectedAirplane = sky._planes[index];
            selectedButton = index;
            isDepartureButton = false;
            display(true, selectedAirplane);
        }
    }


    // called when a runway is selected from the runway UI (takes in a runway and a boolean representing whether the plane is 
    // departing or arriving)
    public void selectRunway(Runway runway) {
        if (selectedAirplane != null && (selectedAirplane.status == PlaneStatus.Terminal || 
        selectedAirplane.status ==  PlaneStatus.Circling)) {
              // ensure that the user has selected a plane and that the plane is at the terminal or circling
            if (runway.open) {
                selectedButton = -1;
                if(selectedAirplane.status == PlaneStatus.Terminal){
                    selectedAirplane.status = PlaneStatus.Taxiing;
                    if(runway.index == 1){
                        selectedAirplane.GetComponent<Animator>().Play("Taxiing");
                    }else{
                        selectedAirplane.GetComponent<Animator>().Play("Taxiing2");
                    }
                    
                }else{
                    selectedAirplane.status = PlaneStatus.Landing;
                    if(runway.index == 1){
                        selectedAirplane.GetComponent<Animator>().Play("Landing2");
                    }else{
                        selectedAirplane.GetComponent<Animator>().Play("Landing");
                    }
                    planeSound2.time = 12.0f;
                    planeSound2.Play();
                }
                runway.plane = selectedAirplane;
                runway.open = false;
            } else {
                // there is already a plane on that runway!
            }
            
        } else {
            // you must selected a plane first!
        }
       
    }
    //display the information on the monitor
    public void display(bool Display, Airplane a){
            GameObject g = FindInActiveObjectByName("FlightDisplay");
            GameObject g2 = FindInActiveObjectByName("ProjectorLightLeft");
            GameObject g3 = FindInActiveObjectByName("ProjectorLightRight");
        if(Display){
            g.SetActive(true);
            g2.SetActive(true);
            g3.SetActive(true);
        }else{
            g.SetActive(false);
            g2.SetActive(false);
            g3.SetActive(false);
        }
    }

    //mainly to initialize the panel elements so we don't have to find them every time
    
    void Update(){
        if(selectedAirplane != null){
            panelElements[0].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane._flightName;
            int Fuel = (int) selectedAirplane.fuelLevel;
            panelElements[1].GetComponent<TMPro.TextMeshProUGUI>().text = Fuel.ToString() + "%";
            panelElements[2].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane.status.ToString();
            panelElements[3].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane.waitingTimeSeconds.ToString();
            panelElements[4].GetComponent<TMPro.TextMeshProUGUI>().text = "Passengers: " + selectedAirplane.passengersOnBoard + "\n PlaneType: " + selectedAirplane.planeType + "\n Priority: " + selectedAirplane.priority;
        }
    }

    //find object
    public static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
