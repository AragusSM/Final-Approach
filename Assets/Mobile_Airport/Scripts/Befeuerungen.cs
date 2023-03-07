using UnityEngine;
using System.Collections;

public class Befeuerungen : MonoBehaviour {
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//
// Airport Runway and Taxiway Lights Controller
//
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
 

    //-------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------
    //
    // Airport Runway and Taxiway Lights Config
    //

    //-------------------------------------------------------------------------------------------------------------------------------------------
    // Glide Angle Approach Lights 
    //-------------------------------------------------------------------------------------------------------------------------------------------
    public enum GlideAngleApproachLightsStatus 
    {
        Off,
        Much_too_high,
        Too_high,
        Ok,
        Too_low,
        Much_to_low,
    }

    //-- Anflug Gleitwinkel Feuer aktuelle Stati
    public GlideAngleApproachLightsStatus glideApproachFireStatusRunway04  = GlideAngleApproachLightsStatus.Off;
    public GlideAngleApproachLightsStatus glideApproachFireStatusRunway24  = GlideAngleApproachLightsStatus.Off;
    public GlideAngleApproachLightsStatus glideApproachFireStatusRunway32l = GlideAngleApproachLightsStatus.Off;
    public GlideAngleApproachLightsStatus glideApproachFireStatusRunway32r = GlideAngleApproachLightsStatus.Off;
    public GlideAngleApproachLightsStatus glideApproachFireStatusRunway14l = GlideAngleApproachLightsStatus.Off;
    public GlideAngleApproachLightsStatus glideApproachFireStatusRunway14r = GlideAngleApproachLightsStatus.Off;
    //-------------------------------------------------------------------------------------------------------------------------------------------


    //-------------------------------------------------------------------------------------------------------------------------------------------
    //  Runway Endfire
    //-------------------------------------------------------------------------------------------------------------------------------------------
    public enum RunwayEndfireStatus
    {
        Landing,
        Start,
    }
    public RunwayEndfireStatus rundwayEndfireStatusRunway04  = RunwayEndfireStatus.Landing;
    public RunwayEndfireStatus rundwayEndfireStatusRunway24  = RunwayEndfireStatus.Landing;
    public RunwayEndfireStatus rundwayEndfireStatusRunway32l = RunwayEndfireStatus.Landing;
    public RunwayEndfireStatus rundwayEndfireStatusRunway32r = RunwayEndfireStatus.Landing;
    public RunwayEndfireStatus rundwayEndfireStatusRunway14l = RunwayEndfireStatus.Landing;
    public RunwayEndfireStatus rundwayEndfireStatusRunway14r = RunwayEndfireStatus.Landing;
    //-------------------------------------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------------------------------------
    //  Runway Underfloor Fire
    //-------------------------------------------------------------------------------------------------------------------------------------------
    public enum RunwayUnderfloorFireStatus
    {
        Off,
        On,
        Runlight,
    }

    //-- Unterflurfeuer aktuelle Stati
    public RunwayUnderfloorFireStatus underfloorfireStatusRunyway06  = RunwayUnderfloorFireStatus.Off;
    public RunwayUnderfloorFireStatus underfloorfireStatusRunyway24  = RunwayUnderfloorFireStatus.Off;
    public RunwayUnderfloorFireStatus underfloorfireStatusRunyway32l = RunwayUnderfloorFireStatus.Off;
    public RunwayUnderfloorFireStatus underfloorfireStatusRunyway32r = RunwayUnderfloorFireStatus.Off;
    public RunwayUnderfloorFireStatus underfloorfireStatusRunyway14l = RunwayUnderfloorFireStatus.Off;
    public RunwayUnderfloorFireStatus underfloorfireStatusRunyway14r = RunwayUnderfloorFireStatus.Off;
    //-------------------------------------------------------------------------------------------------------------------------------------------

    
    //-------------------------------------------------------------------------------------------------------------------------------------
    // Unterflurfeuer Lauflicht Status
    //-------------------------------------------------------------------------------------------------------------------------------------
    int unterflurfeuer_lauflicht_active = 0; 
    float unterflurfeuer_lauflicht_timer = 0.0f;

    //-------------------------------------------------------------------------------------------------------------------------------------
    const int MAX_ANFLUG_GLEITWINKEL_FEUER = 4*6;
    const int MAX_STARTBAHN_ENDFEUER = 19*6;
    const int MAX_UNTERFLUR_FEUER = (32*6) + (16*2*6);
    const int MAX_ATR_MAST_KAEFIG = (32*2) + (16*4);
    const int MAX_ATR_MAST = (5*9) + (1*6);
    const int MAX_TAXIWAY_ATR_MAST = 124 + 63 + 68 + 213 + 120 + 121 + 40 + 48 + 31 + 8 + 16 + 22;

    const int ROLLFELD_06  = 0;
    const int ROLLFELD_24  = 1;
    const int ROLLFELD_14L = 2;
    const int ROLLFELD_14R = 3;
    const int ROLLFELD_32L = 4;
    const int ROLLFELD_32R = 5;
    
    const int ATR_MAST_TYP_3M_3ER        = 0;
    const int ATR_MAST_TYP_3M_3ER_SIGNAL = 1;
    const int ATR_MAST_TYP_3M_5ER        = 2;
    const int ATR_MAST_TYP_3M_5ER_SIGNAL = 3;
    const int ATR_MAST_TYP_6M_3ER        = 4;
    const int ATR_MAST_TYP_6M_3ER_SIGNAL = 5;
    const int ATR_MAST_TYP_6M_5ER        = 6;
    const int ATR_MAST_TYP_6M_5ER_SIGNAL = 7;
    const int ATR_MAST_TYP_9M_3ER        = 8;
    const int ATR_MAST_TYP_9M_3ER_SIGNAL = 9;
    const int ATR_MAST_TYP_9M_5ER        = 10;
    const int ATR_MAST_TYP_9M_5ER_SIGNAL = 11;

    const int ATR_SIGNALFARBE_BLAU  = 0;
    const int ATR_SIGNALFARBE_GRUEN = 1;
    const int ATR_SIGNALFARBE_ROT   = 2;

    const int GLEITWINKEL_FEUER_ANORDNUNG_A = 0;  // Aussen
    const int GLEITWINKEL_FEUER_ANORDNUNG_B = 1;
    const int GLEITWINKEL_FEUER_ANORDNUNG_C = 2;
    const int GLEITWINKEL_FEUER_ANORDNUNG_D = 3; // Innen

    const int FARBE_ROT = 0;
    const int FARBE_WEISS = 1;

    const float TAXIWAY_ATR_HOCH_EINFACH          = 0.0f;
    const float TAXIWAY_ATR_HOCH_DOPPELT          = 1.0f;
    const float TAXIWAY_ATR_HOCH_SIGNAL           = 2.0f;
    const float TAXIWAY_ATR_HOCH_SIGNAL_EINFACH   = 3.0f;
    const float TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT   = 4.0f;

    const float TAXIWAY_ATR_MITTEL_EINFACH          = 5.0f;
    const float TAXIWAY_ATR_MITTEL_DOPPELT          = 6.0f;
    const float TAXIWAY_ATR_MITTEL_SIGNAL           = 7.0f;
    const float TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH   = 8.0f;
    const float TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT   = 9.0f;

    const float TAXIWAY_ATR_NIEDRIG_EINFACH          = 10.0f;
    const float TAXIWAY_ATR_NIEDRIG_DOPPELT          = 11.0f;
    const float TAXIWAY_ATR_NIEDRIG_SIGNAL           = 12.0f;
    const float TAXIWAY_ATR_NIEDRIG_SIGNAL_EINFACH   = 13.0f;
    const float TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT   = 14.0f;
    //---------------------------------------------
    //---------------------------------------------
    //---------------------------------------------
    public float glideAngleFireMaxViewDistance = 250.0f;

    public float rundwayEndfireMaxViewDistance = 200.0f;

    public float underfloorFireMaxViewDistance = 200.0f;

    public float atrCageMaxViewDistance = 500.0f;

    public float atrRundwayEndMaxViewDistance = 300.0f;

    public float atrTaxiwayMaxViewDistance = 250.0f;
    //---------------------------------------------
    //---------------------------------------------
    //---------------------------------------------
    GameObject anflug_gleitwinkel_feuer_aus;
    GameObject anflug_gleitwinkel_feuer_an_rot;
    GameObject anflug_gleitwinkel_feuer_an_weiss;

    GameObject startbahn_endfeuer_gruen;
    GameObject startbahn_endfeuer_rot;

    GameObject unterflurfeuer_aus;
    GameObject unterflurfeuer_rot_an;
    GameObject unterflurfeuer_weiss_an;

    GameObject anflugfeuersystem_atr_mast_kaefig;

    GameObject anflugfeuersystem_atr_mast_3m_3er;
    GameObject anflugfeuersystem_atr_mast_3m_5er;
    GameObject anflugfeuersystem_atr_mast_6m_3er;
    GameObject anflugfeuersystem_atr_mast_6m_5er;
    GameObject anflugfeuersystem_atr_mast_9m_3er;
    GameObject anflugfeuersystem_atr_mast_9m_5er;
    
    GameObject taxiway_atr_hoch_signal_blau;
    GameObject taxiway_atr_mittel_signal_blau;
    GameObject taxiway_atr_niedrig_signal_blau;
    //---------------------------------------------
    //---------------------------------------------
    //---------------------------------------------
    GameObject[] gleitwinkelfeuer_obj;
    int[] gleitwinkelfeuer_last_frame_status;

    GameObject[] startbahn_endfeuer_obj;
    int[] startbahn_endfeuer_last_frame_status;

    GameObject[] unterflurfeuer_obj;
    float[] unterflurfeuer_xpos;
    float[] unterflurfeuer_zpos;
    float[] unterflurfeuer_yrot;
    int[] unterflurfeuer_farbe;
    int[] unterflurfeuer_lauflicht_id;
    int[] unterflurfeuer_rollfeld;
    int[] unterflurfeuer_last_frame_status;

    int unterflurfeuer_last_frame_lauflicht_id;

    GameObject[] anflugfeuersystem_atr_mast_kaefig_obj;
    float[] anflugfeuersystem_atr_mast_kaefig_xpos;
    float[] anflugfeuersystem_atr_mast_kaefig_zpos;
    float[] anflugfeuersystem_atr_mast_kaefig_yrot;
    bool[] anflugfeuersystem_atr_mast_kaefig_status;

    GameObject[] anflugfeuersystem_atr_mast_obj;
    int[] anflugfeuersystem_atr_mast_status;

    GameObject[] taxiway_atr_mast_obj;
    int[] taxiway_atr_mast_status;
    
