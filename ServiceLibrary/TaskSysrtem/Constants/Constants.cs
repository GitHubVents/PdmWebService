namespace ServiceLibrary.TaskSystem.Constants
{

    /// <summary>
    /// Contain enumeration task types { VibroInsertion, Flap, Roof, MountingFrame, Panel, HousingBlock, Monoblock, Frameless } and their numeric constants
    /// </summary>
    public enum TasksTypes
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
        Dxf = 10,
        None = 0
    }


    /// <summary>
    /// Contain enumeration task statuses {completed, waiting, error, execution} and their numeric constants
    /// </summary>
    public enum TaskStatuses
    {
        Completed = 1, Waiting = 2, Error = 3, Execution = 4, Empty = 0, All = 5 // All need for view all tasks 
    }

    /// <summary>
    /// Containe vibro insertion type and their values { 20mm, 30mm }
    /// </summary>
    public enum VibroInsertionTypes
    {
        Twenty_mm = 20, Thirty_mm = 30
    }
    /// <summary>
    /// Containe vibro insertion type and their values { from one to six }
    /// </summary>
    public enum RoofTypes
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6
    }

    /// <summary>
    /// Containe Flape types ant their values
    /// </summary>
    public enum FlapTypes
    {
        Twenty_mm = 20, Thirty_mm = 30
    }

    
    /// <summary>
    /// Containe Flape types ant their id [customize]
    /// </summary>
    public enum Meterials
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


    public enum PanelProfiles
    {
        Profile_3_0 = 30,
        Profile_5_0 = 50,
        Profile_7_0 = 70
    }

    public enum PanelTypes
    {
        NotRemovableBlankPanel = 1,
        DualNotRemovablePanel = 2,
        PanelHinged = 3,
        PanelRemovableWithHandles = 4,
        DualRemovablePpanel = 5,
        ThePanelHeatExchanger = 6,
        TheFrontPanel = 7
    }

    #region material enums It contains everything a need all
    //public enum PanelMeterials
    //{

    //    Aluzinc_Az_150_07 = 1,
    //    Sheet_Galvanized_Zinc = 2,
    //    Sheet_Cold_Hardened = 3,
    //    Sheet_A_304_2B = 4,
    //    Sheet_A_304_BA = 5,
    //    Sheet_A_304_CAT = 6,
    //    Sheet_Hot_Hardened = 7,
    //    Steel_09G2S_GOST_4543_71 = 8,
    //    Sheet_A_3095_2B = 9
    //}
    #endregion

}