namespace ColorSorting3D.Config
{
    public enum RingID
    {
        None = 0,
        Blue,
        Green,
        Orange,
        Pink,
        Purple,
        Yellow,
    }

    public enum PresentationID
    {
        None = 0,
        Loading,
        Home,
        HUD,
        CompleteLevel,
        Modal,
    }

    public enum PresentationTransitionID
    {
        None = 0,
        SetLoading,
        LoadingToHome,
        HomeToHUD,
        HUDToCompleteLevel,
        CompleteLevelToHUD,
        CompleteLevelToHome,
        PopUpModal,
        PopOutModal,
        ReloadHUD,
        HUDToHome,
    }
}