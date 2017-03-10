namespace ServiceConstants
{
    #region task reg
    /// <summary>
    /// Describes enumeration task types { VibroInsertion, Flap, Roof, MountingFrame, Panel, HousingBlock, Monoblock, Frameless } and their numeric constants
    /// </summary>
    public enum TasksType_e
    {
        VibroInsertion = 1,
        Flap = 2,
        Roof = 3,
        MountingFrame = 4,
        Panel = 5,
        HousingBlock = 6,
        Monoblock = 7,
        Frameless = 8,
        Pdf = 9,
        Dxf = 10 
    }


    /// <summary>
    /// Describes enumeration task statuses {completed, waiting, error, execution} and their numeric constants
    /// </summary>
    public enum TaskStatus_e
    {
        Completed = 1,
        Waiting = 2,
        Error = 3,
        Execution = 4,       
    }
    #endregion

 
    /// <summary>
    /// Describes spigon type and their values { 20mm, 30mm }
    /// </summary>
    public enum SpigotType_e
    {
        Twenty_mm = 20, Thirty_mm = 30
    }
    /// <summary>
    /// Describes vibro insertion type and their values { from one to six }
    /// </summary>
    public enum RoofType_e
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6
    }

    /// <summary>
    /// Describes Flape types ant their values
    /// </summary>
    public enum FlapTypes_e
    {
        Twenty_mm = 20, Thirty_mm = 30
    }
 

    /// <summary>
    /// Describes Flape types ant their id [customize]
    /// </summary>
    public enum Materials_e
    {
        Aluzinc_Az_150_07 = 1,
        Sheet_Galvanized_Zinc = 2,
        Sheet_Cold_Hardened = 3,
        Sheet_A_304_2B = 4,
        Sheet_A_304_BA = 5,
        Sheet_A_304_CAT = 6,
        Sheet_Hot_Hardened = 7,
        Steel_09G2S_GOST_4543_71 = 8,
        Sheet_A_3095_2B = 9
    }

    /// <summary>
    /// Describes panel profiles
    /// </summary>
    public enum PanelProfile_e
    {
        Profile_3_0 = 30,
        Profile_5_0 = 50,
        Profile_7_0 = 70
    }
    #region panels 
    /// <summary>
    /// Describes panel types
    /// </summary>
    public enum PanelType_e {
        BlankPanel = 1,                                                 
        RemovablePanel = 4,                                                  
        безКрыши = 21,
        односкат = 22,
        Двухскат = 23,
        безОпор = 30,
        РамаМонтажная = 31,
        НожкиОпорные = 32,                                                
        FrontPanel = 35,
        ПростаяУсилПанель = 24,
        ПодДвериНаПетлях = 25,
        ПоДвериНаЗажимах = 26,
        ПодТорцевую = 27,
        ПодТорцевуюИДвериНаЗажимах = 28,
        ПодТорцевуюИДвериНаПетлях = 29,
        #region system constants
        InsulationWool = 0,
        SealingTape = 0,
        WithScotch = 0,
        withoutScotch = 0
        #endregion

        // ThePanelHeatExchanger = 6,  
        //  PanelHinged = 3,
        //Панель  теплообменника     02-05-ХХХХ
    }

    /// <summary>
    /// Desctibes elemet types for panel
    /// </summary>
    public enum ElemetPanelType_e
    {
        Inner_panel = 1,
        Outer_panel = 2,
        Inner_panel_left,
        Inner_panel_right,
        Outer_panel_left,
        Outer_panel_right,
        Insulation = 3,
        Scotch = 4,
        Sealing_Tape = 5,
        Strengthening_frame_by_width_bottom = 6,
        trengthening_Frame_By_Width_Top = 62,
        Strengthening_Frame_By_Width_Height = 7,
        Door_Bracket = 9,
        Profile_Front_Panel_Horizontal = 12,
        Profile_Front_Panel_Vertical = 12
    }

    #endregion panels



    public enum  ThermoStrip_e
    {
        ThermoScotch  = 1,
        Rivet = 2
    } 

    /// <summary>
    /// Describes pdm types
    /// </summary>
    public enum PdmType_e
    {
        SolidWorksPdm = 1,
        IPS = 2
    }
}