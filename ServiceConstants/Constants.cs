using System.Runtime.Serialization;

namespace  ServiceTypes.Constants
{
    #region task reg
    /// <summary>
    /// Describes enumeration task types { VibroInsertion,  [EnumMember] Flap,  [EnumMember] Roof,  [EnumMember] MountingFrame,  [EnumMember] Panel,  [EnumMember] HousingBlock,  [EnumMember] Monoblock,  [EnumMember] Frameless } and their numeric ServiceTypes.Constants
    /// </summary>
    [DataContract(Name = "TasksType_e")]
    public enum TasksType_e
    {
        [EnumMember]
        VibroInsertion = 1,
        [EnumMember]
        Flap = 2,
        [EnumMember]
        Roof = 3,
        [EnumMember]
        MountingFrame = 4,
        [EnumMember]
        Panel = 5,
        [EnumMember]
        HousingBlock = 6,
        [EnumMember]
        Monoblock = 7,
        [EnumMember]
        Frameless = 8,
        [EnumMember]
        Pdf = 9,
        [EnumMember]
        Dxf = 10 
    }


    /// <summary>
    /// Describes enumeration task statuses {completed,  [EnumMember] waiting,  [EnumMember] error,  [EnumMember] execution} and their numeric ServiceTypes.Constants
    /// </summary>
    [DataContract(Name = "TaskStatus_e")]
    public enum TaskStatus_e
    {
        [EnumMember]
        Completed = 1,
        [EnumMember]
        Waiting = 2,
        [EnumMember]
        Error = 3,
        [EnumMember]
        Execution = 4    
    }
    #endregion


    /// <summary>
    /// Describes spigon type and their values { 20mm,  [EnumMember] 30mm }
    /// </summary>
     [DataContract(Name = "SpigotType_e")]
    public enum SpigotType_e
    {
        [EnumMember]      
        Twenty_mm = 20,  
        [EnumMember]
        Thirty_mm = 30
    }
    /// <summary>
    /// Describes vibro insertion type and their values { from one to six }
    /// </summary>
    [DataContract(Name = "RoofType_e")]
    public enum RoofType_e
    {
        [EnumMember]
        One = 1,  
        [EnumMember]
        Two = 2,   
        [EnumMember]
        Three = 3,   
        [EnumMember]
        Four = 4, 
        [EnumMember]
        Five = 5, 
        [EnumMember]
        Six = 6
    }

    /// <summary>
    /// Describes Flape types ant their values
    /// </summary>
    [DataContract(Name = "FlapTypes_e")]
    public enum FlapTypes_e
    {
        [EnumMember]
        Twenty_mm = 20,   
        [EnumMember]
        Thirty_mm = 30
    }


    /// <summary>
    /// Describes Flape types ant their id [customize]
    /// </summary>
    [DataContract(Name = "Materials_e")]
    public enum Materials_e
    {
        [EnumMember]
        Aluzinc_Az_150_07 = 1,  
        [EnumMember]
        Sheet_Galvanized_Zinc = 2,  
        [EnumMember]
        Sheet_Cold_Hardened = 3,   
        [EnumMember]
        Sheet_A_304_2B = 4,  
        [EnumMember]
        Sheet_A_304_BA = 5,   
        [EnumMember]
        Sheet_A_304_CAT = 6,  
        [EnumMember]
        Sheet_Hot_Hardened = 7,   
        [EnumMember]
        Steel_09G2S_GOST_4543_71 = 8,   
          [EnumMember]
        Sheet_A_3095_2B = 9
    }

    /// <summary>
    /// Describes panel profiles
    /// </summary>
    [DataContract(Name = "PanelProfile_e")]
    public enum PanelProfile_e
    {
        [EnumMember]
        Profile_3_0 = 30,
        [EnumMember]
        Profile_5_0 = 50,
        [EnumMember]
        Profile_7_0 = 70
    }
    #region panels 
    /// <summary>
    /// Describes panel types
    /// </summary>
    [DataContract(Name = "PanelType_e")]
    public enum PanelType_e {
        [EnumMember]
        BlankPanel = 1,
        [EnumMember]                                                 
        RemovablePanel = 4,
        [EnumMember]                                                  
        безКрыши = 21,
        [EnumMember]
        односкат = 22,
        [EnumMember]
        Двухскат = 23,
        [EnumMember]
        безОпор = 30,
        [EnumMember]
        РамаМонтажная = 31,
        [EnumMember]
        НожкиОпорные = 32,
        [EnumMember]                                                
        FrontPanel = 35,
        [EnumMember]
        ПростаяУсилПанель = 24,
        [EnumMember]
        ПодДвериНаПетлях = 25,
        [EnumMember]
        ПоДвериНаЗажимах = 26,
        [EnumMember]
        ПодТорцевую = 27,
        [EnumMember]
        ПодТорцевуюИДвериНаЗажимах = 28,
        [EnumMember]
        ПодТорцевуюИДвериНаПетлях = 29,
        #region system ServiceTypes.Constants  
        [EnumMember]
        Insulation = 0,
        [EnumMember]
        SealingTape = 0,
        [EnumMember]
        WithScotch = 0,
        [EnumMember]
        WithoutScotch = 0
        #endregion

        // ThePanelHeatExchanger = 6,  [EnumMember]  
        //  PanelHinged = 3,  [EnumMember]
        //Панель  теплообменника     02-05-ХХХХ
    }

    /// <summary>
    /// Desctibes elemet types for panel
    /// </summary>

    [DataContract(Name = "ElemetPanelType_e")]
    public enum ElemetPanelType_e
    {
        [EnumMember]
        Inner_panel = 1,
        [EnumMember]
        Outer_panel = 2,
        [EnumMember]
        Inner_panel_left,
        [EnumMember]
        Inner_panel_right,
        [EnumMember]
        Outer_panel_left,
        [EnumMember]
        Outer_panel_right,
        [EnumMember]
        Insulation = 3,
        [EnumMember]
        Scotch = 4,
        [EnumMember]
        Sealing_tape = 5,
        [EnumMember]
        Strengthening_frame_by_width_bottom = 6,
        [EnumMember]
        Trengthening_frame_by_width_top = 62,
        [EnumMember]
        Strengthening_frame_by_width_height = 7,
        [EnumMember]
        Door_bracket = 9,
        [EnumMember]
        Profile_front_panel_horizontal = 12,
        [EnumMember]
        Profile_front_panel_vertical = 12
    }

    #endregion panels


    [DataContract(Name = "ThermoStrip_e")]
    public enum  ThermoStrip_e
    {
        [EnumMember]
        ThermoScotch  = 1,
        [EnumMember]
        Rivet = 2
    } 

    /// <summary>
    /// Describes pdm types
    /// </summary>
    [DataContract(Name = "PdmType_e")]
    public enum PdmType_e
    {
        [EnumMember]
        SolidWorksPdm = 1,
        [EnumMember]
        IPS = 2
    }
}