    Transform airportLightsGroup;

//--------------------------------------------------------------------------------------------------
float[] anflug_gleitwinkelfeuer_position =
{
    //-- rollfeld 06
    62.46215f, 925.2529f,
    54.6647f, 927.0416f,
    46.86725f, 928.8304f,
    39.0698f, 930.6192f,

    //-- rollfeld 24
    -620.1794f, -947.875f,
    -612.3854f, -949.6791f,
    -604.5915f, -951.4831f,
    -596.7976f, -953.2872f,

    //-- rollfeld 14L
    1207.53f, -651.6013f,
    1207.53f, -643.6013f,
    1207.53f, -635.6013f,
    1207.53f, -627.6013f,

    //-- rollfeld 14R
    775.6161f, 496.5689f,
    775.6161f, 504.5689f,
    775.6161f, 512.5689f,
    775.6161f, 520.5689f,

    //-- rollfeld 32L
    -404.4639f, 614.6451f,
    -404.4639f, 606.6451f,
    -404.4639f, 598.6451f,
    -404.4639f, 590.6451f,

    //-- rollfeld 32R
    -1971.604f, -540.8528f,
    -1971.604f, -548.8528f,
    -1971.604f, -556.8528f,
    -1971.604f, -564.8528f,

};
float[] anflug_gleitwinkelfeuer_rotation =   
{
    //-- rollfeld 06
    -77.07953f,
    -77.07953f,
    -77.07953f,
    -77.07953f,

    //-- rollfeld 24
    103.0327f,
    103.0327f,
    103.0327f,
    103.0327f,

    //-- rollfeld 14L
    0.0f,
    0.0f,
    0.0f,
    0.0f,

    //-- rollfeld 14R
    0.0f,
    0.0f,
    0.0f,
    0.0f,

    //-- rollfeld 32L
    180.0f,
    180.0f,
    180.0f,
    180.0f,

    //-- rollfeld 32R
    180.0f,
    180.0f,
    180.0f,
    180.0f,
};

int[] anflug_gleitwinkelfeuer_rollfeld_anordnung =   
{
    //-- rollfeld 06
    ROLLFELD_06,GLEITWINKEL_FEUER_ANORDNUNG_A,
    ROLLFELD_06,GLEITWINKEL_FEUER_ANORDNUNG_B,
    ROLLFELD_06,GLEITWINKEL_FEUER_ANORDNUNG_C,
    ROLLFELD_06,GLEITWINKEL_FEUER_ANORDNUNG_D,

    //-- rollfeld 24
    ROLLFELD_24,GLEITWINKEL_FEUER_ANORDNUNG_A,
    ROLLFELD_24,GLEITWINKEL_FEUER_ANORDNUNG_B,
    ROLLFELD_24,GLEITWINKEL_FEUER_ANORDNUNG_C,
    ROLLFELD_24,GLEITWINKEL_FEUER_ANORDNUNG_D,

    //-- rollfeld 14L
    ROLLFELD_14L,GLEITWINKEL_FEUER_ANORDNUNG_A,
    ROLLFELD_14L,GLEITWINKEL_FEUER_ANORDNUNG_B,
    ROLLFELD_14L,GLEITWINKEL_FEUER_ANORDNUNG_C,
    ROLLFELD_14L,GLEITWINKEL_FEUER_ANORDNUNG_D,

    //-- rollfeld 14R
    ROLLFELD_14R,GLEITWINKEL_FEUER_ANORDNUNG_A,
    ROLLFELD_14R,GLEITWINKEL_FEUER_ANORDNUNG_B,
    ROLLFELD_14R,GLEITWINKEL_FEUER_ANORDNUNG_C,
    ROLLFELD_14R,GLEITWINKEL_FEUER_ANORDNUNG_D,

    //-- rollfeld 32L
    ROLLFELD_32L,GLEITWINKEL_FEUER_ANORDNUNG_A,
    ROLLFELD_32L,GLEITWINKEL_FEUER_ANORDNUNG_B,
    ROLLFELD_32L,GLEITWINKEL_FEUER_ANORDNUNG_C,
    ROLLFELD_32L,GLEITWINKEL_FEUER_ANORDNUNG_D,

    //-- rollfeld 32R
    ROLLFELD_32R,GLEITWINKEL_FEUER_ANORDNUNG_A,
    ROLLFELD_32R,GLEITWINKEL_FEUER_ANORDNUNG_B,
    ROLLFELD_32R,GLEITWINKEL_FEUER_ANORDNUNG_C,
    ROLLFELD_32R,GLEITWINKEL_FEUER_ANORDNUNG_D,
};
//--------------------------------------------------------------------------------------------------
float[] startbahn_endfeuer_position =
{
    //-- rollfeld 06
    94.27407f, 1210.962f,
    75.12434f, 1216.458f,
    84.34458f, 1213.799f,
    68.83643f, 1090.866f,
    31.07497f, 1101.932f,
    73.9427f, 1108.227f,
    36.18124f, 1119.294f,
    78.70856f, 1123.886f,
    40.9471f, 1134.953f,
    83.13398f, 1139.205f,
    45.37255f, 1150.271f,
    87.97658f, 1155.523f,
    50.21516f, 1166.589f,
    92.80325f, 1172.497f,
    55.04182f, 1183.564f,
    97.93073f, 1189.738f,
    60.1693f, 1200.804f,
    103.4218f, 1208.323f,
    65.66035f, 1219.389f,
 
    //-- rollfeld 24
    -593.2039f, -1164.841f,
    -630.9551f, -1153.74f,
    -587.6959f, -1146.261f,
    -625.4471f, -1135.161f,
    -582.5526f, -1129.026f,
    -620.3039f, -1117.925f,
    -577.7104f, -1112.055f,
    -615.4617f, -1100.954f,
    -572.853f, -1095.742f,
    -610.6042f, -1084.641f,
    -568.4135f, -1080.427f,
    -606.1649f, -1069.327f,
    -563.6333f, -1064.773f,
    -601.3846f, -1053.672f,
    -558.5112f, -1047.416f,
    -596.2625f, -1036.315f,
    -611.883f, -1159.234f,
    -602.6652f, -1161.902f,
    -621.8099f, -1156.388f,


    //-- rollfeld 14L
    1508.464f, -597.9289f,
    1508.239f, -567.4356f,
    1508.333f, -582.5612f,
    1386.12f, -612.996f,
    1385.89f, -553.8044f,
    1404.216f, -612.8679f,
    1403.986f, -553.6761f,
    1420.584f, -612.9055f,
    1420.354f, -553.7139f,
    1436.528f, -612.7157f,
    1436.299f, -553.524f,
    1453.549f, -612.6366f,
    1453.319f, -553.445f,
    1471.194f, -612.3524f,
    1470.965f, -553.1608f,
    1489.181f, -612.2793f,
    1488.951f, -553.0877f,
    1508.56f, -612.1658f,
    1508.33f, -552.9742f,


    //-- rollfeld 14R
    1105.208f, 551.4844f,
    1104.983f, 574.462f,
    1105.076f, 563.0283f,
    982.8634f, 540.3729f,
    982.6339f, 584.2694f,
    1000.96f, 540.501f,
    1000.73f, 584.3977f,
    1017.328f, 540.4634f,
    1017.098f, 584.36f,
    1033.272f, 540.6532f,
    1033.042f, 584.5498f,
    1050.293f, 540.7323f,
    1050.063f, 584.6288f,
    1067.938f, 541.0165f,
    1067.708f, 584.9131f,
    1085.925f, 541.0896f,
    1085.695f, 584.9861f,
    1105.304f, 541.2031f,
    1105.074f, 585.0997f,

    //-- rollfeld 32L
    -783.5748f, 527.6946f,
    -783.8791f, 571.5908f,
    -764.1961f, 527.8412f,
    -764.5004f, 571.7372f,
    -746.2095f, 527.9449f,
    -746.5137f, 571.8411f,
    -728.5647f, 528.2593f,
    -728.869f, 572.1553f,
    -711.5441f, 528.3673f,
    -711.8484f, 572.2635f,
    -695.6003f, 528.5844f,
    -695.9047f, 572.4805f,
    -679.2321f, 528.5745f,
    -679.5364f, 572.4708f,
    -661.1362f, 528.7337f,
    -661.4405f, 572.6298f,
    -783.6147f, 549.7659f,
    -783.5017f, 538.3324f,
    -783.7655f, 561.3096f,

    //-- rollfeld 32R
    -2282.986f, -600.3122f,
    -2282.626f, -630.8042f,
    -2282.787f, -615.6792f,
    -2160.709f, -584.7045f,
    -2160.219f, -643.8945f,
    -2178.805f, -584.9127f,
    -2178.314f, -644.1028f,
    -2195.173f, -584.9473f,
    -2194.682f, -644.1374f,
    -2211.116f, -585.2076f,
    -2210.625f, -644.3977f,
    -2228.136f, -585.3619f,
    -2227.646f, -644.5519f,
    -2245.78f, -585.7241f,
    -2245.289f, -644.9142f,
    -2263.767f, -585.8767f,
    -2263.276f, -645.0667f,
    -2283.145f, -586.0758f,
    -2282.654f, -645.2659f,


};
int[] startbahn_endfeuer_rollfeld =   
{
    //-- rollfeld 06
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,
    ROLLFELD_06,

    //-- rollfeld 24
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,
    ROLLFELD_24,

    //-- rollfeld 14L
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,
    ROLLFELD_14L,

    //-- rollfeld 14R
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,
    ROLLFELD_14R,

    //-- rollfeld 32L
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,
    ROLLFELD_32L,

    //-- rollfeld 32R
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
    ROLLFELD_32R,
};


//--------------------------------------------------------------------------------------------------
float[] startbahn_atr_mast_position =
{
    //-- rollfeld 06
    85.67064f, 1304.687f, 
    132.5196f, 1291.339f,
    109.2026f, 1297.983f,
    111.3717f, 1394.54f,
    158.0715f, 1381.235f,
    134.8338f, 1387.855f,
    137.5973f, 1486.143f,
    184.2466f, 1472.851f,
    161.015f, 1479.47f,

    //-- rollfeld 24
    -602.2731f, -1223.672f,
    -649.0794f, -1210.173f,
    -625.7841f, -1216.892f,
    -628.2611f, -1313.441f,
    -674.9183f, -1299.987f,
    -651.7017f, -1306.683f,
    -654.779f, -1404.961f,
    -701.3853f, -1391.519f,
    -678.1752f, -1398.213f,

    //-- rollfeld 14l
    1638.028f, -614.4376f,
    1638.028f, -551.171f,
    1638.028f, -582.6932f,
    1725.732f, -551.2311f,
    1725.732f, -614.3419f,
    1725.732f, -582.6695f,
    1816.212f, -614.2215f,
    1816.212f, -551.1616f,
    1816.213f, -582.5947f,

    //-- rollfeld 14r
    1121.522f, 588.4362f,
    1121.522f, 538.4372f,
    1121.555f, 562.4672f,
    1185.253f, 589.671f,
    1185.253f, 539.7944f,
    1185.287f, 563.8244f,

    //-- rollfeld 32l
    -850.7546f, 575.7239f,
    -850.7546f, 525.725f,
    -850.7213f, 549.755f,
    -928.2389f, 575.6817f,
    -928.2389f, 525.8051f,
    -928.2056f, 549.8351f,
    -1006.731f, 525.8249f,
    -1006.731f, 575.8238f,
    -1006.82f, 549.9772f,

    //-- rollfeld 32r
    -2390.73f, -591.2704f,
    -2390.73f, -639.984f,
    -2390.73f, -615.5161f,
    -2484.187f, -591.3306f,
    -2484.187f, -639.8883f,
    -2484.187f, -615.4924f,
    -2579.469f, -591.2611f,
    -2579.469f, -639.7679f,
    -2579.469f, -615.4175f,

};

float[] startbahn_atr_mast_rotation =
{
    //-- rollfeld 06
    15.90369f,    
    15.90369f,    
    15.90369f,    
    15.90369f,    
    15.90369f,    
    15.90369f,    
    15.90369f,    
    15.90369f,    
    15.90369f,    

    //-- rollfeld 24
    196.0866f,
    196.0866f,
    196.0866f,
    196.0866f,
    196.0866f,
    196.0866f,
    196.0866f,
    196.0866f,
    196.0866f,

    //-- rollfeld 14l
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,

    //-- rollfeld 14r
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,
    90.0f,

    //-- rollfeld 32l
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,

    //-- rollfeld 32r
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
    270.0f,
};

int[] startbahn_atr_mast_typ =   
{
    //-- rollfeld 06
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER_SIGNAL,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER_SIGNAL,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER_SIGNAL,

    //-- rollfeld 24
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER_SIGNAL,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER_SIGNAL,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER_SIGNAL,

    //-- rollfeld 14l
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER_SIGNAL,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER_SIGNAL,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER_SIGNAL,
    
    //-- rollfeld 14r
    ATR_MAST_TYP_3M_3ER,           
    ATR_MAST_TYP_3M_3ER,           
    ATR_MAST_TYP_3M_3ER_SIGNAL,
    ATR_MAST_TYP_6M_3ER,
    ATR_MAST_TYP_6M_3ER,
    ATR_MAST_TYP_6M_3ER_SIGNAL,
    
    //-- rollfeld 32l
    ATR_MAST_TYP_3M_3ER,           
    ATR_MAST_TYP_3M_3ER,           
    ATR_MAST_TYP_3M_3ER_SIGNAL,
    ATR_MAST_TYP_6M_3ER,
    ATR_MAST_TYP_6M_3ER,
    ATR_MAST_TYP_6M_3ER_SIGNAL,
    ATR_MAST_TYP_9M_3ER,
    ATR_MAST_TYP_9M_3ER,
    ATR_MAST_TYP_9M_3ER_SIGNAL,

    //-- rollfeld 32r
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER,           
    ATR_MAST_TYP_3M_5ER_SIGNAL,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER,
    ATR_MAST_TYP_6M_5ER_SIGNAL,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER,
    ATR_MAST_TYP_9M_5ER_SIGNAL,

};

float[] taxiway_atr_mast_position_rotation_typ = new float[MAX_TAXIWAY_ATR_MAST * 4]
{
    //-- typ: TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT (Group 1)
    -2206.814f, -582.6782f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2156.48f, -582.3874f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2106.025f, -582.0959f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1980.302f, -580.6521f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1929.821f, -580.0582f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1879.45f, -579.561f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1828.945f, -579.1147f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1778.439f, -578.6663f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1728.053f, -578.218f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT, 
    -1677.487f, -577.7713f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -1626.84f, -577.4472f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1576.354f, -577.0421f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1525.788f, -576.6369f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1475.384f, -576.3128f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1374.171f, -575.4816f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1323.728f, -574.8792f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1273.273f, -574.3459f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1222.762f, -573.919f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1172.224f, -573.4513f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1121.687f, -572.8609f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -1071.066f, -572.3931f, 359.5601f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -812.8975f, -570.1125f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -762.2462f, -569.7288f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -711.448f, -569.2277f, 359.5721f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -638.5135f, -568.5772f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -587.4594f, -568.1987f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -536.5331f, -567.8148f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -485.479f, -567.4417f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -76.85931f, -564.2051f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -26.06536f, -563.8731f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    24.7286f, -563.5411f, 359.6824f,  TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    75.52255f, -563.2092f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    126.2058f, -562.8772f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    176.9998f, -562.5452f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    227.9044f, -562.2133f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    473.5811f, -559.4154f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    524.2774f, -559.15f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    575.0622f, -558.8846f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    639.5745f, -558.0843f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    690.2399f, -557.7642f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    740.9053f, -557.444f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    791.5706f, -557.0439f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    842.1559f, -556.6437f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    892.8214f, -556.3236f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    943.4867f, -555.9235f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    994.2321f, -555.5233f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1058.024f, -554.4828f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1108.929f, -554.1627f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1159.595f, -553.8426f, 359.6824f,  TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1210.26f, -553.4424f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    1324.098f, -552.0018f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1374.843f, -551.6816f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1425.428f, -551.4416f, 359.6824f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2257.416f, -646.7438f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2206.814f, -646.3751f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2156.191f, -646.0854f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2105.649f, -645.7148f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2055.209f, -645.243f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -2004.793f, -644.801f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1954.378f, -644.4428f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -1903.978f, -644.0873f, 359.669f,  TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1835.8f, -643.4528f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1785.222f, -643.0068f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1734.822f, -642.5961f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1684.422f, -642.2865f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1634.023f, -641.606f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1583.623f, -641.0267f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1533.134f, -640.6038f, 359.669f,  TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1482.823f, -640.3386f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1432.378f, -639.8712f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -1381.978f, -639.4268f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1331.578f, -639.0375f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1281.134f, -638.6039f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1230.734f, -638.1027f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1180.289f, -637.7579f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1129.845f, -637.3901f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1079.401f, -636.9119f, 359.669f,  TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -1028.988f, -636.3786f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -978.5854f, -636.0064f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -928.2281f, -635.6377f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -877.9077f, -635.2321f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -827.5136f, -634.8265f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -777.1194f, -634.4209f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -726.7622f, -634.0154f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -662.3594f, -633.2042f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -611.9705f, -632.896f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -561.543f, -632.5107f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -511.1025f, -632.2161f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -405.6855f, -631.0032f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -355.2201f, -630.636f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -304.7547f, -630.2687f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -187.9599f, -628.9818f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -137.4726f, -628.6491f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -87.02682f, -628.2748f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -36.62267f, -627.9421f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    13.78149f, -627.4431f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    64.14406f, -627.0272f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    114.6314f, -626.6529f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    165.0771f, -626.2786f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    215.4397f, -625.8627f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    265.8022f, -625.4053f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    316.248f, -624.9894f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    366.6522f, -624.5735f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    417.0979f, -624.116f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    467.529f, -623.7043f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    518.0012f, -623.2926f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    568.4734f, -622.8809f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    618.8223f, -622.3868f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    669.2945f, -621.9751f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    719.6844f, -621.5222f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    786.7475f, -620.5341f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    837.2197f, -620.2048f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    887.6097f, -619.7931f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    937.9996f, -619.3814f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    988.3484f, -618.9697f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1038.779f, -618.5168f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1089.169f, -618.064f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1139.642f, -617.6523f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1204.111f, -616.9113f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1254.501f, -616.4995f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    1304.974f, -616.129f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1355.46f, -615.7981f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1405.878f, -615.4249f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    1456.295f, -615.0518f, 359.669f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    //-- typ: TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH (Group 2)
    -666.9457f, 574.4536f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -603.8382f, 574.8506f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -553.2332f, 575.2476f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -502.7604f, 575.5122f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -452.2876f, 575.9092f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -401.9184f, 576.293f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -351.5012f, 576.6768f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -301.132f, 577.0605f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -250.7148f, 577.4443f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -200.2977f, 577.6842f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,

    -149.8325f, 578.116f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -56.84323f, 578.8548f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -5.862744f, 579.0659f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    45.11774f, 579.5936f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    96.20377f, 579.8046f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    147.0787f, 580.2268f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    203.6533f, 580.5434f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    254.3172f, 580.86f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    305.4032f, 581.3877f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    356.2782f, 581.7043f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,

    406.8365f, 582.0209f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    457.6059f, 582.3375f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    508.3753f, 582.6541f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    559.2502f, 583.0762f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    609.8085f, 583.3928f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    660.4724f, 583.815f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    711.5584f, 584.2372f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    762.7501f, 584.4482f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    813.4139f, 584.7648f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    864.2889f, 585.2925f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,

    928.252f, 585.6091f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    979.127f, 585.9257f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -666.9457f, 525.6068f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -616.4456f, 526.0044f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -565.7068f, 526.402f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -514.9681f, 526.72f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -464.3089f, 526.9586f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -413.8088f, 527.4357f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -363.2291f, 527.7537f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -312.5699f, 528.0718f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,

    -262.0698f, 528.3898f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -211.4106f, 528.8669f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -160.7514f, 528.9464f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -79.15591f, 529.8212f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    -28.65577f, 530.1391f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    22.00342f, 530.3777f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    72.50355f, 530.8548f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    123.1627f, 531.0933f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    173.9015f, 531.4909f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    239.1142f, 531.8884f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,

    289.9325f, 532.286f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    340.5917f, 532.6041f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    391.3304f, 533.0016f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    442.0691f, 533.3197f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    492.8078f, 533.7173f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    628.4146f, 534.5607f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    679.1149f, 534.9355f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    729.6277f, 535.2166f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    780.2343f, 535.5915f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    830.8408f, 536.0601f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,

    881.4473f, 536.3412f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    932.1475f, 536.8098f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,
    982.9388f, 537.0218f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL_EINFACH,


    //-- typ: TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT (Group 3)
    44.36392f, 1153.937f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT, 
    20.47701f, 1072.206f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    6.014016f, 1023.326f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -8.315057f, 974.7142f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -22.51021f, 925.9685f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -36.9732f, 876.821f, 106.3116f,  TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -51.16835f, 827.6736f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -65.63133f, 778.6602f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -80.22824f, 729.5128f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -94.55731f, 680.6332f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -108.8864f, 631.3519f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -144.2403f, 510.5591f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -158.5694f, 461.6796f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -173.1663f, 412.5322f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -187.4954f, 363.6527f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -201.8245f, 314.6393f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -216.1535f, 265.4919f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -230.7504f, 216.4784f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -245.2134f, 167.3311f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -266.1044f, 95.01612f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -280.8352f, 45.73483f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -295.0304f, -3.144709f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -309.2255f, -51.75641f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -323.6885f, -100.7699f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -337.8837f, -149.5155f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -406.5473f, -383.4357f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -421.2f, -433.0594f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -446.2072f, -518.4355f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -486.0623f, -654.8029f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -500.9104f, -703.8405f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -515.3431f, -753.1967f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -529.7758f, -802.7311f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -549.3757f, -869.3709f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -563.6302f, -918.9053f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -578.5974f, -968.4397f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -592.8519f, -1017.261f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -607.1064f, -1066.261f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    35.2769f, 968.6097f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    20.83133f, 919.4216f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    6.230008f, 870.2943f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -7.91503f, 821.471f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -27.38347f, 754.2441f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -41.68061f, 705.2688f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -56.28193f, 656.2935f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -70.57904f, 607.1661f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -99.62961f, 508.7592f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -167.1609f, 277.8756f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -182.0428f, 227.9149f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -196.659f, 177.9543f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -211.0094f, 128.5251f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -224.8244f, 81.74784f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -239.6089f, 32.30455f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -253.4239f, -16.89636f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -271.3592f, -77.731f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -285.9014f, -127.659f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -300.6859f, -176.6176f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -365.156f, -396.6887f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -379.4558f, -445.1625f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -405.7964f, -536.0887f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -441.255f, -655.6349f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    -455.945f, -704.7704f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -470.1284f, -753.6526f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -484.3119f, -802.2816f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -499.0018f, -851.417f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -513.1202f, -900.1693f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -527.6797f, -950.0247f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -542.0187f, -999.2183f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,
    -556.1371f, -1048.412f, 106.3116f, TAXIWAY_ATR_HOCH_SIGNAL_DOPPELT,

    //-- typ: TAXIWAY_ATR_HOCH_SIGNAL (Group 4)
    -372.4518f, -214.5418f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -394.8769f, -231.4016f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -446.04f, -231.5477f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -497.057f, -231.4015f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -547.343f, -231.4015f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -596.752f, -231.4015f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -626.1343f, -232.3863f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -677.1512f, -232.84f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -728.1682f, -233.0668f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -778.7466f, -233.7322f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -829.1789f, -234.2514f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -879.7573f, -234.6093f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -931.251f, -235.0629f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -982.5178f, -235.9703f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1034.012f, -236.6508f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1085.505f, -237.3314f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1164.674f, -238.0119f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1216.168f, -238.2387f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1268.115f, -238.4655f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1319.836f, -238.9192f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -1372.237f, -239.1461f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1423.731f, -239.3729f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1476.132f, -239.8266f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1528.533f, -240.0534f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1580.253f, -240.5071f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1631.747f, -240.9608f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1682.333f, -241.1876f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1733.6f, -241.6413f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1785.321f, -242.0949f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1836.361f, -242.7755f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -1887.628f, -243.456f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1939.348f, -243.6828f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1991.296f, -243.9097f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2043.016f, -243.9096f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2105.398f, -244.1365f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2156.438f, -244.5901f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2207.015f, -247.9619f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2255.485f, -263.3457f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2297.843f, -291.163f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2332.404f, -329.3064f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -2354.952f, -375.0364f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2365.068f, -425.4026f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2360.221f, -477.0332f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2342.94f, -525.0812f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2312.594f, -566.8071f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -487.6763f, -638.5866f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -430.5802f, -638.4647f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -404.2281f, -559.7747f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -465.3503f, -560.9948f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -593.6218f, -265.7231f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -542.1598f, -265.4093f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -491.0115f, -269.4886f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -633.1597f, -266.6699f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -683.6802f, -266.8268f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -734.6716f, -267.1405f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -785.8198f, -267.4543f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -836.8111f, -267.7681f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -887.8024f, -267.925f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -938.7938f, -268.2388f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -989.7851f, -268.5526f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -1040.776f, -268.7095f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1091.768f, -269.1802f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1142.236f, -271.802f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1239.457f, -270.8212f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1290f, -271.1494f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1340.543f, -271.4776f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1390.757f, -271.9699f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1441.464f, -272.2981f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1492.007f, -272.7904f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1542.385f, -273.2827f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -1592.928f, -273.775f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1643.635f, -274.1032f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1694.177f, -274.7596f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1744.556f, -275.0878f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1795.098f, -275.416f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1845.477f, -276.0724f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1896.019f, -276.4007f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1946.562f, -276.7289f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1997.433f, -278.534f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2093.267f, -278.8622f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -2143.974f, -279.0263f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2194.516f, -281.3237f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2242.762f, -295.7644f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2283.623f, -324.4818f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2312.996f, -365.8349f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2329.078f, -413.2597f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2329.078f, -463.1459f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2314.309f, -510.7348f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2283.951f, -550.2825f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2241.285f, -576.046f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -2021.77f, -291.7867f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2073.335f, -294.4372f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2032.733f, -307.8961f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2033.092f, -359.4178f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2033.45f, -410.8438f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2033.41f, -461.6335f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2032.87f, -511.8713f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2017.474f, -569.1317f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2072.844f, -569.4018f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2069.063f, -314.1613f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -2068.792f, -366.2898f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2068.522f, -418.6884f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2067.712f, -481.0805f,  0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -2067.441f, -534.8295f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1441.333f, -570.1564f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1406.283f, -532.0699f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1373.165f, -492.8796f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1340.046f, -454.5172f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1306.651f, -415.8788f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1278.225f, -374.2046f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -1256.974f, -328.1145f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1235.171f, -282.3005f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1165.897f, -278.9886f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1206.192f, -312.3831f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1220.819f, -362.061f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1206.468f, -412.0149f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1168.381f, -447.0653f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1123.671f, -473.8361f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1078.685f, -500.0549f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1032.319f, -524.3419f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -984.2969f, -545.041f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -933.791f, -558.5644f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -882.7332f, -567.12f,  0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1047.774f, -564.36f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1088.896f, -534.8293f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1132.227f, -508.6104f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1176.109f, -483.2194f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1220.267f, -458.3803f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1255.041f, -442.097f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1290.644f, -453.6884f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -1325.418f, -490.3948f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1358.813f, -529.5851f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -1370.128f, -552.2162f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -419.3499f, -277.8097f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -397.2048f, -287.0571f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -418.2548f, -292.4108f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -403.0452f, -321.0049f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -392.0943f, -330.0089f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -391.4859f, -314.9211f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -405.1137f, -369.1888f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -424.8253f, -350.5723f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -434.0727f, -334.1459f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -443.661f, -312.3658f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -454.6282f, -292.1033f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -466.5316f, -275.7194f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -330.9091f, -293.9069f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -326.8325f, -314.4463f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -311.9375f, -327.6165f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -314.1326f, -275.8762f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -293.2797f, -264.117f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -301.403f, -200.7026f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -287.7768f, -219.3076f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -246.8981f, -224.5485f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -198.4202f, -218.5215f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -164.8788f, -201.4887f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -248.9945f, -264.1168f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -227.507f, -264.1168f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -203.9232f, -266.4752f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -165.927f, -277.743f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -138.4125f, -286.1283f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -102.5127f, -296.61f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -75.78434f, -301.5888f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -27.83054f, -301.8508f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    10.42769f, -314.6909f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -34.90569f, -332.7718f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -81.54929f, -335.1302f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -116.401f, -335.9163f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -166.1891f, -338.0126f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -214.929f, -335.6542f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -265.5032f, -331.9856f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -351.14f, -385.1153f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -299.0589f, -379.8722f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -245.2301f, -379.8722f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -192.1004f, -379.5226f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -138.6211f, -378.8235f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -85.49144f, -378.474f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -45.18655f, -382.7166f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -72.76357f, -407.907f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -106.7045f, -436.8098f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -137.9938f, -461.7351f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -172.4651f, -489.5772f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -211.179f, -512.1161f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -257.3175f, -515.5633f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -299.2134f, -504.6916f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -349.0641f, -490.6378f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -375.0502f, -472.3415f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -391.7555f, -518.2148f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -358.61f, -522.4575f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -318.04f, -533.8595f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -298.683f, -545.2615f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -322.8129f, -553.2165f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -357.5285f, -559.6115f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    125.0091f, -316.6112f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    646.0027f, -314.7109f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    746.4011f, -324.8458f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    599.7625f, -376.7869f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -125.1847f, -553.993f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -162.0384f, -539.1416f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -139.2111f, -509.4387f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -106.2078f, -479.7358f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    -68.52909f, -451.408f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -31.4004f, -425.5555f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    -3.072583f, -405.7535f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    27.45546f, -390.352f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    457.8336f, -554.7924f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    295.5804f, -557.139f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    1303.164f, -545.9882f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    1297.871f, -532.1954f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    1224.257f, -544.2239f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    1451.47f, -546.6494f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    1517.333f, -546.4449f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    1340.064f, -305.5275f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,
    1313.173f, -377.1494f, 0.0f, TAXIWAY_ATR_HOCH_SIGNAL,

    //-- typ: TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT (Group 5)
    -224.8516f, -641.6274f, -59.58411f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -243.1884f, -688.4346f, -71.97513f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -258.1762f, -736.4197f, -72.50723f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -273.1868f, -784.6289f, -72.5072f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -287.7591f, -832.7285f, -73.03479f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -302.3314f, -880.7186f, -73.03482f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -316.9037f, -928.8182f, -73.03482f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -331.476f, -976.8083f, -73.03482f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -345.9388f, -1024.798f, -73.03482f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -360.4016f, -1072.898f, -73.03482f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    -374.3165f, -1120.997f, -73.86588f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -388.2314f, -1169.207f, -73.86584f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -258.466f, -640.9273f, -121.2357f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -259.4348f, -664.972f, -72.62701f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -274.3713f, -712.7687f, -72.62701f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -289.3079f, -760.7149f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -304.3937f, -808.6611f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -319.0315f, -856.6072f, -72.62704f,  TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -333.6693f, -904.8521f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -348.4565f, -952.9476f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    -363.393f, -1000.744f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -378.1801f, -1048.541f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -392.6685f, -1096.488f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    -407.7544f, -1144.434f, -72.62704f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    195.8226f, -299.8712f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    147.4135f, -304.9796f, -8.655884f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    246.786f, -299.263f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    297.6277f, -298.7765f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    348.5911f, -298.1683f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    415.0016f, -297.4385f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    458.7888f, -296.952f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    510.3603f, -296.7087f, -0.8537598f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    561.4453f, -296.2222f, 1.945129f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    613.0167f, -298.7764f, 11.3934f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    633.8156f, -325.9001f, -188.9896f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    585.1632f, -331.9816f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    534.3215f, -332.1032f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    483.2365f, -332.3465f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    432.1514f, -332.5898f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    381.0664f, -333.0763f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    329.4949f, -333.3195f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    278.1666f, -333.5628f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    226.7167f, -333.6844f, 179.8039f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    175.2668f, -333.806f, 185.7426f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    72.22345f, -378.6419f, -5.052856f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    124.0655f, -376.4728f, -0.7417603f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    176.0946f, -376.0771f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    227.5303f, -375.4837f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    278.9659f, -375.088f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    330.4016f, -374.6924f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    381.2437f, -374.2968f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    432.6794f, -374.0989f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    505.8763f, -373.7033f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    557.7075f, -373.5055f, -0.7416992f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    592.5255f, -387.7492f, 141.7754f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    551.9704f, -420.1932f, 141.2207f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    511.4153f, -452.8351f, 141.2207f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    471.6516f, -484.2899f, 147.729f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    427.9313f, -511.5904f, 153.8807f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    381.837f, -534.1428f, 162.5608f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    466.7058f, -543.6383f, 321.4507f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    507.063f, -511.3924f, -39.38745f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    546.431f, -479.3441f, -39.38745f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    587.9752f, -446.9f, -38.15906f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    628.9259f, -416.2365f, -33.23532f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    672.844f, -390.9143f, -20.6188f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    722.697f, -377.0663f, -6.093567f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    774.5283f, -373.5053f, -0.7591095f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    826.5574f, -373.1097f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    878.3887f, -372.5162f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    929.8243f, -371.9227f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    980.8643f, -371.3293f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1031.311f, -370.9336f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1082.944f, -370.5379f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1134.578f, -370.3401f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1186.013f, -369.9445f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1237.647f, -369.5488f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1289.478f, -369.1531f, -0.7590942f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1310.988f, -391.6265f, 118.0694f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1286.267f, -436.5732f, 118.0694f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    1261.432f, -482.2379f, 119.3205f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1235.796f, -527.6354f, 119.3205f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1298.551f, -508.9424f, 275.3021f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1317.511f, -460.6073f, -61.59424f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1343.148f, -414.4086f, -60.32721f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1369.585f, -369.2781f, -56.48169f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1398.693f, -325.7499f, -56.48169f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1432.608f, -297.1761f, 0.5312042f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1480.676f, -306.5227f, 32.41647f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1517.795f, -342.5737f, 69.15215f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    1535.687f, -391.1758f, 91.9541f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1532.749f, -442.4484f, 111.044f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1513.255f, -490.5164f, 128.4623f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1479.607f, -529.5049f, 145.0961f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1531.147f, -530.039f, 307.6725f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1558.119f, -486.2437f, -65.07626f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1584.289f, -440.045f, -33.68134f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1329.119f, -317.521f, -187.2351f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1277.447f, -323.7329f, 178.493f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1225.142f, -324.7251f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    1173.206f, -325.1525f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1121.484f, -325.3662f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1018.04f, -326.2211f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    966.1042f, -326.8622f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    914.1685f, -327.7171f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    861.8052f, -328.572f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    810.0832f, -328.9994f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    758.5749f, -329.4268f, 179.3776f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    754.0866f, -315.1071f, 328.1844f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    803.2438f, -296.5128f, -3.999207f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    855.1796f, -294.5892f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    907.329f, -293.948f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    959.6921f, -293.0931f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1012.055f, -292.2382f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1064.418f, -291.597f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1116.354f, -291.5969f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1168.29f, -291.1695f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1220.653f, -290.5283f, -1.019165f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1272.161f, -290.9557f, 4.179886f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,
    1324.097f, -295.0165f, 10.8049f, TAXIWAY_ATR_MITTEL_SIGNAL_DOPPELT,

    //-- typ: TAXIWAY_ATR_NIEDRIG_SIGNAL (Group 6)
    -603.4008f, -223.0662f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -601.8434f, -171.4521f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,      
    -599.8412f, -120.0605f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -597.6165f, -68.44642f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -595.8367f, -16.38739f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -593.6119f, 35.44915f, 0.0f,  TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -591.8322f, 87.06322f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -588.94f, 149.801f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -588.4279f, 209.2081f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -598.4144f, 250.6907f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    -612.882f, 291.277f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -636.7955f, 336.218f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -660.7091f, 381.2964f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -684.8975f, 426.2375f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -708.3987f, 471.7282f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -672.8033f, 522.4414f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -684.0729f, 512.821f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -684.0729f, 501.2765f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -751.8279f, 520.3799f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -748.8044f, 501.6888f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    -746.1931f, 481.486f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -723.6539f, 467.1929f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -717.332f, 460.0464f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -693.9682f, 415.5177f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -670.3295f, 371.1265f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -653.5626f, 339.1043f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -630.8853f, 291.9594f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -619.5995f, 271.5902f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -615.9534f, 267.4231f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -611.4738f, 247.838f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    -600.7313f, 197.4462f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -601.4024f, 155.341f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -604.0864f, 119.4425f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -605.8643f, 68.9999f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -607.8773f, 18.6749f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -609.8903f, -31.65011f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -612.071f, -81.97511f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -614.0841f, -132.1323f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -615.4261f, -164.0048f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -616.6004f, -185.3091f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    -618.7812f, -225.4013f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -645.3567f, 591.605f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -630.0989f, 591.7608f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -635.2637f, 642.6431f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -640.4284f, 693.9079f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -650.184f, 643.0258f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -654.7749f, 694.2906f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -658.6193f, 712.3043f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -639.7269f, 723.398f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -622.7052f, 583.2188f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    -652.3145f, 582.6466f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    923.1144f, 592.9966f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    901.0811f, 594.5114f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    907.0024f, 602.223f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    924.2159f, 602.2229f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    940.6032f, 650.4207f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    923.3897f, 650.283f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    932.203f, 686.087f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    959.3315f, 692.9724f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    981.2509f, 684.0053f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    1003.502f, 667.3997f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    1020.772f, 646.4765f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    1034.037f, 626.7687f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    1032.521f, 604.408f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    1015.466f, 592.6591f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    619.167f, 527.1013f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    607.6738f, 512.8049f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    606.8329f, 486.735f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    607.3936f, 467.6732f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    608.2346f, 440.7624f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    610.7575f, 409.6467f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    568.1487f, 408.2451f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    570.6716f, 442.7246f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    569.8306f, 474.1206f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    555.8146f, 503.8347f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    529.1841f, 524.2982f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    560.9717f, 401.4198f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    538.7721f, 398.9947f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    487.6833f, 398.8531f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    436.4531f, 398.7116f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    385.0813f, 398.4286f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    334.1341f, 398.1456f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    283.0224f, 397.8875f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    232.0972f, 397.6626f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    178.5863f, 396.4261f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    127.7735f, 395.6392f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    88.42722f, 411.0404f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    85.72919f, 422.507f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    47.61956f, 422.3947f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    14.90599f, 422.3947f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    7.486414f, 410.366f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -34.10817f, 402.1595f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -83.45957f, 406.7686f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -106.5052f, 425.0927f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -107.9614f, 470.417f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -156.8654f, 299.4813f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -125.4665f, 325.354f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -87.28537f, 341.4303f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -46.84351f, 339.9231f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -13.43502f, 326.1076f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    -9.164762f, 290.4384f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -22.75987f, 239.8412f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -37.38047f, 189.8482f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -51.76526f, 140.5626f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -65.91423f, 91.27707f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -85.48697f, 22.41875f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -100.3434f, -27.10264f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -117.7938f, -87.23576f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    -144.441f, -175.6668f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    616.5322f, 403.6771f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    627.8058f, 399.9193f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    399.9193f, 400.1016f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    730.6491f, 400.4663f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    782.4355f, 400.6487f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    834.4042f, 400.831f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    886.1905f, 401.0133f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    938.3416f, 402.1074f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    974.4462f, 430.0064f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    1004.533f, 472.6754f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,
    1016.476f, 509.11f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    1002.492f, 527.1236f, 0.0f, TAXIWAY_ATR_NIEDRIG_SIGNAL,

    //-- typ: TAXIWAY_ATR_MITTEL_EINFACH (Group 7)
    -711.9126f, 653.8655f, 303.3614f, TAXIWAY_ATR_MITTEL_EINFACH,
    -693.8716f, 680.828f, 313.1144f, TAXIWAY_ATR_MITTEL_EINFACH,
    -669.4993f, 706.8365f, 317.5069f, TAXIWAY_ATR_MITTEL_EINFACH,
    -632.1418f, 732.6967f, 324.5973f, TAXIWAY_ATR_MITTEL_EINFACH,
    -590.5639f, 762.0231f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -548.5435f, 790.8655f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -506.5231f, 819.7078f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -464.627f, 848.3015f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -422.358f, 877.3925f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -380.4619f, 905.9861f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,

    -338.5658f, 934.7042f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -296.5454f, 963.7952f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -254.6494f, 993.0104f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -212.5047f, 1022.101f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -170.7329f, 1051.317f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -129.3341f, 1080.408f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -87.68665f, 1109.623f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    -31.58784f, 1151.306f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    10.87804f, 1180.922f, 325.3638f, TAXIWAY_ATR_MITTEL_EINFACH,
    40.80784f, 1180.452f, 234.7872f, TAXIWAY_ATR_MITTEL_EINFACH,

    -22.34251f, 1174.027f, 144.963f, TAXIWAY_ATR_MITTEL_EINFACH,
    -57.75685f, 1149.112f, 144.963f, TAXIWAY_ATR_MITTEL_EINFACH,
    -103.2f, 1117.458f, 144.963f, TAXIWAY_ATR_MITTEL_EINFACH,
    -145.9793f, 1087.999f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -187.9751f, 1058.696f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -230.5977f, 1029.236f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -273.0635f, 999.6197f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -315.5294f, 970.4734f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -357.9953f, 941.1704f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -397.1705f, 914.3746f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,

    -438.8529f, 885.0717f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -480.692f, 856.0821f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -522.5311f, 826.6224f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -564.8403f, 797.0061f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -607.3062f, 767.5464f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -636.3116f, 747.4565f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -666.2681f, 728.1987f, 145.2164f, TAXIWAY_ATR_MITTEL_EINFACH,
    -697.0568f, 699.7874f, 129.2169f, TAXIWAY_ATR_MITTEL_EINFACH,
    -725.5868f, 661.034f, 117.4939f, TAXIWAY_ATR_MITTEL_EINFACH,
    -746.7479f, 624.3142f, 125.7801f, TAXIWAY_ATR_MITTEL_EINFACH,


    //-- typ: TAXIWAY_ATR_MITTEL_SIGNAL (Group 8)
    1622.892f, -419.2212f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1674.186f, -418.258f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1724.866f, -417.9814f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1775.407f, -417.6357f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1826.018f, -417.2209f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1876.905f, -417.1517f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1926.962f, -416.322f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1977.503f, -416.0454f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2015.323f, -414.87f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2064.758f, -404.499f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,

    2113.779f, -393.9897f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2163.075f, -383.4113f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2212.718f, -372.902f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2273.423f, -360.2493f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2294.448f, -314.0768f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2315.43f, -267.8785f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2336.412f, -221.6803f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2357.54f, -175.0723f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2378.798f, -128.3734f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2375.963f, -90.57163f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,

    2363.992f, -40.5893f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2349.712f, -29.35377f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2298.939f, -40.06099f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    2248.511f, -50.94091f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1728.436f, 143.6273f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1690.926f, 177.9336f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1677.043f, 189.4135f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1662.894f, 191.5493f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1624.449f, 225.7221f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1586.005f, 259.628f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,

    1567.317f, 275.9135f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1519.128f, 258.6936f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1471.2f, 241.3422f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1423.377f, 223.9907f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1375.978f, 206.745f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1327.838f, 189.4994f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1253.674f, 195.0284f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1260.061f, 190.1667f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1229.365f, 240.1182f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1208.634f, 284.064f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,

    1207.02f, 298.9609f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1183.061f, 323.4167f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1154.286f, 368.1255f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1129.908f, 413.6138f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1117.845f, 470.1599f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1115.583f, 526.4548f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1105.028f, 607.6299f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,
    1075.875f, 644.5734f, 0.0f, TAXIWAY_ATR_MITTEL_SIGNAL,

    //-- typ: TAXIWAY_ATR_NIEDRIG_DOPPELT (Group 9)
    1357.925f, -65.88196f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1332.549f, -21.02714f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1307.352f, 23.64897f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1281.618f, 68.6825f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1256.242f, 113.716f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1230.866f, 158.5708f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1205.133f, 203.4257f, -119.6085f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1179.042f, 248.1018f, -148.6431f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    1354.456f, -78.67329f, -46.54935f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    922.4822f, 696.3495f, -151.9734f, TAXIWAY_ATR_NIEDRIG_DOPPELT,

    876.9477f, 720.3777f, -152.4952f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    831.2648f, 744.2574f, -152.4952f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    785.4335f, 767.9889f, -152.4952f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    740.0473f, 791.572f, -152.4952f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    694.9577f, 815.7485f, -151.5457f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    651.7963f, 839.4799f, -151.5457f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    618.869f, 871.6656f, -151.5457f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    573.7794f, 895.9904f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    528.8381f, 919.2768f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    483.3035f, 942.8599f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,

    437.7689f, 966.5912f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    392.3826f, 990.026f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    346.848f, 1013.757f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    300.5718f, 1037.341f, -152.5878f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    253.2574f, 1060.627f, -158.7369f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    213.2107f, 1072.493f, -172.2257f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    171.3841f, 1071.603f, -188.0583f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    134.7488f, 1062.11f, 157.8462f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    97.52015f, 1039.862f, 144.657f, TAXIWAY_ATR_NIEDRIG_DOPPELT,
    70.82235f, 1014.351f, 131.1907f, TAXIWAY_ATR_NIEDRIG_DOPPELT,

    48.57418f, 983.9453f, 117.6241f, TAXIWAY_ATR_NIEDRIG_DOPPELT,

    //-- typ: TAXIWAY_ATR_MITTEL_SIGNAL (Group 10)
    -30.60981f, 1134.271f, 16.49866f, TAXIWAY_ATR_MITTEL_SIGNAL,
    10.88926f, 1122.087f, 16.49866f, TAXIWAY_ATR_MITTEL_SIGNAL,
    6.425696f, 1106.283f, 16.49866f, TAXIWAY_ATR_MITTEL_SIGNAL,
    -38.57184f, 1120.518f, 16.49866f, TAXIWAY_ATR_MITTEL_SIGNAL,
    -56.06418f, 1120.277f, -6.793518f, TAXIWAY_ATR_MITTEL_SIGNAL,
    25.84823f, 1127.756f, -27.71857f, TAXIWAY_ATR_MITTEL_SIGNAL,
    14.50836f, 1094.702f, -120.4359f, TAXIWAY_ATR_MITTEL_SIGNAL,
    -33.38446f, 1140.302f, -100.0329f, TAXIWAY_ATR_MITTEL_SIGNAL,

    //-- typ: TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT (Group 11)
    -185.252f, 1136.418f, 0.6994934f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    -133.8716f, 1135.694f, 1.498627f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    -219.7469f, 1151.012f, 1.498627f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    -168.3664f, 1149.565f, 1.498627f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    -116.8653f, 1148.117f, 1.498627f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    -65.84669f, 1146.791f, 1.498627f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    -103.8393f, 1128.458f, 55.09158f,  TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    492.2553f, 993.9516f, 27.84682f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    537.4747f, 970.1198f, 27.12436f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    582.694f, 947.2656f, 27.12436f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,

    626.8135f, 924.2892f, 27.12436f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    690.2428f, 890.5579f, 27.12436f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    735.2177f, 867.0927f, 26.34628f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    762.4715f, 854.3822f, 10.37959f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    819.3569f, 822.9851f, 25.50143f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,
    845.4138f, 810.7573f, 25.50143f, TAXIWAY_ATR_NIEDRIG_SIGNAL_DOPPELT,

    //-- typ: TAXIWAY_ATR_MITTEL_DOPPELT (Group 12)
    -648.7731f, -558.9272f, -74.31921f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -666.1913f, -562.8313f, -74.31918f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -652.0765f, -513.2793f, -74.31918f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -638.1119f, -463.8775f, -74.31918f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -634.6582f, -509.6755f, -74.31918f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -617.3901f, -460.7242f, -46.43814f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -597.7194f, -447.3602f, -16.78085f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -576.5472f, -444.5072f, 15.89102f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -527.2955f, -458.7722f, 16.63223f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -478.0438f, -473.4877f, 18.25172f, TAXIWAY_ATR_MITTEL_DOPPELT,

    -457.172f, -487.9029f, 60.53391f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -444.5587f, -456.6702f, -27.87158f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -458.6736f, -462.0759f, -163.8064f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -507.7751f, -448.1112f, -163.8064f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -556.8766f, -433.6961f, -163.8064f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -585.4066f, -422.2842f, -125.5406f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -604.0261f, -406.9681f, -108.2892f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -608.3807f, -364.924f, -89.28973f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -626.5498f, -414.0255f, -86.91327f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -623.5467f, -363.1222f, -88.59012f, TAXIWAY_ATR_MITTEL_DOPPELT,

    -622.0452f, -311.1677f, -88.59012f, TAXIWAY_ATR_MITTEL_DOPPELT,
    -607.3298f, -313.1198f, -87.94241f, TAXIWAY_ATR_MITTEL_DOPPELT,

};
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//
// Awake
//
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
void Awake() 
{

    //----------------------------------------------------------------------------------------------------   
    anflug_gleitwinkel_feuer_aus      = transform.Find("anflug_gleitwinkel_feuer/aus").gameObject;
    anflug_gleitwinkel_feuer_an_rot   = transform.Find("anflug_gleitwinkel_feuer/an_rot").gameObject; 
    anflug_gleitwinkel_feuer_an_weiss = transform.Find("anflug_gleitwinkel_feuer/an_weiss").gameObject;
    //----------------------------------------------------------------------------------------------------   
    startbahn_endfeuer_gruen          = transform.Find("startbahn_endfeuer/endfeuer_gruen").gameObject;
    startbahn_endfeuer_rot            = transform.Find("startbahn_endfeuer/endfeuer_rot").gameObject;
    //----------------------------------------------------------------------------------------------------   
    unterflurfeuer_aus                = transform.Find("unterflurfeuer/unterflurfeuer_aus").gameObject;
    unterflurfeuer_rot_an             = transform.Find("unterflurfeuer/unterflurfeuer_rot").gameObject;
    unterflurfeuer_weiss_an           = transform.Find("unterflurfeuer/unterflurfeuer_weiss").gameObject;
    //----------------------------------------------------------------------------------------------------   
    anflugfeuersystem_atr_mast_kaefig = transform.Find("anflugfeuersystem_atr_mast_kaefig").gameObject;
    //----------------------------------------------------------------------------------------------------   
    anflugfeuersystem_atr_mast_3m_3er = transform.Find("anflugfeuersystem_mast_atr_3m_3er").gameObject;
    anflugfeuersystem_atr_mast_3m_5er = transform.Find("anflugfeuersystem_mast_atr_3m_5er").gameObject;
    anflugfeuersystem_atr_mast_6m_3er = transform.Find("anflugfeuersystem_mast_atr_6m_3er").gameObject;
    anflugfeuersystem_atr_mast_6m_5er = transform.Find("anflugfeuersystem_mast_atr_6m_5er").gameObject;
    anflugfeuersystem_atr_mast_9m_3er = transform.Find("anflugfeuersystem_mast_atr_9m_3er").gameObject;
    anflugfeuersystem_atr_mast_9m_5er = transform.Find("anflugfeuersystem_mast_atr_9m_5er").gameObject;
    //----------------------------------------------------------------------------------------------------   
    taxiway_atr_hoch_signal_blau      = transform.Find("taxiway_atr_hoch_signal").gameObject;
    taxiway_atr_mittel_signal_blau    = transform.Find("taxiway_atr_mittel_signal").gameObject;
    taxiway_atr_niedrig_signal_blau   = transform.Find("taxiway_atr_niedrig_signal").gameObject;
    //----------------------------------------------------------------------------------------------------   
    airportLightsGroup = transform;
    //----------------------------------------------------------------------------------------------------   

    gleitwinkelfeuer_obj = new GameObject[MAX_ANFLUG_GLEITWINKEL_FEUER];
    gleitwinkelfeuer_last_frame_status = new int[MAX_ANFLUG_GLEITWINKEL_FEUER];

    startbahn_endfeuer_obj = new GameObject[MAX_STARTBAHN_ENDFEUER];
    startbahn_endfeuer_last_frame_status = new int[MAX_STARTBAHN_ENDFEUER];

    unterflurfeuer_obj = new GameObject[MAX_UNTERFLUR_FEUER];
    unterflurfeuer_last_frame_status = new int[MAX_UNTERFLUR_FEUER];

    unterflurfeuer_xpos = new float[MAX_UNTERFLUR_FEUER];
    unterflurfeuer_zpos = new float[MAX_UNTERFLUR_FEUER];
    unterflurfeuer_yrot = new float[MAX_UNTERFLUR_FEUER];
    unterflurfeuer_farbe = new int[MAX_UNTERFLUR_FEUER];
    unterflurfeuer_lauflicht_id  = new int[MAX_UNTERFLUR_FEUER];
    unterflurfeuer_rollfeld = new int[MAX_UNTERFLUR_FEUER];

    anflugfeuersystem_atr_mast_kaefig_obj = new GameObject[MAX_ATR_MAST_KAEFIG];
    anflugfeuersystem_atr_mast_kaefig_xpos = new float[MAX_ATR_MAST_KAEFIG];
    anflugfeuersystem_atr_mast_kaefig_zpos = new float[MAX_ATR_MAST_KAEFIG];
    anflugfeuersystem_atr_mast_kaefig_yrot = new float[MAX_ATR_MAST_KAEFIG];
    anflugfeuersystem_atr_mast_kaefig_status = new bool[MAX_ATR_MAST_KAEFIG];

    anflugfeuersystem_atr_mast_obj    = new GameObject[MAX_ATR_MAST];
    anflugfeuersystem_atr_mast_status = new int[MAX_ATR_MAST];

    taxiway_atr_mast_obj    = new GameObject[MAX_TAXIWAY_ATR_MAST];
    taxiway_atr_mast_status = new int[MAX_TAXIWAY_ATR_MAST];

}
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//
// Init 
//
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
void Start() 
{
    int i;
    //--------------------------------------------------
    for(i=0;i<MAX_ANFLUG_GLEITWINKEL_FEUER;i++)
    {
        gleitwinkelfeuer_obj[i] = null;
        gleitwinkelfeuer_last_frame_status[i] = -1;
    }
    //--------------------------------------------------
    for(i=0;i<MAX_STARTBAHN_ENDFEUER;i++)
    {
        startbahn_endfeuer_obj[i] = null;
        startbahn_endfeuer_last_frame_status[i] = -1;
    }
    //--------------------------------------------------
    for(i=0;i<MAX_UNTERFLUR_FEUER;i++)
    {
        unterflurfeuer_obj[i] = null;
        unterflurfeuer_last_frame_status[i] = -1;
    }
    //--------------------------------------------------
    for(i=0;i<MAX_ATR_MAST_KAEFIG;i++)
    {
        anflugfeuersystem_atr_mast_kaefig_obj[i] = null;
        anflugfeuersystem_atr_mast_kaefig_status[i] = false;
    }
    //--------------------------------------------------
    for(i=0;i<MAX_ATR_MAST;i++)
    {
        anflugfeuersystem_atr_mast_obj[i] = null;
        anflugfeuersystem_atr_mast_status[i] = -1;
    }
    //--------------------------------------------------
    for(i=0;i<MAX_TAXIWAY_ATR_MAST;i++)
    {
        taxiway_atr_mast_obj[i] = null;
        taxiway_atr_mast_status[i] = -1;
    }
    //--------------------------------------------------
    //--------------------------------------------------
    //--------------------------------------------------
    //
    // alle weissen unterflurfeuer anlegen
    //
    float rollfeld_06_xstart = 69.12529f;
    float rollfeld_06_zstart = 1162.703f;
    float rollfeld_06_xend = -102.3832f;
    float rollfeld_06_zend = 578.1174f;

    float rollfeld_24_xstart = -596.4734f;
    float rollfeld_24_zstart = -1107.496f;
    float rollfeld_24_xend = -460.3291f;
    float rollfeld_24_zend = -642.2178f;

    float rollfeld_14l_xstart = 1427.095f;
    float rollfeld_14l_zstart = -582.9001f;
    float rollfeld_14l_xend = 912.5676f;
    float rollfeld_14l_zend = -587.1082f;

    float rollfeld_14r_xstart = 1039.82f;
    float rollfeld_14r_zstart = 562.3685f;
    float rollfeld_14r_xend = 422.2458f;
    float rollfeld_14r_zend = 557.9722f;

    float rollfeld_32l_xstart = -729.3429f;
    float rollfeld_32l_zstart = 549.9794f;
    float rollfeld_32l_xend = -115.5343f;
    float rollfeld_32l_zend = 554.2927f;

    float rollfeld_32r_xstart = -2194.083f;
    float rollfeld_32r_zstart = -614.482f;
    float rollfeld_32r_xend = -1574.34f;
    float rollfeld_32r_zend = -609.0208f;
    
    float rollfeld_06_xadd = (rollfeld_06_xend - rollfeld_06_xstart) / 32.0f;
    float rollfeld_06_zadd = (rollfeld_06_zend - rollfeld_06_zstart) / 32.0f;

    float rollfeld_24_xadd = (rollfeld_24_xend - rollfeld_24_xstart) / 32.0f;
    float rollfeld_24_zadd = (rollfeld_24_zend - rollfeld_24_zstart) / 32.0f;

    float rollfeld_14l_xadd = (rollfeld_14l_xend - rollfeld_14l_xstart) / 32.0f;
    float rollfeld_14l_zadd = (rollfeld_14l_zend - rollfeld_14l_zstart) / 32.0f;

    float rollfeld_14r_xadd = (rollfeld_14r_xend - rollfeld_14r_xstart) / 32.0f;
    float rollfeld_14r_zadd = (rollfeld_14r_zend - rollfeld_14r_zstart) / 32.0f;

    float rollfeld_32l_xadd = (rollfeld_32l_xend - rollfeld_32l_xstart) / 32.0f;
    float rollfeld_32l_zadd = (rollfeld_32l_zend - rollfeld_32l_zstart) / 32.0f;

    float rollfeld_32r_xadd = (rollfeld_32r_xend - rollfeld_32r_xstart) / 32.0f;
    float rollfeld_32r_zadd = (rollfeld_32r_zend - rollfeld_32r_zstart) / 32.0f;

    for(i=0;i<32;i++)
    {
        //-- rollfeld 06
        unterflurfeuer_xpos         [i + 0 * 32] = 70.70829f + (i * rollfeld_06_xadd);
        unterflurfeuer_zpos         [i + 0 * 32] = 1167.452f + (i * rollfeld_06_zadd);
        unterflurfeuer_farbe        [i + 0 * 32] = FARBE_WEISS;
        unterflurfeuer_lauflicht_id [i + 0 * 32] = i & 15;
        unterflurfeuer_rollfeld     [i + 0 * 32] = ROLLFELD_06;
        unterflurfeuer_yrot         [i + 0 * 32] = 16.39943f;

        //-- rollfeld 24
        unterflurfeuer_xpos         [i + 1 * 32] = -598.508f + (i * rollfeld_24_xadd);
        unterflurfeuer_zpos         [i + 1 * 32] = -1114.83f + (i * rollfeld_24_zadd);
        unterflurfeuer_farbe        [i + 1 * 32] = FARBE_WEISS;
        unterflurfeuer_lauflicht_id [i + 1 * 32] = i & 15;
        unterflurfeuer_rollfeld     [i + 1 * 32] = ROLLFELD_24;
        unterflurfeuer_yrot         [i + 1 * 32] = -163.6224f;
    
        //-- rollfeld 14l
        unterflurfeuer_xpos         [i + 2 * 32] = 1434.876f + (i * rollfeld_14l_xadd);
        unterflurfeuer_zpos         [i + 2 * 32] = -582.9001f + (i * rollfeld_14l_zadd);
        unterflurfeuer_farbe        [i + 2 * 32] = FARBE_WEISS;
        unterflurfeuer_lauflicht_id [i + 2 * 32] = i & 15;
        unterflurfeuer_rollfeld     [i + 2 * 32] = ROLLFELD_14L;
        unterflurfeuer_yrot         [i + 2 * 32] = 89.57336f;    

        //-- rollfeld 14r
        unterflurfeuer_xpos         [i + 3 * 32] = 1049.35f + (i * rollfeld_14r_xadd);
        unterflurfeuer_zpos         [i + 3 * 32] = 562.3685f + (i * rollfeld_14r_zadd);
        unterflurfeuer_farbe        [i + 3 * 32] = FARBE_WEISS;
        unterflurfeuer_lauflicht_id [i + 3 * 32] = i & 15;
        unterflurfeuer_rollfeld     [i + 3 * 32] = ROLLFELD_14R;
        unterflurfeuer_yrot         [i + 3 * 32] = 90.50058f;    

        //-- rollfeld 32l
        unterflurfeuer_xpos         [i + 4 * 32] = -723.633f + (i * rollfeld_32l_xadd);
        unterflurfeuer_zpos         [i + 4 * 32] = 550.1202f + (i * rollfeld_32l_zadd);
        unterflurfeuer_farbe        [i + 4 * 32] = FARBE_WEISS;
        unterflurfeuer_lauflicht_id [i + 4 * 32] = i & 15;
        unterflurfeuer_rollfeld     [i + 4 * 32] = ROLLFELD_32L;
        unterflurfeuer_yrot         [i + 4 * 32] = -89.57336f;    

        //-- rollfeld 32r
        unterflurfeuer_xpos         [i + 5 * 32] = -2204.443f + (i * rollfeld_32r_xadd);
        unterflurfeuer_zpos         [i + 5 * 32] = -614.482f + (i * rollfeld_32r_zadd);
        unterflurfeuer_farbe        [i + 5 * 32] = FARBE_WEISS;
        unterflurfeuer_lauflicht_id [i + 5 * 32] = i & 15;
        unterflurfeuer_rollfeld     [i + 5 * 32] = ROLLFELD_32R;
        unterflurfeuer_yrot         [i + 5 * 32] = -90.50058f;    

    }

    //-- rote lauflichter
    for(i=0;i<16;i++)
    {
        //-- rollfeld 06 links
        unterflurfeuer_xpos         [(6*32) + i + 0 * 16] = 57.51666f + (i * rollfeld_06_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 0 * 16] = 1171.277f + (i * rollfeld_06_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 0 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 0 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 0 * 16] = ROLLFELD_06;
        unterflurfeuer_yrot         [(6*32) + i + 0 * 16] = 16.39943f;

        //-- rollfeld 24 links
        unterflurfeuer_xpos         [(6*32) + i + 1 * 16] = -612.0406f + (i * rollfeld_24_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 1 * 16] = -1111.045f + (i * rollfeld_24_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 1 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 1 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 1 * 16] = ROLLFELD_24;
        unterflurfeuer_yrot         [(6*32) + i + 1 * 16] = -163.6224f;
    
        //-- rollfeld 14l links
        unterflurfeuer_xpos         [(6*32) + i + 2 * 16] = 1434.876f + (i * rollfeld_14l_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 2 * 16] = -561.6693f + (i * rollfeld_14l_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 2 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 2 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 2 * 16] = ROLLFELD_14L;
        unterflurfeuer_yrot         [(6*32) + i + 2 * 16] = 89.57336f;
    
        //-- rollfeld 14r links
        unterflurfeuer_xpos         [(6*32) + i + 3 * 16] = 1049.35f + (i * rollfeld_14r_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 3 * 16] = 578.3376f + (i * rollfeld_14r_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 3 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 3 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 3 * 16] = ROLLFELD_14R;
        unterflurfeuer_yrot         [(6*32) + i + 3 * 16] = 90.50058f;    

        //-- rollfeld 32l links
        unterflurfeuer_xpos         [(6*32) + i + 4 * 16] = -729.0474f + (i * rollfeld_32l_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 4 * 16] = 565.3087f + (i * rollfeld_32l_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 4 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 4 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 4 * 16] = ROLLFELD_32L;
        unterflurfeuer_yrot         [(6*32) + i + 4 * 16] = -89.57336f;

        //-- rollfeld 32r links
        unterflurfeuer_xpos         [(6*32) + i + 5 * 16] = -2204.443f + (i * rollfeld_32r_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 5 * 16] = -636.0308f + (i * rollfeld_32r_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 5 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 5 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 5 * 16] = ROLLFELD_32R;
        unterflurfeuer_yrot         [(6*32) + i + 5 * 16] = -90.50058f;    

        //-- rollfeld 06 rechts -------------------------------------------------------------
        unterflurfeuer_xpos         [(6*32) + i + 6 * 16] = 84.42758f + (i * rollfeld_06_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 6 * 16] = 1163.362f + (i * rollfeld_06_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 6 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 6 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 6 * 16] = ROLLFELD_06;
        unterflurfeuer_yrot         [(6*32) + i + 6 * 16] = 16.39943f;

        //-- rollfeld 24 rechts
        unterflurfeuer_xpos         [(6*32) + i + 7 * 16] = -585.5432f + (i * rollfeld_24_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 7 * 16] = -1118.426f + (i * rollfeld_24_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 7 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 7 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 7 * 16] = ROLLFELD_24;
        unterflurfeuer_yrot         [(6*32) + i + 7 * 16] = -163.6224f;
    
        //-- rollfeld 14l rechts
        unterflurfeuer_xpos         [(6*32) + i + 8 * 16] = 1434.876f + (i * rollfeld_14l_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 8 * 16] = -604.4645f + (i * rollfeld_14l_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 8 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 8 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 8 * 16] = ROLLFELD_14L;
        unterflurfeuer_yrot         [(6*32) + i + 8 * 16] = 89.57336f;
    
        //-- rollfeld 14r rechts
        unterflurfeuer_xpos         [(6*32) + i + 9 * 16] = 1049.35f + (i * rollfeld_14r_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 9 * 16] = 546.5926f + (i * rollfeld_14r_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 9 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 9 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 9 * 16] = ROLLFELD_14R;
        unterflurfeuer_yrot         [(6*32) + i + 9 * 16] = 90.50058f;    

        //-- rollfeld 32l rechts
        unterflurfeuer_xpos         [(6*32) + i + 10 * 16] = -729.0474f + (i * rollfeld_32l_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 10 * 16] = 534.58f + (i * rollfeld_32l_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 10 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 10 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 10 * 16] = ROLLFELD_32L;
        unterflurfeuer_yrot         [(6*32) + i + 10 * 16] = -89.57336f;

        //-- rollfeld 32r rechts
        unterflurfeuer_xpos         [(6*32) + i + 11 * 16] = -2204.443f + (i * rollfeld_32r_xadd);
        unterflurfeuer_zpos         [(6*32) + i + 11 * 16] = -593.3477f + (i * rollfeld_32r_zadd);
        unterflurfeuer_farbe        [(6*32) + i + 11 * 16] = FARBE_ROT;
        unterflurfeuer_lauflicht_id [(6*32) + i + 11 * 16] = i & 15;
        unterflurfeuer_rollfeld     [(6*32) + i + 11 * 16] = ROLLFELD_32R;
        unterflurfeuer_yrot         [(6*32) + i + 11 * 16] = -90.50058f;    
    }



    //--------------------------------------------------
    //--------------------------------------------------
    //--------------------------------------------------
    //
    // anflugfeuersystem atr mast käfig erzeugen
    //
    rollfeld_06_xstart = 7.33979f;
    rollfeld_06_zstart = 1077.728f;
    rollfeld_06_xend = -133.4441f;
    rollfeld_06_zend = 587.3022f;

    rollfeld_24_xstart = -540.915f;
    rollfeld_24_zstart = -1089.225f;
    rollfeld_24_xend = -415.3035f;
    rollfeld_24_zend = -645.9675f;

    rollfeld_14l_xstart = 1158.626f;
    rollfeld_14l_zstart = -627.3701f;
    rollfeld_14l_xend = -179.384f;
    rollfeld_14l_zend = -639.3806f;

    rollfeld_14r_xstart = 879.6896f;
    rollfeld_14r_zstart = 598.9758f;
    rollfeld_14r_xend = 230.3076f;
    rollfeld_14r_zend = 594.105f;

    rollfeld_32l_xstart = -648.9133f;
    rollfeld_32l_zstart = 518.9541f;
    rollfeld_32l_xend = -167.4448f;
    rollfeld_32l_zend = 522.29f;

    rollfeld_32r_xstart = -2165.917f;
    rollfeld_32r_zstart = -656.4094f;
    rollfeld_32r_xend = -749.0538f;
    rollfeld_32r_zend = -644.2979f;
    
    rollfeld_06_xadd = (rollfeld_06_xend - rollfeld_06_xstart) / 16.0f;
    rollfeld_06_zadd = (rollfeld_06_zend - rollfeld_06_zstart) / 16.0f;

    rollfeld_24_xadd = (rollfeld_24_xend - rollfeld_24_xstart) / 16.0f;
    rollfeld_24_zadd = (rollfeld_24_zend - rollfeld_24_zstart) / 16.0f;

    rollfeld_14l_xadd = (rollfeld_14l_xend - rollfeld_14l_xstart) / 32.0f;
    rollfeld_14l_zadd = (rollfeld_14l_zend - rollfeld_14l_zstart) / 32.0f;

    rollfeld_14r_xadd = (rollfeld_14r_xend - rollfeld_14r_xstart) / 16.0f;
    rollfeld_14r_zadd = (rollfeld_14r_zend - rollfeld_14r_zstart) / 16.0f;

    rollfeld_32l_xadd = (rollfeld_32l_xend - rollfeld_32l_xstart) / 16.0f;
    rollfeld_32l_zadd = (rollfeld_32l_zend - rollfeld_32l_zstart) / 16.0f;

    rollfeld_32r_xadd = (rollfeld_32r_xend - rollfeld_32r_xstart) / 32.0f;
    rollfeld_32r_zadd = (rollfeld_32r_zend - rollfeld_32r_zstart) / 32.0f;

    for(i=0;i<32;i++)
    {
        //-- rollfeld 14L
        anflugfeuersystem_atr_mast_kaefig_xpos[i + 0 * 32] = rollfeld_14l_xstart + (i * rollfeld_14l_xadd);
        anflugfeuersystem_atr_mast_kaefig_zpos[i + 0 * 32] = rollfeld_14l_zstart + (i * rollfeld_14l_zadd);
        anflugfeuersystem_atr_mast_kaefig_yrot[i + 0 * 32] = 89.68628f;

        //-- rollfeld 32R
        anflugfeuersystem_atr_mast_kaefig_xpos[i + 1 * 32] = rollfeld_32r_xstart + (i * rollfeld_32r_xadd);
        anflugfeuersystem_atr_mast_kaefig_zpos[i + 1 * 32] = rollfeld_32r_zstart + (i * rollfeld_32r_zadd);
        anflugfeuersystem_atr_mast_kaefig_yrot[i + 1 * 32] = -90.69434f;
    }

    for(i=0;i<16;i++)
    {
        //-- rollfeld 06
        anflugfeuersystem_atr_mast_kaefig_xpos[64 + i + 0 * 16] = rollfeld_06_xstart + (i * rollfeld_06_xadd);
        anflugfeuersystem_atr_mast_kaefig_zpos[64 + i + 0 * 16] = rollfeld_06_zstart + (i * rollfeld_06_zadd);
        anflugfeuersystem_atr_mast_kaefig_yrot[64 + i + 0 * 16] = 14.582f;

        //-- rollfeld 24
        anflugfeuersystem_atr_mast_kaefig_xpos[64 + i + 1 * 16] = rollfeld_24_xstart + (i * rollfeld_24_xadd);
        anflugfeuersystem_atr_mast_kaefig_zpos[64 + i + 1 * 16] = rollfeld_24_zstart + (i * rollfeld_24_zadd);
        anflugfeuersystem_atr_mast_kaefig_yrot[64 + i + 1 * 16] = 195.0644f;

        //-- rollfeld 32l
        anflugfeuersystem_atr_mast_kaefig_xpos[64 + i + 2 * 16] = rollfeld_32l_xstart + (i * rollfeld_32l_xadd);
        anflugfeuersystem_atr_mast_kaefig_zpos[64 + i + 2 * 16] = rollfeld_32l_zstart + (i * rollfeld_32l_zadd);
        anflugfeuersystem_atr_mast_kaefig_yrot[64 + i + 2 * 16] = -90.69434f;
    
        //-- rollfeld 14r
        anflugfeuersystem_atr_mast_kaefig_xpos[64 + i + 3 * 16] = rollfeld_14r_xstart + (i * rollfeld_14r_xadd);
        anflugfeuersystem_atr_mast_kaefig_zpos[64 + i + 3 * 16] = rollfeld_14r_zstart + (i * rollfeld_14r_zadd);
        anflugfeuersystem_atr_mast_kaefig_yrot[64 + i + 3 * 16] = 89.68628f;
    }


}
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//
// Update
//
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
bool LodCheck(float xpos, float zpos, float dist_near, float dist_far)
{    
    float distance = ((xpos - Camera.main.transform.position.x) * (xpos - Camera.main.transform.position.x)) + ((zpos - Camera.main.transform.position.z) * (zpos - Camera.main.transform.position.z));

    if( (distance >= (dist_near * dist_near)) && (distance < (dist_far * dist_far)) )
        return true;            

    return false;
}    
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//
// Update
//
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
void Update()
{
    int i;
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    unterflurfeuer_lauflicht_timer -= Time.deltaTime;
    if(unterflurfeuer_lauflicht_timer < 0.0f)
    {
        unterflurfeuer_lauflicht_timer += 0.12f;
        unterflurfeuer_lauflicht_active++;
        if(unterflurfeuer_lauflicht_active >= 16)
            unterflurfeuer_lauflicht_active = 0;
    }
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    // 
    // anflug gleitwinkel feuer erzeugen
    //
    for(i=0;i<MAX_ANFLUG_GLEITWINKEL_FEUER;i++)
    {
        //-- check distance                        
        if(LodCheck(anflug_gleitwinkelfeuer_position[i * 2 + 0], anflug_gleitwinkelfeuer_position[i * 2 + 1], 0.0f, glideAngleFireMaxViewDistance) == true)
        {
            GlideAngleApproachLightsStatus status = GlideAngleApproachLightsStatus.Off;
            switch(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 0])
            {     
                case 0:{status = glideApproachFireStatusRunway04;};break;  
                case 1:{status = glideApproachFireStatusRunway24;};break;  
                case 2:{status = glideApproachFireStatusRunway14l;};break;  
                case 3:{status = glideApproachFireStatusRunway14r;};break;  
                case 4:{status = glideApproachFireStatusRunway32l;};break;  
                case 5:{status = glideApproachFireStatusRunway32r;};break;  
            }
            //------------------------------------------------------------------------------------
            if((int)status != gleitwinkelfeuer_last_frame_status[i])
            {
                //-- altes objekt löschen
                if(gleitwinkelfeuer_obj[i] != null)
                    Destroy(gleitwinkelfeuer_obj[i]);                   
                //------------------------------------------------------------------------------------
                if(status == GlideAngleApproachLightsStatus.Off)
                {
                    //-- alle auf aus
                    GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_aus);
                    gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                    gleitwinkelfeuer.transform.parent = airportLightsGroup;
                    gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                    gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                }
                //------------------------------------------------------------------------------------
                if(status == GlideAngleApproachLightsStatus.Much_too_high)
                {
                    //-- alle auf aus
                    GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                    gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                    gleitwinkelfeuer.transform.parent = airportLightsGroup;
                    gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                    gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                }
                //------------------------------------------------------------------------------------
                if(status == GlideAngleApproachLightsStatus.Too_high)            
                {
                    //-- ganz aussen
                    if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 0)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- aussen
                    else if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 1)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- innne
                    else if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 2)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- ganz innen
                    else
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                }
                //------------------------------------------------------------------------------------
                if(status == GlideAngleApproachLightsStatus.Ok)            
                {
                    //-- ganz aussen
                    if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 0)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- aussen
                    else if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 1)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- innne
                    else if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 2)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- ganz innen
                    else
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                }
                //------------------------------------------------------------------------------------
                if(status == GlideAngleApproachLightsStatus.Much_to_low)            
                {
                    //-- ganz aussen
                    if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 0)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_weiss);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- aussen
                    else if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 1)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- innne
                    else if(anflug_gleitwinkelfeuer_rollfeld_anordnung[i * 2 + 1] == 2)       
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                    //-- ganz innen
                    else
                    {
                        GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                        gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                        gleitwinkelfeuer.transform.parent = airportLightsGroup;
                        gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                        gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                    }
                }
                //------------------------------------------------------------------------------------
                if(status == GlideAngleApproachLightsStatus.Much_to_low)
                {
                    //-- alle auf aus
                    GameObject gleitwinkelfeuer = (GameObject)Instantiate(anflug_gleitwinkel_feuer_an_rot);
                    gleitwinkelfeuer_obj[i] = gleitwinkelfeuer;
                    gleitwinkelfeuer.transform.parent = airportLightsGroup;
                    gleitwinkelfeuer.transform.localPosition    = new Vector3 (anflug_gleitwinkelfeuer_position[i * 2 + 0], 0.0f, anflug_gleitwinkelfeuer_position[i * 2 + 1]);
                    gleitwinkelfeuer.transform.localEulerAngles = new Vector3 (0.0f, anflug_gleitwinkelfeuer_rotation[i], 0.0f);
                }
                //-- aktuellen status speichern
                gleitwinkelfeuer_last_frame_status[i] = (int)status;
            }
        //------------------------------------------------------------------------------------
        }
        else //-- wenn außer sichtweite
        {
            if(gleitwinkelfeuer_last_frame_status[i] != -1)
            {
                if(gleitwinkelfeuer_obj[i] != null)
                    Destroy(gleitwinkelfeuer_obj[i]);                   
                gleitwinkelfeuer_obj[i] = null;
                gleitwinkelfeuer_last_frame_status[i] = -1;
            }
        }
        //------------------------------------------------------------------------------------
    }

    
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    // 
    // Startbahn Endfeuer erzeugen
    //
    for(i=0;i<MAX_STARTBAHN_ENDFEUER;i++)
    {
        //-- check distance                        
        if(LodCheck(startbahn_endfeuer_position[i * 2 + 0], startbahn_endfeuer_position[i * 2 + 1], 0.0f, rundwayEndfireMaxViewDistance) == true)
        {
            RunwayEndfireStatus status = 0;
            switch(startbahn_endfeuer_rollfeld[i])
            {     
                case 0:{status = rundwayEndfireStatusRunway04;};break;  
                case 1:{status = rundwayEndfireStatusRunway24;};break;  
                case 2:{status = rundwayEndfireStatusRunway14l;};break;  
                case 3:{status = rundwayEndfireStatusRunway14r;};break;  
                case 4:{status = rundwayEndfireStatusRunway32l;};break;  
                case 5:{status = rundwayEndfireStatusRunway32r;};break;  
            }
            //------------------------------------------------------------------------------------
            if((int)status != startbahn_endfeuer_last_frame_status[i])
            {
                //-- altes objekt löschen
                if(startbahn_endfeuer_obj[i] != null)
                    Destroy(startbahn_endfeuer_obj[i]);                   
                //------------------------------------------------------------------------------------
                if(status == RunwayEndfireStatus.Start)
                {
                    //-- grüne leuchte 
                    GameObject startbahnendfeuer = (GameObject)Instantiate(startbahn_endfeuer_gruen);
                    
                    startbahn_endfeuer_obj[i] = startbahnendfeuer;
                    startbahnendfeuer.transform.parent = airportLightsGroup;
                    startbahnendfeuer.transform.localPosition    = new Vector3 (startbahn_endfeuer_position[i * 2 + 0], 0.0f, startbahn_endfeuer_position[i * 2 + 1]);
                    startbahnendfeuer.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 0.0f);
                }
                //------------------------------------------------------------------------------------
                else
                {
                    //-- rote leuchte
                    GameObject startbahnendfeuer = (GameObject)Instantiate(startbahn_endfeuer_rot);
                    
                    startbahn_endfeuer_obj[i] = startbahnendfeuer;
                    startbahnendfeuer.transform.parent = airportLightsGroup;
                    startbahnendfeuer.transform.localPosition    = new Vector3 (startbahn_endfeuer_position[i * 2 + 0], 0.0f, startbahn_endfeuer_position[i * 2 + 1]);
                    startbahnendfeuer.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, 0.0f);
                }
                //-- aktuellen status speichern
                startbahn_endfeuer_last_frame_status[i] = (int)status;
            }
        //------------------------------------------------------------------------------------
        }
        else //-- wenn außer sichtweite
        {
            if(startbahn_endfeuer_last_frame_status[i] != -1)
            {
                if(startbahn_endfeuer_obj[i] != null)
                    Destroy(startbahn_endfeuer_obj[i]);                   
                startbahn_endfeuer_obj[i] = null;
                startbahn_endfeuer_last_frame_status[i] = -1;
            }
        }
        //------------------------------------------------------------------------------------
    }

    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    // 
    // unterflur feuer erzeugen
    //
    for(i=0;i<MAX_UNTERFLUR_FEUER;i++)
    {
        //-- check distance                        
        if(LodCheck(unterflurfeuer_xpos[i], unterflurfeuer_zpos[i], 0.0f, underfloorFireMaxViewDistance) == true)
        {
            RunwayUnderfloorFireStatus status = 0;
            switch(unterflurfeuer_rollfeld[i])
            {     
                case 0:{status = underfloorfireStatusRunyway06;};break;  
                case 1:{status = underfloorfireStatusRunyway24;};break;  
                case 2:{status = underfloorfireStatusRunyway14l;};break;  
                case 3:{status = underfloorfireStatusRunyway14r;};break;  
                case 4:{status = underfloorfireStatusRunyway32l;};break;  
                case 5:{status = underfloorfireStatusRunyway32r;};break;  
            }
            //------------------------------------------------------------------------------------
            int draw_status = -1;         
            //------------------------------------------------------------------------------------
            if(status == RunwayUnderfloorFireStatus.Off)           
            {
                //-- lampe aus
                draw_status = 0;    // unterflurfeuer_aus
            }
            //------------------------------------------------------------------------------------
            else if(status == RunwayUnderfloorFireStatus.On)
            {
                //-- lampe an
                if(unterflurfeuer_farbe[i] == FARBE_ROT)    
                    draw_status = 1;    // unterflurfeuer_rot_an
                else
                    draw_status = 2;    // unterflurfeuer_weiss_an
            }
            //------------------------------------------------------------------------------------
            else // lauflicht
            {
                if(unterflurfeuer_lauflicht_id[i] == unterflurfeuer_lauflicht_active)
                {
                    if(unterflurfeuer_farbe[i] == FARBE_ROT)    
                        draw_status = 1;    // unterflurfeuer_rot_an
                    else
                        draw_status = 2;    // unterflurfeuer_rot_an
                }
                else
                    draw_status = 0;    // unterflurfeuer_aus
            }
            //------------------------------------------------------------------------------------
            if(draw_status != unterflurfeuer_last_frame_status[i])
            {
                //-- altes objekt löschen
                if(unterflurfeuer_obj[i] != null)
                    Destroy(unterflurfeuer_obj[i]);                   

                //-- neues objekt erzeugen
                GameObject unterflurfeuer;
                if(draw_status == 0)
                    unterflurfeuer = (GameObject)Instantiate(unterflurfeuer_aus);
                else if(draw_status == 2)
                    unterflurfeuer = (GameObject)Instantiate(unterflurfeuer_rot_an);
                else
                    unterflurfeuer = (GameObject)Instantiate(unterflurfeuer_weiss_an);

                unterflurfeuer_obj[i] = unterflurfeuer;
                unterflurfeuer.transform.parent = airportLightsGroup;
                unterflurfeuer.transform.localPosition    = new Vector3 (unterflurfeuer_xpos[i], 0.0f, unterflurfeuer_zpos[i]);
                unterflurfeuer.transform.localEulerAngles = new Vector3 (0.0f, unterflurfeuer_yrot[i], 0.0f);

                unterflurfeuer_last_frame_status[i] = draw_status;
            }
        //------------------------------------------------------------------------------------
        }
        else //-- wenn außer sichtweite
        {
            if(unterflurfeuer_last_frame_status[i] != -1)
            {
                if(unterflurfeuer_obj[i] != null)
                    Destroy(unterflurfeuer_obj[i]);                   
                unterflurfeuer_obj[i] = null;
                unterflurfeuer_last_frame_status[i] = -1;
            }
        }
        //------------------------------------------------------------------------------------
    }
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    // 
    // Anflugfeuersystem ATR Mast Käfig erzeugen
    //
    for(i=0;i<MAX_ATR_MAST_KAEFIG;i++)
    {
        //-- check distance                        
        if(LodCheck(anflugfeuersystem_atr_mast_kaefig_xpos[i], anflugfeuersystem_atr_mast_kaefig_zpos[i], 0.0f, atrCageMaxViewDistance) == true)
        {
            //-- einschalten wenn bislang inaktiv war
            if(anflugfeuersystem_atr_mast_kaefig_status[i] == false)
            {
                anflugfeuersystem_atr_mast_kaefig_status[i] = true;

                GameObject atr_mast = (GameObject)Instantiate(anflugfeuersystem_atr_mast_kaefig);                 
                anflugfeuersystem_atr_mast_kaefig_obj[i] = atr_mast;
                atr_mast.transform.parent = airportLightsGroup;
                atr_mast.transform.localPosition    = new Vector3 (anflugfeuersystem_atr_mast_kaefig_xpos[i], -0.12f, anflugfeuersystem_atr_mast_kaefig_zpos[i]);
                atr_mast.transform.localEulerAngles = new Vector3 (0.0f, anflugfeuersystem_atr_mast_kaefig_yrot[i], 0.0f);
            }
        }
        //-- ausschalten wenn bislang aktiv war
        else  
        {
            if(anflugfeuersystem_atr_mast_kaefig_status[i] == true)
            {
                //-- altes objekt löschen
                Destroy(anflugfeuersystem_atr_mast_kaefig_obj[i]); 

                anflugfeuersystem_atr_mast_kaefig_status[i] = false;         
            }
        }
        //------------------------------------------------------------------------------------
    }
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    // 
    // Anflugfeuersystem ATR Mast (mit und ohne Signalleuchten)
    //
    for(i=0;i<MAX_ATR_MAST;i++)
    {
        //-- check distance                        
        if(LodCheck(startbahn_atr_mast_position[i * 2 + 0], startbahn_atr_mast_position[i * 2 + 1], 0.0f, atrRundwayEndMaxViewDistance) == true)
        {
            //-- mast in sichtweite           
            int status = 1;
            //--------------------------------------------------------------------------------
            if(status != anflugfeuersystem_atr_mast_status[i])
            {
                //-- altes objekt löschen
                if(anflugfeuersystem_atr_mast_status[i] != -1)
                    Destroy(anflugfeuersystem_atr_mast_obj[i]); 
                
                GameObject mast_atr_obj;

                switch(startbahn_atr_mast_typ[i])
                {
                    //-----------------------------------------------------
                    case ATR_MAST_TYP_3M_3ER:       {mast_atr_obj = anflugfeuersystem_atr_mast_3m_3er;};break;
                    case ATR_MAST_TYP_3M_3ER_SIGNAL:{mast_atr_obj = anflugfeuersystem_atr_mast_3m_3er;};break;
                    //-----------------------------------------------------
                    case ATR_MAST_TYP_3M_5ER:       {mast_atr_obj = anflugfeuersystem_atr_mast_3m_5er;};break;
                    case ATR_MAST_TYP_3M_5ER_SIGNAL:{mast_atr_obj = anflugfeuersystem_atr_mast_3m_5er;};break;
                    //-----------------------------------------------------
                    case ATR_MAST_TYP_6M_3ER:       {mast_atr_obj = anflugfeuersystem_atr_mast_6m_3er;};break;
                    case ATR_MAST_TYP_6M_3ER_SIGNAL:{mast_atr_obj = anflugfeuersystem_atr_mast_6m_3er;};break;
                    //-----------------------------------------------------
                    case ATR_MAST_TYP_6M_5ER:       {mast_atr_obj = anflugfeuersystem_atr_mast_6m_5er;};break;
                    case ATR_MAST_TYP_6M_5ER_SIGNAL:{mast_atr_obj = anflugfeuersystem_atr_mast_6m_5er;};break;
                    //-----------------------------------------------------
                    case ATR_MAST_TYP_9M_3ER:       {mast_atr_obj = anflugfeuersystem_atr_mast_9m_3er;};break;
                    case ATR_MAST_TYP_9M_3ER_SIGNAL:{mast_atr_obj = anflugfeuersystem_atr_mast_9m_3er;};break;
                    //-----------------------------------------------------
                    case ATR_MAST_TYP_9M_5ER:       {mast_atr_obj = anflugfeuersystem_atr_mast_9m_5er;};break;
                    default:                        {mast_atr_obj = anflugfeuersystem_atr_mast_9m_5er;};break;
                }
                anflugfeuersystem_atr_mast_status[i] = 1;

                GameObject mast_atr = (GameObject)Instantiate(mast_atr_obj);                

                anflugfeuersystem_atr_mast_obj[i] = mast_atr;
                mast_atr.transform.parent = airportLightsGroup;
                mast_atr.transform.localPosition    = new Vector3 (startbahn_atr_mast_position[i * 2 + 0], 0.0f, startbahn_atr_mast_position[i * 2 + 1]);
                mast_atr.transform.localEulerAngles = new Vector3 (0.0f, startbahn_atr_mast_rotation[i], 0.0f);
            }
        }
        //-- wenn mast bislang sichtbar war, dann abschalten
        else 
        {
            if(anflugfeuersystem_atr_mast_status[i] != -1)
            {
                //-- altes objekt löschen
                Destroy(anflugfeuersystem_atr_mast_obj[i]); 

                anflugfeuersystem_atr_mast_status[i] = -1;         
            }        
        }
    }
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    // 
    // Anflugfeuersystem Taxiway ATR Mast (mit und ohne Signalleuchten)
    //
    for(i=0;i<MAX_TAXIWAY_ATR_MAST;i++)
    {
        //-- check distance                        
        if(LodCheck(taxiway_atr_mast_position_rotation_typ[i * 4 + 0], taxiway_atr_mast_position_rotation_typ[i * 4 + 1], 0.0f, atrTaxiwayMaxViewDistance) == true)
        {
            //-- mast in sichtweite           
            int status = 1;
            //--------------------------------------------------------------------------------
            if(status != taxiway_atr_mast_status[i])
            {
                //-- altes objekt löschen
                if(taxiway_atr_mast_status[i] != -1)
                    Destroy(taxiway_atr_mast_obj[i]); 

                GameObject taxiway_atr_obj;

                int type = (int)(taxiway_atr_mast_position_rotation_typ[i * 4 + 3]);
                switch(type)
                {
                    //-----------------------------------------------------
                    // TAXIWAY_ATR_HOCH_EINFACH
                    case 0:{taxiway_atr_obj = taxiway_atr_hoch_signal_blau;};break;
                    case 1:{taxiway_atr_obj = taxiway_atr_hoch_signal_blau;};break;
                    case 2:{taxiway_atr_obj = taxiway_atr_hoch_signal_blau;};break;
                    case 3:{taxiway_atr_obj = taxiway_atr_hoch_signal_blau;};break;
                    case 4:{taxiway_atr_obj = taxiway_atr_hoch_signal_blau;};break;
                    //-----------------------------------------------------
                    // TAXIWAY_ATR_MITTEL_SIGNAL
                    case 5:{taxiway_atr_obj = taxiway_atr_mittel_signal_blau;};break;
                    case 6:{taxiway_atr_obj = taxiway_atr_mittel_signal_blau;};break;
                    case 7:{taxiway_atr_obj = taxiway_atr_mittel_signal_blau;};break;
                    case 8:{taxiway_atr_obj = taxiway_atr_mittel_signal_blau;};break;
                    case 9:{taxiway_atr_obj = taxiway_atr_mittel_signal_blau;};break;
                    //-----------------------------------------------------
                    // TAXIWAY_ATR_NIEDRIG_SIGNAL
                    case 10:{taxiway_atr_obj = taxiway_atr_niedrig_signal_blau;};break;
                    case 11:{taxiway_atr_obj = taxiway_atr_niedrig_signal_blau;};break;
                    case 12:{taxiway_atr_obj = taxiway_atr_niedrig_signal_blau;};break;
                    case 13:{taxiway_atr_obj = taxiway_atr_niedrig_signal_blau;};break;
                    default:{taxiway_atr_obj = taxiway_atr_niedrig_signal_blau;};break;
                    //-----------------------------------------------------
                }
                taxiway_atr_mast_status[i] = 1;

                GameObject mast_atr = (GameObject)Instantiate(taxiway_atr_obj);                

                taxiway_atr_mast_obj[i] = mast_atr;
                mast_atr.transform.parent = airportLightsGroup;
                mast_atr.transform.localPosition    = new Vector3 (taxiway_atr_mast_position_rotation_typ[i * 4 + 0], 0.0f, taxiway_atr_mast_position_rotation_typ[i * 4 + 1]);
                mast_atr.transform.localEulerAngles = new Vector3 (0.0f, taxiway_atr_mast_position_rotation_typ[i * 4 + 2], 0.0f);
            }
        }
        //-- wenn mast bislang sichtbar war, dann abschalten
        else 
        {
            if(taxiway_atr_mast_status[i] != -1)
            {
                //-- altes objekt löschen
                Destroy(taxiway_atr_mast_obj[i]); 

                taxiway_atr_mast_status[i] = -1;         
            }        
        }
    }
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------


}
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------
}
