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
        
        allValues[0] = "Air Force ONE,N/A,N/A,Andrews Air Force Base,AFB,Boeing 747,Priority,28,230,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[1] = "Korean Air 539,KAL539,KE539,Seoul,ICN,Boeing 747,N/A,36,-325,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[2] = "Lufthansa 419,DLH419,LH419,Frankfurt,FRA,Boeing 747,N/A,25,120,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[3] = "Qatar Airways 8830,QTR8830,QR8830,Melbourne,MEL,Boeing 747,N/A,38,215,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[4] = "Cathay Pacific 3081,CPA3081,CX3081,Hong Kong,HKG,Boeing 747,N/A,36,295,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[5] = "Singapore Airlines 7290,SIA7290,SQ7290,Singapore,SIN,Boeing 747,N/A,6,-3,passagiermaschine_747_static,416,A,3,20,20,15,15";
        allValues[6] = "Air Force ABYSS,N/A,N/A,Ramstein Air Base,RMS,Lockheed C-130,Priority,17,-498,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[7] = "Air Force KILO,N/A,N/A,RAF Mildenhall,MHZ,Lockheed C-130,Priority,7,-529,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[8] = "Air Force SCRON,N/A,N/A,Peterson Air Force Base,COS,Lockheed C-130,Priority,38,-199,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[9] = "Air Force EDGAR,N/A,N/A,Bagram Air Base,OAI,Lockheed C-130,Priority,22,-509,frachtmaschine_antonov_static,124,A,3,20,20,15,15";
        allValues[10] = "Icelandair 614,ICE614,FI614,New York JFK,JFK,Boeing 757,N/A,17,77,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[11] = "Delta 937,DAL937,DL937,Denver,DEN,Boeing 757,N/A,52,272,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[12] = "United 1976,UAL1976,UA1976,San Francisco,SFO,Boeing 757,N/A,47,89,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[13] = "Delta 947,DAL947,DL947,Atlanta,ATL,Boeing 757,N/A,36,231,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[14] = "United 1432,UAL1432,UA1432,Los Angeles,LAX,Boeing 757,N/A,18,-489,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[15] = "United 714,UAL714,UA714,Chicago,ORD,Boeing 757,N/A,12,-124,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[16] = "United 724,UAL724,UA724,Honolulu,HNL,Boeing 757,N/A,45,155,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[17] = "Delta 2120,DAL2120,DL2120,Las Vegas,LAS,Boeing 757,N/A,11,88,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[18] = "United 2310,UAL2310,UA2310,Boston,BOS,Boeing 757,N/A,47,-428,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[19] = "United 8116,UAL8116,UA8116,Newark,EWR,Boeing 757,N/A,22,-104,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[20] = "Delta 1426,DAL1426,DL1426,Minneapolis,MSP,Boeing 757,N/A,29,-297,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[21] = "Delta 1226,DAL1226,DL1226,Detroit,DTW,Boeing 757,N/A,5,-296,frachtmaschine_757_static,239,B,4,20,20,15,15";
        allValues[22] = "easyJet Switzerland 1417,EZS1417,DS1417,Geneva,GVA,Airbus A320,N/A,21,160,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[23] = "IndiGO 5306,IGO5306,6E5306,Chennai,MAA,Airbus A320,N/A,19,-564,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[24] = "Austrian Airlines 295,AUA295,OS295,Vienna,VIE,Airbus A320,N/A,44,-476,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[25] = "Eurowings 8054,EWG8054,EW8054,Berlin,BER,Airbus A320,N/A,46,94,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[26] = "JetBlue 900,JBU900,B6900,Fort Lauderdale,FLL,Airbus A320,N/A,37,198,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[27] = "Iberia Express 3925,IBS3925,I23925,Palma de Mallorca ,PMI,Airbus A320,N/A,18,-376,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[28] = "Spirit 3374,NKS3374,NK3374,Memphis,MEM,Airbus A320,Emergency,10,189,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[29] = "Frontier 2510,FFT2510,F92510,Tampa,TPA,Airbus A320,N/A,9,102,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[30] = "American Airlines 1686,AAL1686,AA1686,Phoenix,PHX,Airbus A320,N/A,12,-454,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[31] = "American Airlines 2632,AAL2632,AA2632,Washington Dulles,IAD,Airbus A320,N/A,27,287,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[32] = "United 539,UAL539,UA539,San Antonio,SAT,Airbus A320,N/A,46,-417,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[33] = "Delta 2388,DAL2388,DL2388,Seattle,SEA,Airbus A320,N/A,21,86,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[34] = "JetBlue 1284,JBU1284,B61284,New York JFK,JFK,Airbus A320,N/A,43,-433,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[35] = "JetBlue 399,JBU399,B6399,LaGuardia,LGA,Airbus A320,N/A,21,-144,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[36] = "Nile Air 141,NIA141,NP141,Cairo,CAI,Airbus A320,N/A,6,156,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[37] = "Allegiant Air 2278,AAY2278,G42278,Phoenix,IWA,Airbus A320,N/A,24,-256,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[38] = "Delta 1543,DAL1543,DL1543,Dallas,DFW,Airbus A320,N/A,44,-514,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[39] = "Avianca 127,AVA127,AV127,Bogota,BOG,Airbus A320,N/A,11,39,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[40] = "JetBlue 2626,JBU2626,B62626,Raleigh-Durham,RDU,Airbus A320,N/A,53,-565,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[41] = "Delta 2350,DAL2350,DL2350,Indianapolis,IND,Airbus A320,N/A,37,-160,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[42] = "IndiGO 702,IGO702,6E702,Bengaluru,BLR,Airbus A320,N/A,24,-42,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[43] = "Qatar Airways 235,QTR235,QR235,Doha,DOH,Airbus A320,N/A,12,-89,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[44] = "Air Arabia 482,ABY482,G9482,Sharjah,SHJ,Airbus A320,N/A,11,-2,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[45] = "Jetstar Asia 243,JSA243,3K243,Singapore Changi,SIN,Airbus A320,N/A,5,-410,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[46] = "Delta 1712,DAL1712,DL1712,Salt Lake City,SLC,Airbus A320,N/A,47,-373,passagiermaschine_a320_static,180,C,5,20,20,15,15";
        allValues[47] = "MEDEVAC N400SC,N/A,N/A,Boston Logan,BOS,Fairchild FH-227,Priority,10,-231,privatjet2_static,52,E,9,20,20,15,15";
        allValues[48] = "MEDEVAC N782AL,N/A,N/A,Steamboat Springs,SBS,Fairchild FH-227,Priority,11,-293,privatjet2_static,52,E,9,20,20,15,15";
        allValues[49] = "N711Z,N/A,N/A,North Little Rock,ORK,Cessna 172,N/A,23,-568,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[50] = "N8949U,N/A,N/A,New Richmond,RNH,Beechcraft Bonanza,N/A,39,-105,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[51] = "N7368R,N/A,N/A,St. George,SGU,Beechcraft Bonanza,N/A,17,-412,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[52] = "N168NL,N/A,N/A,Provo,PVU,Beechcraft Bonanza,N/A,29,215,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[53] = "N168NL,N/A,N/A,Burbank,BUR,Beechcraft Bonanza,N/A,10,-347,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[54] = "N733SW,N/A,N/A,Cedar City,CDC,Beechcraft Bonanza,N/A,56,-555,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[55] = "VH-DDC,N/A,N/A,Coffs Harbour,CFS,Beechcraft Bonanza,N/A,34,-584,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[56] = "GTX510,N/A,N/A,El Paso,ELP,Beechcraft Bonanza,N/A,11,-465,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[57] = "GTX510,N/A,N/A,Tulsa,TUL,Beechcraft Bonanza,N/A,30,-248,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[58] = "N789J,N/A,N/A,Guntersville,8A1,Beechcraft Bonanza,N/A,10,-16,sportflugzeug_beech_static,6,F,10,20,20,15,15";
        allValues[59] = "N67443,N/A,N/A,Hubler Field,ID00,Cessna 172,N/A,51,-380,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[60] = "N46953,N/A,N/A,North Perry,HWO,Cessna 172,N/A,19,220,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[61] = "N48068,N/A,N/A,South Valley,U42,Cessna 172,N/A,17,-371,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[62] = "N95787,N/A,N/A,Dane County,MSN,Cessna 172,N/A,49,-465,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[63] = "N48SU,N/A,N/A,Griffith-Merrillville,05C,Cessna 172,N/A,50,-443,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[64] = "N49469,N/A,N/A,Fort Lauderdale Exec,FXR,Cessna 172,N/A,21,-581,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[65] = "N340SX,N/A,N/A,Elbert County-Patz FId,EBA,Cessna 172,Emergency,56,46,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[66] = "N4786P,N/A,N/A,Daugherty Field,LGB,Cessna 172,N/A,37,45,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[67] = "N4852P,N/A,N/A,Southwest Georgia,ABY,Cessna 172,N/A,22,-202,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[68] = "N5335P,N/A,N/A,Hayward Exec,HWD,Cessna 172,N/A,20,-256,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[69] = "N5151 B,N/A,N/A,Santa Clara County,RHV,Cessna 172,N/A,16,51,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[70] = "N757NJ,N/A,N/A,Lakeland Linder,LAL,Cessna 172,N/A,28,-382,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[71] = "N48787,N/A,N/A,Glendale Muni,GEU,Cessna 172,N/A,37,-479,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[72] = "N64823,N/A,N/A,Tampa Exec,VDF,Cessna 172,N/A,15,20,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[73] = "N48967,N/A,N/A,Myrtle Beach,MYR,Cessna 172,N/A,12,-244,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[74] = "N152DM,N/A,N/A,Jacksonville Exec At Craig,CRG,Cessna 172,N/A,41,-561,sportflugzeug_cessna_static,4,F,10,20,20,15,15";
        allValues[75] = "TMB193,N/A,N/A,Oakland,OAK,Honda HA-420,N/A,17,-133,privatjet1_static,5,F,10,20,20,15,15";
        allValues[76] = "GTW184,N/A,N/A,Van Nuys,VNY,Honda HA-420,N/A,36,-551,privatjet1_static,5,F,10,20,20,15,15";
        allValues[77] = "N143RP,N/A,N/A,St Louis,SUS,Honda HA-420,N/A,33,3,privatjet1_static,5,F,10,20,20,15,15";
        allValues[78] = "JIT679,N/A,N/A,Piedmont Triad,GSO,Honda HA-420,N/A,15,142,privatjet1_static,5,F,10,20,20,15,15";
        allValues[79] = "JIT100,N/A,N/A,Boca Raton,BCT,Honda HA-420,N/A,41,41,privatjet1_static,5,F,10,20,20,15,15";
        allValues[80] = "JIT41,N/A,N/A,Salt Lake City,SLC,Honda HA-420,N/A,52,175,privatjet1_static,5,F,10,20,20,15,15";
        allValues[81] = "JIT404,N/A,N/A,Arlington,GKY,Honda HA-420,N/A,25,-439,privatjet1_static,5,F,10,20,20,15,15";






    for (int i = 0; i < allValues.Length; i++) {
        string[] dataValues = allValues[i].Split(',');
        PlaneData planeData = new PlaneData();
        // assign data values to plane data
        planeData.callSign = dataValues[0];
        planeData.iata = dataValues[1];
        planeData.icao = dataValues[2];
        planeData.arriveDepart = dataValues[3];
        planeData.adIATA = dataValues[4];
        planeData.planeType = dataValues[5];
        planeData.priority = dataValues[6];
        planeData.fuel = float.Parse(dataValues[7]);
        planeData.timeTillOnTime = float.Parse(dataValues[8]);
        planeData.planeAsset = dataValues[9];
        planeData.maxPassengers = int.Parse(dataValues[10]);
        planeData.planeSize = dataValues[11][0];
        planeData.minFuel = float.Parse(dataValues[12]);
        planeData.timeToTerminal = float.Parse(dataValues[13]);
        planeData.timeToRunway = float.Parse(dataValues[14]);
        planeData.timeToLand = float.Parse(dataValues[15]);
        planeData.timeToAir = float.Parse(dataValues[16]);
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
            panelElements[4].GetComponent<TMPro.TextMeshProUGUI>().text = "Passengers: " + selectedAirplane.passengersOnBoard + ", PlaneType: " + selectedAirplane.planeType + ", Priority: " + selectedAirplane.priority;
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
